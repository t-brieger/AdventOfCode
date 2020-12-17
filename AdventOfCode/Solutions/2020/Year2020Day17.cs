using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions
{
    public class Year2020Day17 : Solution
    {
        public override string Part1(string input)
        {
            //TODO - this only works when input is a square, otherwise we won't be checking some cubes that should be checked
            HashSet<(int, int, int)> enabled = new HashSet<(int, int, int)>();
            string[] lines = input.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '#')
                        enabled.Add((j, i, 0));
                }
            }

            for (int iter = 0; iter < 6; iter++)
            {
                HashSet<(int, int, int)> newEnabled = new HashSet<(int, int, int)>(enabled);
                for (int x = -1 - iter; x < lines.Length + iter + 1; x++)
                {
                    for (int y = -1 - iter; y < lines.Length + iter + 1; y++)
                    {
                        for (int z = -1 - iter; z <= 1 + iter; z++)
                        {
                            int neighbours = 0;
                            for (int relX = -1; relX <= 1; relX++)
                                for (int relY = -1; relY <= 1; relY++)
                                    for (int relZ = -1; relZ <= 1; relZ++)
                                        if (relX != 0 || relY != 0 || relZ != 0)
                                            neighbours += enabled.Contains((x + relX, y + relY, z + relZ)) ? 1 : 0;
                            if (enabled.Contains((x, y, z)) && (neighbours < 2 || neighbours > 3))
                                newEnabled.Remove((x, y, z));
                            else if (!enabled.Contains((x, y, z)) && neighbours == 3)
                                newEnabled.Add((x, y, z));
                        }
                    }
                }

                enabled = newEnabled;
            }


            return enabled.Count.ToString();
        }

        public override string Part2(string input)
        {
            HashSet<(int, int, int, int)> enabled = new HashSet<(int, int, int, int)>();
            string[] lines = input.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '#')
                        enabled.Add((j, i, 0, 0));
                }
            }

            for (int iter = 0; iter < 6; iter++)
            {
                HashSet<(int, int, int, int)> newEnabled = new HashSet<(int, int, int, int)>(enabled);
                for (int x = -1 - iter; x < lines.Length + iter + 1; x++)
                {
                    for (int y = -1 - iter; y < lines.Length + iter + 1; y++)
                    {
                        for (int z = -1 - iter; z <= 1 + iter; z++)
                        {
                            for (int w = -1 - iter; w <= 1 + iter; w++)
                            {
                                int neighbours = 0;
                                for (int relX = -1; relX <= 1; relX++)
                                    for (int relY = -1; relY <= 1; relY++)
                                        for (int relZ = -1; relZ <= 1; relZ++)
                                            for (int relW = -1; relW <= 1; relW++)
                                                if (relX != 0 || relY != 0 || relZ != 0 || relW != 0)
                                                    neighbours += enabled.Contains((x + relX, y + relY, z + relZ, w + relW)) ? 1 : 0;
                                if (enabled.Contains((x, y, z, w)) && (neighbours < 2 || neighbours > 3))
                                    newEnabled.Remove((x, y, z, w));
                                else if (!enabled.Contains((x, y, z, w)) && neighbours == 3)
                                    newEnabled.Add((x, y, z, w));
                            }
                        }
                    }
                }

                enabled = newEnabled;
            }


            return enabled.Count.ToString();
        }
    }
}
