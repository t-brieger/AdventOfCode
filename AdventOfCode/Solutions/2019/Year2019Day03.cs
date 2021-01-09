using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2019Day03 : Solution
    {
        private static int ManhattanDist((int X, int Y) p) => Math.Abs(p.X) + Math.Abs(p.Y);

        public override string Part1(string input)
        {
            string[] wires = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            HashSet<(int, int)> firstWire = new HashSet<(int, int)>();

            HashSet<(int, int)> collisions = new HashSet<(int, int)>();

            int x = 0, y = 0;
            foreach (string direction in wires[0].Split(','))
            {
                char dir = direction[0];
                int amount = int.Parse(direction.Substring(1));

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
                int amount = int.Parse(direction.Substring(1));

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

            Dictionary<(int, int), int> firstWire = new Dictionary<(int, int), int>();
            
            int bestCollision = Int32.MaxValue;

            int x = 0, y = 0;
            int wireLen = 0;
            foreach (string direction in wires[0].Split(','))
            {
                char dir = direction[0];
                int amount = int.Parse(direction.Substring(1));

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
                int amount = int.Parse(direction.Substring(1));

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
                    
                    if (firstWire.ContainsKey((x, y)))
                        if (wireLen + firstWire[(x, y)] < bestCollision)
                            bestCollision = wireLen + firstWire[(x, y)];
                }
            }

            return bestCollision.ToString();
        }
    }
}
