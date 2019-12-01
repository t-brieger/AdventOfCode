using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2017
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


            foreach (string key in lines.Keys)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines.ElementAt(i).Value.Contains(key))
                        break;
                    if (i == lines.Count - 1)
                        return key;
                }
            }

            return "";
        }

        private class Tree<T>
        {
            public Tree(Tree<T>[] children, Tree<T> parent, string value, T id)
            {
                this.children = children;
                this.parent = parent;
                this.id = value;
                this.value = id;
            }

            public Tree() { }

            public Tree<T>[] children;
            public Tree<T> parent = null;
            public string id;
            public T value;
        }

        private static string getRootNode(Dictionary<string, Tuple<HashSet<string>, int>> lines)
        {
            foreach (string key in lines.Keys)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines.ElementAt(i).Value.Item1.Contains(key))
                        break;
                    if (i == lines.Count - 1)
                        return key;
                }
            }

            return null;
        }

        private static void populateTree(Tree<int> t, Dictionary<string, Tuple<HashSet<string>, int>> lines)
        {
            t.value = lines[t.id].Item2;
            List<Tree<int>> childrenToBeAdded = new List<Tree<int>>();
            foreach (string child in (lines[t.id].Item1))
            {
                Tree<int> childNode = new Tree<int>();
                childNode.id = child;
                childNode.parent = t;
                populateTree(childNode, lines);
                childrenToBeAdded.Add(childNode);
            }

            t.children = childrenToBeAdded.ToArray();
        }

        private static void makeCumulative(Tree<int> t)
        {
            foreach (Tree<int> child in t.children)
            {
                makeCumulative(child);
            }

            foreach (Tree<int> child in t.children)
            {
                t.value += child.value;
            }
        }

        private static int getUnbalancedChild(Tree<int> t)
        {
            //TODO!!!!!
            return -1;
        }

        public override string Part2(string input)
        {
            Dictionary<string, Tuple<HashSet<string>, int>> lines = new Dictionary<string, Tuple<HashSet<string>, int>>();
            foreach (string line in input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string value in parts.Skip(3))
                {
                    if (lines.ContainsKey(parts[0]))
                        lines[parts[0]].Item1.Add(value);
                    else
                        lines.Add(parts[0],
                            new Tuple<HashSet<string>, int>(new HashSet<string>(new[] { value }),
                                int.Parse(parts[1].Substring(1, parts[1].Length - 2))));
                }
                if (!lines.ContainsKey(parts[0]))
                    lines.Add(parts[0], new Tuple<HashSet<string>, int>(new HashSet<string>(), int.Parse(parts[1].Substring(1, parts[1].Length - 2))));
            }

            Tree<int> tree = new Tree<int>();
            tree.id = getRootNode(lines);
            populateTree(tree, lines);
            makeCumulative(tree);
            return "";
        }
    }
}