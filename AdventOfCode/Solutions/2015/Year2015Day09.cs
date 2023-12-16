using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2015Day09 : Solution
{
    public override string Part1(string input)
    {
        //TSP sure is a novel problem
        Dictionary<(string, string), int> distances = new();
        foreach (string[] arr in input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                     .Select(line => line.Split(' ')))
        {
            distances.Add((arr[0], arr[2]), int.Parse(arr[4]));
            distances.Add((arr[2], arr[0]), int.Parse(arr[4]));
        }

        int minDist = Int32.MaxValue;

        string[] cities = distances.Keys.Select(x => x.Item1).Distinct().ToArray();
        foreach (string[] perm in Util.GetPermutations(cities, cities.Length))
        {
            int dist = 0;
            for (int i = 0; i < perm.Length - 1; i++) dist += distances[(perm[i], perm[i + 1])];

            minDist = Math.Min(minDist, dist);
        }

        return minDist.ToString();
    }

    public override string Part2(string input)
    {
        Dictionary<(string, string), int> distances = new();
        foreach (string[] arr in input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                     .Select(line => line.Split(' ')))
        {
            distances.Add((arr[0], arr[2]), int.Parse(arr[4]));
            distances.Add((arr[2], arr[0]), int.Parse(arr[4]));
        }

        int maxDist = Int32.MinValue;
        
        string[] cities = distances.Keys.Select(x => x.Item1).Distinct().ToArray();
        foreach (string[] perm in Util.GetPermutations(cities, cities.Length))
        {
            int dist = 0;
            for (int i = 0; i < perm.Length - 1; i++) dist += distances[(perm[i], perm[i + 1])];

            maxDist = Math.Max(maxDist, dist);
        }

        return maxDist.ToString();
    }
}
