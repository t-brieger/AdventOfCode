using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode.Solutions._2019
{
    public class Year2019Day03 : Solution
    {
        private static int ManhattanDist(Point p) => Math.Abs(p.X) + Math.Abs(p.Y);

        public override string Part1(string input)
        {
            string[] wires = input.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

            //null -> intersection; 0/1 -> wires[x]
            Dictionary<Point, bool?> grid = new Dictionary<Point, bool?>();

            HashSet<Point> collisions = new HashSet<Point>();

            for (byte wireIndex = 0; wireIndex < wires.Length; wireIndex++)
            {
                int x = 0;
                int y = 0;
                foreach (string modifier in wires[wireIndex].Split(','))
                {
                    int steps = int.Parse(modifier.Substring(1));
                    switch (modifier[0])
                    {
                        case 'U':
                            for (int i = 0; i < steps; i++)
                            {
                                y--;
                                if (grid.ContainsKey(new Point(x, y)))
                                {
                                    if (grid[new Point(x, y)] != (wireIndex != 0))
                                    {
                                        grid[new Point(x, y)] = null;
                                        collisions.Add(new Point(x, y));
                                    }
                                }
                                else
                                    grid.Add(new Point(x, y), wireIndex != 0);
                            }

                            break;
                        case 'D':
                            for (int i = 0; i < steps; i++)
                            {
                                y++;
                                if (grid.ContainsKey(new Point(x, y)))
                                {
                                    if (grid[new Point(x, y)] != (wireIndex != 0))
                                    {
                                        grid[new Point(x, y)] = null;
                                        collisions.Add(new Point(x, y));
                                    }
                                }
                                else
                                    grid.Add(new Point(x, y), wireIndex != 0);
                            }

                            break;
                        case 'L':
                            for (int i = 0; i < steps; i++)
                            {
                                x--;
                                if (grid.ContainsKey(new Point(x, y)))
                                {
                                    if (grid[new Point(x, y)] != (wireIndex != 0))
                                    {
                                        grid[new Point(x, y)] = null;
                                        collisions.Add(new Point(x, y));
                                    }
                                }
                                else
                                    grid.Add(new Point(x, y), wireIndex != 0);
                            }

                            break;
                        case 'R':
                            for (int i = 0; i < steps; i++)
                            {
                                x++;
                                if (grid.ContainsKey(new Point(x, y)))
                                {
                                    if (grid[new Point(x, y)] != (wireIndex != 0))
                                    {
                                        grid[new Point(x, y)] = null;
                                        collisions.Add(new Point(x, y));
                                    }
                                }
                                else
                                    grid.Add(new Point(x, y), wireIndex != 0);
                            }

                            break;
                        default:
                            throw new Exception("unrecognized direction: " + modifier[0]);
                    }
                }
            }

            return collisions.Min(ManhattanDist).ToString();
        }

        public override string Part2(string input)
        {
            // fuck this
            //TODO: make this work
            string[][] wireSegments = input.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(_ => _.Split(',')).ToArray();

            int bestIntersection = int.MaxValue;

            Dictionary<Point, int> firstWirePoints = new Dictionary<Point, int>();
            firstWirePoints.Add(new Point(0, 0), 0);

            int delay = 0;
            int x = 0;
            int y = 0;

            for (int i = 0; i < wireSegments[0].Length; i++)
            {
                var updateDelay = new Action(() =>
                {
                    if (firstWirePoints.ContainsKey(new Point(x, y)))
                        delay = firstWirePoints[new Point(x, y)];
                    else
                    {
                        delay++;
                        firstWirePoints.Add(new Point(x, y), delay);
                    }
                });

                int length = int.Parse(wireSegments[0][i].Substring(1));

                switch (wireSegments[0][i][0])
                {
                    case 'U':
                        for (int j = 0; j < length; j++)
                        {
                            y--;
                            updateDelay();
                        }

                        break;
                    case 'D':
                        for (int j = 0; j < length; j++)
                        {
                            y++;
                            updateDelay();
                        }
                        break;
                    case 'L':
                        for (int j = 0; j < length; j++)
                        {
                            x--;
                            updateDelay();
                        }
                        break;
                    case 'R':
                        for (int j = 0; j < length; j++)
                        {
                            x++;
                            updateDelay();
                        }
                        break;
                }
            }

            firstWirePoints.Remove(new Point(0, 0));

            delay = x = y = 0;
            Dictionary<Point, int> delays = new Dictionary<Point, int>();
            //delays.Add(new Point(0, 0), 0);

            for (int i = 0; i < wireSegments[1].Length; i++)
            {

                var updateDelay = new Action(() =>
                {
                    if (delays.ContainsKey(new Point(x, y)))
                        delay = delays[new Point(x, y)];
                    else
                    {
                        delay++;
                        delays.Add(new Point(x, y), delay);
                        if (firstWirePoints.ContainsKey(new Point(x, y)) &&
                            (delays[new Point(x, y)] + firstWirePoints[new Point(x, y)]) < bestIntersection)
                            bestIntersection = delays[new Point(x, y)] + firstWirePoints[new Point(x, y)];
                    }
                });

                int length = int.Parse(wireSegments[1][i].Substring(1));

                switch (wireSegments[1][i][0])
                {
                    case 'U':
                        for (int j = 0; j < length; j++)
                        {
                            y--;
                            updateDelay();
                        }

                        break;
                    case 'D':
                        for (int j = 0; j < length; j++)
                        {
                            y++;
                            updateDelay();
                        }
                        break;
                    case 'L':
                        for (int j = 0; j < length; j++)
                        {
                            x--;
                            updateDelay();
                        }
                        break;
                    case 'R':
                        for (int j = 0; j < length; j++)
                        {
                            x++;
                            updateDelay();
                        }
                        break;
                }
            }

            return bestIntersection.ToString();

            /* string[] wires = input.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<Point, int>[] delaysGlobal = new Dictionary<Point, int>[2]; //I suck at naming things

            //null -> intersection; 0/1 -> wires[x]
            Dictionary<Point, bool?> grid = new Dictionary<Point, bool?>();

            HashSet<(Point, int)> collisions = new HashSet<(Point, int)>();

            for (byte wireIndex = 0; wireIndex < wires.Length; wireIndex++)
            {
                Dictionary<Point, int> delays;
                delaysGlobal[wireIndex] = delays = new Dictionary<Point, int>();
                delays.Add(new Point(0, 0), 0);
                int delay = 0;
                int x = 0;
                int y = 0;
                foreach (string modifier in wires[wireIndex].Split(','))
                {
                    int steps = int.Parse(modifier.Substring(1));

                    switch (modifier[0])
                    {
                        case 'U':
                            for (int i = 0; i < steps; i++)
                            {
                                y--;

                                delay++;
                                if (delays.ContainsKey(new Point(x, y)))
                                    delay = delays[new Point(x, y)];
                                else
                                    delays[new Point(x, y)] = delay;

                                if (grid.ContainsKey(new Point(x, y)))
                                {
                                    if (grid[new Point(x, y)] != (wireIndex != 0))
                                    {
                                        grid[new Point(x, y)] = null;
                                        collisions.Add((new Point(x, y), delaysGlobal[0][new Point(x, y)] + delays[new Point(x, y)]));
                                    }
                                }
                                else
                                    grid.Add(new Point(x, y), wireIndex != 0);
                            }

                            break;
                        case 'D':
                            for (int i = 0; i < steps; i++)
                            {
                                y++;

                                delay++;
                                if (delays.ContainsKey(new Point(x, y)))
                                    delay = delays[new Point(x, y)];
                                else
                                    delays[new Point(x, y)] = delay;

                                if (grid.ContainsKey(new Point(x, y)))
                                {
                                    if (grid[new Point(x, y)] != (wireIndex != 0))
                                    {
                                        grid[new Point(x, y)] = null;
                                        collisions.Add((new Point(x, y), delaysGlobal[0][new Point(x, y)] + delays[new Point(x, y)]));
                                    }
                                }
                                else
                                    grid.Add(new Point(x, y), wireIndex != 0);
                            }

                            break;
                        case 'L':
                            for (int i = 0; i < steps; i++)
                            {
                                x--;

                                delay++;
                                if (delays.ContainsKey(new Point(x, y)))
                                    delay = delays[new Point(x, y)];
                                else
                                    delays[new Point(x, y)] = delay;

                                if (grid.ContainsKey(new Point(x, y)))
                                {
                                    if (grid[new Point(x, y)] != (wireIndex != 0))
                                    {
                                        grid[new Point(x, y)] = null;
                                        collisions.Add((new Point(x, y), delaysGlobal[0][new Point(x, y)] + delays[new Point(x, y)]));
                                    }
                                }
                                else
                                    grid.Add(new Point(x, y), wireIndex != 0);
                            }

                            break;
                        case 'R':
                            for (int i = 0; i < steps; i++)
                            {
                                x++;

                                delay++;
                                if (delays.ContainsKey(new Point(x, y)))
                                    delay = delays[new Point(x, y)];
                                else
                                    delays[new Point(x, y)] = delay;

                                if (grid.ContainsKey(new Point(x, y)))
                                {
                                    if (grid[new Point(x, y)] != (wireIndex != 0))
                                    {
                                        grid[new Point(x, y)] = null;
                                        collisions.Add((new Point(x, y), delaysGlobal[0][new Point(x, y)] + delays[new Point(x, y)]));
                                    }
                                }
                                else
                                    grid.Add(new Point(x, y), wireIndex != 0);
                            }

                            break;
                        default:
                            throw new Exception("unrecognized direction: " + modifier[0]);
                    }
                }
            }

            return collisions.Min(x => x.Item2).ToString();
            */
        }
    }
}