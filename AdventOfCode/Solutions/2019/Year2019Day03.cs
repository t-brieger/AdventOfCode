using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2019Day03 : Solution
    {
        private static int ManhattanDist((int X, int Y) p)
        {
            (int x, int y) = p;
            return Math.Abs(x) + Math.Abs(y);
        }

        public override string Part1(string input)
        {
            string[] wires = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            HashSet<(int, int)> firstWire = new();

            HashSet<(int, int)> collisions = new();

            int x = 0, y = 0;
            foreach (string direction in wires[0].Split(','))
            {
                char dir = direction[0];
                int amount = int.Parse(direction[1..]);

                for (int i = 0; i < amount; i++)
                {
                    switch (dir)
                    {
                        case 'U':
                            y--;
                            break;
                        case 'D':
                            y++;
                            break;
                        case 'R':
                            x++;
                            break;
                        case 'L':
                            x--;
                            break;
                    }

                    firstWire.Add((x, y));
                }
            }

            x = 0;
            y = 0;
            foreach (string direction in wires[1].Split(','))
            {
                char dir = direction[0];
                int amount = int.Parse(direction[1..]);

                for (int i = 0; i < amount; i++)
                {
                    switch (dir)
                    {
                        case 'U':
                            y--;
                            break;
                        case 'D':
                            y++;
                            break;
                        case 'R':
                            x++;
                            break;
                        case 'L':
                            x--;
                            break;
                    }

                    if (firstWire.Contains((x, y)))
                        collisions.Add((x, y));
                }
            }

            return collisions.Min(ManhattanDist).ToString();
        }

        public override string Part2(string input)
        {
            string[] wires = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            Dictionary<(int, int), int> firstWire = new();

            int bestCollision = Int32.MaxValue;

            int x = 0, y = 0;
            int wireLen = 0;
            foreach (string direction in wires[0].Split(','))
            {
                char dir = direction[0];
                int amount = int.Parse(direction[1..]);

                for (int i = 0; i < amount; i++)
                {
                    switch (dir)
                    {
                        case 'U':
                            y--;
                            break;
                        case 'D':
                            y++;
                            break;
                        case 'R':
                            x++;
                            break;
                        case 'L':
                            x--;
                            break;
                    }

                    wireLen++;

                    firstWire.TryAdd((x, y), wireLen);
                }
            }

            x = 0;
            y = 0;
            wireLen = 0;
            foreach (string direction in wires[1].Split(','))
            {
                char dir = direction[0];
                int amount = int.Parse(direction[1..]);

                for (int i = 0; i < amount; i++)
                {
                    switch (dir)
                    {
                        case 'U':
                            y--;
                            break;
                        case 'D':
                            y++;
                            break;
                        case 'R':
                            x++;
                            break;
                        case 'L':
                            x--;
                            break;
                    }

                    wireLen++;

                    if (!firstWire.ContainsKey((x, y))) continue;
                    if (wireLen + firstWire[(x, y)] < bestCollision)
                        bestCollision = wireLen + firstWire[(x, y)];
                }
            }

            return bestCollision.ToString();
        }
    }
}