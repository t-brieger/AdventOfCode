using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2023Day11 : Solution
{
    private static HashSet<(int x, int y)> DoUniverseExpansion(string input, int factor)
    {
        string[] lines = input.Split('\n');

        HashSet<(int x, int y)> galaxies = new();

        int extraLines = 0;
        int maxX = -1;
        for (int y = 0; y < lines.Length; y++)
        {
            bool galaxy = false;
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#')
                {
                    galaxy = true;

                    galaxies.Add((x, y + extraLines));
                }

                maxX = x;
            }

            if (!galaxy)
                extraLines += factor - 1;
        }

        for (int x = 0; x < maxX; x++)
        {
            if (!galaxies.Any(g => g.x == x))
            {
                galaxies = galaxies.Select(g => (g.x < x ? g.x : (g.x + factor - 1), g.y)).ToHashSet();
                x += factor - 1;
                maxX += factor - 1;
            }
        }

        return galaxies;
    }

    public override string Part1(string input)
    { 
        long distSum = 0;
        (int, int)[] galaxiesOrdered = DoUniverseExpansion(input, 2).ToArray();
        for (int i = 0; i < galaxiesOrdered.Length; i++)
        {
            (int x, int y) g1 = galaxiesOrdered[i];
            for (int j = i + 1; j < galaxiesOrdered.Length; j++)
            {
                (int x, int y) g2 = galaxiesOrdered[j];
                distSum += Math.Abs(g1.x - g2.x) + Math.Abs(g1.y - g2.y);
            }
        }

        return distSum.ToString();
    }

    public override string Part2(string input)
    { 
        long distSum = 0;
        (int, int)[] galaxiesOrdered = DoUniverseExpansion(input, 1_000_000).ToArray();
        for (int i = 0; i < galaxiesOrdered.Length; i++)
        {
            (int x, int y) g1 = galaxiesOrdered[i];
            for (int j = i + 1; j < galaxiesOrdered.Length; j++)
            {
                (int x, int y) g2 = galaxiesOrdered[j];
                distSum += Math.Abs(g1.x - g2.x) + Math.Abs(g1.y - g2.y);
            }
        }

        return distSum.ToString();
    }
}