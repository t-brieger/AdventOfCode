using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
    public class Year2015Day18 : Solution
    {
        public override string Part1(string input)
        {
            HashSet<(int, int)> lit = new();
            string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            for (int j = 0; j < lines[i].Length; j++)
                if (lines[i][j] == '#')
                    lit.Add((i, j));

            for (int i = 0; i < 100; i++)
            {
                HashSet<(int, int)> newLit = new();
                for (int y = 0; y < 100; y++)
                for (int x = 0; x < 100; x++)
                {
                    int neighbours = 0;
                    neighbours += lit.Contains((y - 1, x - 1)) ? 1 : 0;
                    neighbours += lit.Contains((y - 1, x)) ? 1 : 0;
                    neighbours += lit.Contains((y - 1, x + 1)) ? 1 : 0;
                    neighbours += lit.Contains((y, x + 1)) ? 1 : 0;
                    neighbours += lit.Contains((y + 1, x + 1)) ? 1 : 0;
                    neighbours += lit.Contains((y + 1, x)) ? 1 : 0;
                    neighbours += lit.Contains((y + 1, x - 1)) ? 1 : 0;
                    neighbours += lit.Contains((y, x - 1)) ? 1 : 0;

                    if (lit.Contains((y, x)) && neighbours is >= 2 and <= 3) newLit.Add((y, x));
                    else if (!lit.Contains((y, x)) && neighbours == 3) newLit.Add((y, x));
                }

                lit = newLit;
            }

            return lit.Count.ToString();
        }

        public override string Part2(string input)
        {
            const int gridSize = 100;
            const int iterations = 100;
            
            HashSet<(int, int)> lit = new();
            string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            for (int j = 0; j < lines[i].Length; j++)
                if (lines[i][j] == '#')
                    lit.Add((i, j));

            lit.Add((0, 0));
            lit.Add((0, gridSize - 1));
            lit.Add((gridSize - 1, 0));
            lit.Add((gridSize - 1, gridSize - 1));

            for (int i = 0; i < iterations; i++)
            {
                HashSet<(int, int)> newLit = new();

                for (int y = 0; y < gridSize; y++)
                for (int x = 0; x < gridSize; x++)
                {
                    int neighbours = 0;
                    neighbours += lit.Contains((y - 1, x - 1)) ? 1 : 0;
                    neighbours += lit.Contains((y - 1, x)) ? 1 : 0;
                    neighbours += lit.Contains((y - 1, x + 1)) ? 1 : 0;
                    neighbours += lit.Contains((y, x + 1)) ? 1 : 0;
                    neighbours += lit.Contains((y + 1, x + 1)) ? 1 : 0;
                    neighbours += lit.Contains((y + 1, x)) ? 1 : 0;
                    neighbours += lit.Contains((y + 1, x - 1)) ? 1 : 0;
                    neighbours += lit.Contains((y, x - 1)) ? 1 : 0;

                    if (lit.Contains((y, x)) && neighbours is >= 2 and <= 3) newLit.Add((y, x));
                    else if (!lit.Contains((y, x)) && neighbours == 3) newLit.Add((y, x));
                }

                lit = newLit;

                lit.Add((0, 0));
                lit.Add((0, gridSize - 1));
                lit.Add((gridSize - 1, 0));
                lit.Add((gridSize - 1, gridSize - 1));
            }

            return lit.Count.ToString();
        }
    }
}