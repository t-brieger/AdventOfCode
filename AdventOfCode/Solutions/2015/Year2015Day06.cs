using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions._2015
{
    class Year2015Day06 : Solution
    {
        public override string Part1(string input)
        {
            bool[] grid = new bool[1_000_000];
            Array.Fill(grid, false);

            foreach (string s in input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int operation = -1;
                switch (s.Substring(0, 7))
                {
                    case "turn on":
                        operation = 0;
                        break;
                    case "turn of":
                        operation = 1;
                        break;
                    case "toggle ":
                        operation = 2;
                        break;
                    default:
                        throw new Exception("unrecognized instruction: " + s.Substring(0, 7));
                }

                string[] parts = s.Split(' ');
                string[] point1 = parts[parts.Length - 3].Split(',');
                string[] point2 = parts[parts.Length - 1].Split(',');

                (int x, int y) firstPoint = (int.Parse(point1[0]), int.Parse(point1[1]));
                (int x, int y) secondPoint = (int.Parse(point2[0]), int.Parse(point2[1])); ;

                if (operation == 0)
                {
                    for (int i = firstPoint.x; i <= secondPoint.x; i++)
                    {
                        for (int j = firstPoint.y; j <= secondPoint.y; j++)
                        {
                            grid[j * 1000 + i] = true;
                        }
                    }
                }else if (operation == 1)
                {
                    for (int i = firstPoint.x; i <= secondPoint.x; i++)
                    {
                        for (int j = firstPoint.y; j <= secondPoint.y; j++)
                        {
                            grid[j * 1000 + i] = false;
                        }
                    }
                }else
                {
                    for (int i = firstPoint.x; i <= secondPoint.x; i++)
                    {
                        for (int j = firstPoint.y; j <= secondPoint.y; j++)
                        {
                            grid[j * 1000 + i] = !grid[j * 1000 + i];
                        }
                    }
                }
            }

            return grid.Count(x => x).ToString();
        }

        public override string Part2(string input)
        {
            int[] grid = new int[1_000_000];
            Array.Fill(grid, 0);

            foreach (string s in input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int operation = -1;
                switch (s.Substring(0, 7))
                {
                    case "turn on":
                        operation = 1;
                        break;
                    case "turn of":
                        operation = -1;
                        break;
                    case "toggle ":
                        operation = 2;
                        break;
                    default:
                        throw new Exception("unrecognized instruction: " + s.Substring(0, 7));
                }

                string[] parts = s.Split(' ');
                string[] point1 = parts[parts.Length - 3].Split(',');
                string[] point2 = parts[parts.Length - 1].Split(',');

                (int x, int y) firstPoint = (int.Parse(point1[0]), int.Parse(point1[1]));
                (int x, int y) secondPoint = (int.Parse(point2[0]), int.Parse(point2[1])); ;

                for (int i = firstPoint.x; i <= secondPoint.x; i++)
                {
                    for (int j = firstPoint.y; j <= secondPoint.y; j++)
                    {
                        grid[j * 1000 + i] += operation;
                        if (grid[j * 1000 + i] < 0)
                            grid[j * 1000 + i] = 0;
                    }
                }
            }

            return grid.Sum().ToString();
        }
    }
}
