using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2017Day07 : Solution
{
    public override string Part1(string input)
    {
        Dictionary<string, HashSet<string>> lines = new();
        foreach (string line in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            string[] parts = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string value in parts.Skip(3))
                if (lines.ContainsKey(parts[0]))
                    lines[parts[0]].Add(value);
                else
                    lines.Add(parts[0], new HashSet<string>(new[] { value }));
        }


        foreach (string key in lines.Keys.Where(key =>
                     lines.TakeWhile((_, i) => !lines.ElementAt(i).Value.Contains(key)).Where((_, i) => i == lines.Count - 1)
                         .Any())) return key;

        return "";
    }

    private static string GetRootNode(Dictionary<string, Tuple<HashSet<string>, int>> lines)
    {
        return lines.Keys.FirstOrDefault(key =>
            lines.TakeWhile((_, i) => !lines.ElementAt(i).Value.Item1.Contains(key))
                .Where((_, i) => i == lines.Count - 1).Any());
    }

    private static void PopulateTree(Tree<int> t, IReadOnlyDictionary<string, Tuple<HashSet<string>, int>> lines)
    {
        t.value = lines[t.id].Item2;
        List<Tree<int>> childrenToBeAdded = new();
        foreach (Tree<int> childNode in lines[t.id].Item1.Select(child => new Tree<int> { id = child }))
        {
            PopulateTree(childNode, lines);
            childrenToBeAdded.Add(childNode);
        }

        t.children = childrenToBeAdded.ToArray();
    }

    private static void MakeCumulative(Tree<int> t)
    {
        foreach (Tree<int> child in t.children) MakeCumulative(child);

        foreach (Tree<int> child in t.children) t.value += child.value;
    }

    public override string Part2(string input)
    {
        Dictionary<string, Tuple<HashSet<string>, int>> lines = new();
        foreach (string[] parts in input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                     .Select(line => line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)))
        {
            foreach (string value in parts.Skip(3))
                if (lines.ContainsKey(parts[0]))
                    lines[parts[0]].Item1.Add(value);
                else
                    lines.Add(parts[0],
                        new Tuple<HashSet<string>, int>(new HashSet<string>(new[] { value }),
                            Int32.Parse(parts[1].Substring(1, parts[1].Length - 2))));
            if (!lines.ContainsKey(parts[0]))
                lines.Add(parts[0],
                    new Tuple<HashSet<string>, int>(new HashSet<string>(),
                        Int32.Parse(parts[1].Substring(1, parts[1].Length - 2))));
        }

        Tree<int> tree = new() { id = GetRootNode(lines) };
        PopulateTree(tree, lines);
        MakeCumulative(tree);
        return FindOutlier(tree).ToString();
    }

    private static int FindOutlier(Tree<int> tree, int diff = 0)
    {
        if (tree.children.All(x => x.value == tree.children.First().value))
            return tree.value - tree.children.Sum(x => x.value) - diff;

        Tree<int> firstChild = tree.children[0];
        Tree<int> secondChild = tree.children.First(x => x.value != firstChild.value);
        int firstValCount = 1;
        for (int i = 2; i < tree.children.Length; i++)
            if (tree.children[i].value == firstChild.value)
                firstValCount++;

        return FindOutlier(firstValCount == 1 ? firstChild : secondChild,
            Math.Abs(firstChild.value - secondChild.value));
    }

    private class Tree<T>
    {
        public Tree<T>[] children;
        public string id;
        public T value;
    }
}