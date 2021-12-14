using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2017Day12 : Solution
{
    private static void GetConnections(IReadOnlyDictionary<int, HashSet<int>> connections, ISet<int> seen, int n)
    {
        seen.Add(n);

        foreach (int i in connections[n].Where(i => !seen.Contains(i))) GetConnections(connections, seen, i);
    }

    public override string Part1(string input)
    {
        Dictionary<int, HashSet<int>> connections = input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(" <-> "))
            .ToDictionary(x => Int32.Parse(x[0]), x => new HashSet<int>(x[1].Split(',').Select(Int32.Parse)));

        HashSet<int> seen = new();

        GetConnections(connections, seen, 0);

        return seen.Count.ToString();
    }

    public override string Part2(string input)
    {
        Dictionary<int, HashSet<int>> connections = input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(" <-> "))
            .ToDictionary(x => Int32.Parse(x[0]), x => new HashSet<int>(x[1].Split(',').Select(Int32.Parse)));

        int groups = 0;

        while (connections.Count > 0)
        {
            HashSet<int> seen = new();
            GetConnections(connections, seen, connections.Keys.First());

            foreach (int i in seen)
                connections.Remove(i);

            groups++;
        }

        return groups.ToString();
    }
}