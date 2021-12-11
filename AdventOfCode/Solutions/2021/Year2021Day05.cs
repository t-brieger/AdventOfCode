using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2021Day05 : Solution
    {
        public override string Part1(string input)
        {
            input = input.Replace(" -> ", ",");

            int[][] lines = input.Split('\n').Select(l => l.Split(",").Select(int.Parse).ToArray())
                .ToArray();

            HashSet<(int, int)> seen = new();
            HashSet<(int, int)> seenTwice = new();

            foreach (int[] line in lines.Where(line => line[0] == line[2] || line[1] == line[3]))
            {
                for (int x = Math.Min(line[0], line[2]); x <= Math.Max(line[0], line[2]); x++)
                {
                    for (int y = Math.Min(line[1], line[3]); y <= Math.Max(line[1], line[3]); y++)
                    {
                        if (seen.Contains((x, y)))
                            seenTwice.Add((x, y));
                        else
                            seen.Add((x, y));
                    }
                }
            }

            return seenTwice.Count.ToString();
        }

        public override string Part2(string input)
        {
            input = input.Replace(" -> ", ",");

            int[][] lines = input.Split('\n').Select(l => l.Split(",").Select(int.Parse).ToArray())
                .ToArray();

            HashSet<(int, int)> seen = new();
            HashSet<(int, int)> seenTwice = new();

            foreach (int[] line in lines)
            {
                if (line[0] != line[2] && line[1] != line[3])
                {
                    int x = line[0];
                    int y = line[1];

                    for (int steps = 0; steps <= Math.Abs(line[0] - line[2]); steps++)
                    {
                        if (seen.Contains((x, y)))
                            seenTwice.Add((x, y));
                        else
                            seen.Add((x, y));

                        if (line[0] > line[2])
                            x--;
                        else
                            x++;

                        if (line[1] > line[3])
                            y--;
                        else
                            y++;
                    }

                    continue;
                }

                for (int x = Math.Min(line[0], line[2]); x <= Math.Max(line[0], line[2]); x++)
                {
                    for (int y = Math.Min(line[1], line[3]); y <= Math.Max(line[1], line[3]); y++)
                    {
                        if (seen.Contains((x, y)))
                            seenTwice.Add((x, y));
                        else
                            seen.Add((x, y));
                    }
                }
            }

            return seenTwice.Count.ToString();
        }
    }
}