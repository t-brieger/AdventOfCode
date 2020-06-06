using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2018Day06 : Solution
    {
        private static int ManhattanDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        public override string Part1(string input)
        {
            short[][] inputs;
            inputs = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries).Select(Int16.Parse).ToArray()).ToArray();

            int minX = inputs.Min(coord => coord[0]) - 1;
            int maxX = inputs.Max(coord => coord[0]) + 1;
            int minY = inputs.Min(coord => coord[1]) - 1;
            int maxY = inputs.Max(coord => coord[1]) + 1;

            int[] area = new int[inputs.Length];

            foreach (int x in Enumerable.Range(minX, maxX - minX + 1))
            {
                foreach (int y in Enumerable.Range(minY, maxY - minX + 1))
                {
                    int d = inputs.Select(coord => ManhattanDistance(x, y, coord[0], coord[1])).Min();
                    int[] closest = Enumerable.Range(0, inputs.Length).Where(i => ManhattanDistance(x, y, inputs[i][0], inputs[i][1]) == d).ToArray();

                    if (closest.Length != 1)
                    {
                        continue;
                    }

                    if (x == minX || x == maxX || y == minY || y == maxY)
                    {
                        foreach (int icoord in closest)
                        {
                            area[icoord] = -1;
                        }
                    }
                    else
                    {
                        foreach (int icoord in closest.Where(icoord => area[icoord] != -1))
                        {
                            area[icoord]++;
                        }
                    }
                }
            }
            return area.Max().ToString();
        }

        public override string Part2(string input)
        {
            short[][] inputs;
            inputs = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(Int16.Parse).ToArray()).ToArray();

            int minX = inputs.Min(coord => coord[0]) - 1;
            int maxX = inputs.Max(coord => coord[0]) + 1;
            int minY = inputs.Min(coord => coord[1]) - 1;
            int maxY = inputs.Max(coord => coord[1]) + 1;

            int area = (from x in Enumerable.Range(minX, maxX - minX + 1) from y in Enumerable.Range(minY, maxY - minX + 1) select inputs.Select(coord => ManhattanDistance(x, y, coord[0], coord[1])).Sum()).Count(d => d < 10000);

            return area.ToString();
        }
    }
}