using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2020Day20 : Solution
{
    public override string Part1(string input)
    {
        Dictionary<int, string[]> tiles = new();
        foreach (string element in input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries))
        {
            string[] lines = element.Split("\n");
            int tileId = int.Parse(lines[0].Substring(5, lines[0].Length - 6));
            tiles.Add(tileId, lines.Skip(1).ToArray());
        }

        Dictionary<string, int> edgeCount = new();

        //populate edgeCount
        foreach ((_, string[] value) in tiles)
        {
            //UP
            string edge = value[0];
            if (!edgeCount.ContainsKey(edge))
                edgeCount.Add(edge, 0);
            edgeCount[edge]++;

            edge = new string(edge.Reverse().ToArray());
            if (!edgeCount.ContainsKey(edge))
                edgeCount.Add(edge, 0);
            edgeCount[edge]++;

            //DOWN
            edge = value[^1];
            if (!edgeCount.ContainsKey(edge))
                edgeCount.Add(edge, 0);
            edgeCount[edge]++;

            edge = new string(edge.Reverse().ToArray());
            if (!edgeCount.ContainsKey(edge))
                edgeCount.Add(edge, 0);
            edgeCount[edge]++;

            //LEFT
            edge = string.Join("", value.Select(x => x[0]));
            if (!edgeCount.ContainsKey(edge))
                edgeCount.Add(edge, 0);
            edgeCount[edge]++;

            edge = new string(edge.Reverse().ToArray());
            if (!edgeCount.ContainsKey(edge))
                edgeCount.Add(edge, 0);
            edgeCount[edge]++;

            //RIGHT
            edge = string.Join("", value.Select(x => x[^1]));
            if (!edgeCount.ContainsKey(edge))
                edgeCount.Add(edge, 0);
            edgeCount[edge]++;

            edge = new string(edge.Reverse().ToArray());
            if (!edgeCount.ContainsKey(edge))
                edgeCount.Add(edge, 0);
            edgeCount[edge]++;
        }

        int numCorners = 0;
        long product = 1L;

        //find corners
        foreach ((int key, string[] value) in tiles)
        {
            int unique = 0;

            //UP
            string edge = value[0];
            if (edgeCount[edge] == 1)
                unique++;

            //DOWN
            edge = value[^1];
            if (edgeCount[edge] == 1)
                unique++;

            //LEFT
            edge = string.Join("", value.Select(x => x[0]));
            if (edgeCount[edge] == 1)
                unique++;

            //RIGHT
            edge = string.Join("", value.Select(x => x[^1]));
            if (edgeCount[edge] == 1)
                unique++;

            if (unique != 2) continue;
            product *= key;
            numCorners++;
        }

        Console.WriteLine(numCorners);

        return product.ToString();
    }

    public override string Part2(string input)
    {
        //TODO
        return null;
    }
}