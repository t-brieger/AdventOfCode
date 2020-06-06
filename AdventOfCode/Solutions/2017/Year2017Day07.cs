using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2017Day07 : Solution
    {
        public override string Part1(string input)
        {
            Dictionary<string, HashSet<string>> lines = new Dictionary<string, HashSet<string>>();
            foreach (string line in input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string value in parts.Skip(3))
                    if (lines.ContainsKey(parts[0]))
                        lines[parts[0]].Add(value);
                    else
                        lines.Add(parts[0], new HashSet<string>(new[] { value }));
            }


            foreach (string key in lines.Keys.Where(key => lines.TakeWhile((t, i) => !lines.ElementAt(i).Value.Contains(key)).Where((t, i) => i == lines.Count - 1).Any()))
            {
                return key;
            }

            return "";
        }

        private class Tree<T>
        {
            public Tree<T>[] children;
            public string id;
            public T value;
        }

        private static string GetRootNode(Dictionary<string, Tuple<HashSet<string>, int>> lines)
        {
            foreach (string key in lines.Keys)
            {
                if (lines.TakeWhile((t, i) => !lines.ElementAt(i).Value.Item1.Contains(key)).Where((t, i) => i == lines.Count - 1).Any())
                {
                    return key;
                }
            }

            return null;
        }

        private static void PopulateTree(Tree<int> t, IReadOnlyDictionary<string, Tuple<HashSet<string>, int>> lines)
        {
            t.value = lines[t.id].Item2;
            List<Tree<int>> childrenToBeAdded = new List<Tree<int>>();
            foreach (string child in lines[t.id].Item1)
            {
                Tree<int> childNode = new Tree<int> {id = child};
                PopulateTree(childNode, lines);
                childrenToBeAdded.Add(childNode);
            }

            t.children = childrenToBeAdded.ToArray();
        }

        private static void MakeCumulative(Tree<int> t)
        {
            foreach (Tree<int> child in t.children)
            {
                MakeCumulative(child);
            }

            foreach (Tree<int> child in t.children)
            {
                t.value += child.value;
            }
        }

        public override string Part2(string input)
        {
            Dictionary<string, Tuple<HashSet<string>, int>> lines = new Dictionary<string, Tuple<HashSet<string>, int>>();
            foreach (string[] parts in input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)))
            {
                foreach (string value in parts.Skip(3))
                {
                    if (lines.ContainsKey(parts[0]))
                        lines[parts[0]].Item1.Add(value);
                    else
                        lines.Add(parts[0],
                            new Tuple<HashSet<string>, int>(new HashSet<string>(new[] { value }),
                                Int32.Parse(parts[1].Substring(1, parts[1].Length - 2))));
                }
                if (!lines.ContainsKey(parts[0]))
                    lines.Add(parts[0], new Tuple<HashSet<string>, int>(new HashSet<string>(), Int32.Parse(parts[1].Substring(1, parts[1].Length - 2))));
            }

            Tree<int> tree = new Tree<int> {id = GetRootNode(lines)};
            PopulateTree(tree, lines);
            MakeCumulative(tree);
            return "";
        }
    }
}