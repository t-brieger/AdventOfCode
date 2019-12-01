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
            inputs = input.Split('\n').Select(line => line.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries).Select(short.Parse).ToArray()).ToArray();

            var minX = inputs.Min(coord => coord[0]) - 1;
            var maxX = inputs.Max(coord => coord[0]) + 1;
            var minY = inputs.Min(coord => coord[1]) - 1;
            var maxY = inputs.Max(coord => coord[1]) + 1;

            var area = new int[inputs.Length];

            foreach (var x in Enumerable.Range(minX, maxX - minX + 1))
            {
                foreach (var y in Enumerable.Range(minY, maxY - minX + 1))
                {
                    var d = inputs.Select(coord => ManhattanDistance(x, y, coord[0], coord[1])).Min();
                    var closest = Enumerable.Range(0, inputs.Length).Where(i => ManhattanDistance(x, y, inputs[i][0], inputs[i][1]) == d).ToArray();

                    if (closest.Length != 1)
                    {
                        continue;
                    }

                    if (x == minX || x == maxX || y == minY || y == maxY)
                    {
                        foreach (var icoord in closest)
                        {
                            area[icoord] = -1;
                        }
                    }
                    else
                    {
                        foreach (var icoord in closest)
                        {
                            if (area[icoord] != -1)
                            {
                                area[icoord]++;
                            }
                        }
                    }
                }
            }
            return area.Max().ToString();
        }

        public override string Part2(string input)
        {
            short[][] inputs;
            inputs = input.Split('\n').Select(line => line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(short.Parse).ToArray()).ToArray();

            var minX = inputs.Min(coord => coord[0]) - 1;
            var maxX = inputs.Max(coord => coord[0]) + 1;
            var minY = inputs.Min(coord => coord[1]) - 1;
            var maxY = inputs.Max(coord => coord[1]) + 1;

            var area = 0;

            foreach (var x in Enumerable.Range(minX, maxX - minX + 1))
            {
                foreach (var y in Enumerable.Range(minY, maxY - minX + 1))
                {
                    var d = inputs.Select(coord => ManhattanDistance(x, y, coord[0], coord[1])).Sum();
                    if (d < 10000)
                        area++;
                }
            }
            return area.ToString();
        }
    }
}