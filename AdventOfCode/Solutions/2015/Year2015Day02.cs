using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions._2015
{
    class Year2015Day02 : Solution
    {
        public override string Part1(string input)
        {
            string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            int totalArea = 0;

            foreach (string line in lines)
            {
                int[] measurements = line.Split('x').Select(int.Parse).ToArray();
                totalArea += 2 * (measurements[0] * measurements[1] + measurements[1] * measurements[2] + measurements[0] * measurements[2]);
                totalArea += Math.Min(Math.Min(measurements[0] * measurements[1], measurements[1] * measurements[2]), measurements[0] * measurements[2]);
            }

            return totalArea.ToString();
        }

        public override string Part2(string input)
        {
            string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            int totalLength = 0;

            foreach (string line in lines)
            {
                int[] measurements = line.Split('x').Select(int.Parse).ToArray();

                totalLength += 2 * measurements.Sum() - 2 * measurements.Max();
                totalLength += measurements[0] * measurements[1] * measurements[2];
            }

            return totalLength.ToString();
        }
    }
}
