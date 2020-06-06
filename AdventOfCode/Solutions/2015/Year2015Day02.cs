using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    class Year2015Day02 : Solution
    {
        public override string Part1(string input)
        {
            string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            int totalArea = 0;

            foreach (int[] measurements in lines.Select(line => line.Split('x').Select(Int32.Parse).ToArray()))
            {
                totalArea += 2 * (measurements[0] * measurements[1] + measurements[1] * measurements[2] + measurements[0] * measurements[2]);
                totalArea += Math.Min(Math.Min(measurements[0] * measurements[1], measurements[1] * measurements[2]), measurements[0] * measurements[2]);
            }

            return totalArea.ToString();
        }

        public override string Part2(string input)
        {
            string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            int totalLength = 0;

            foreach (int[] measurements in lines.Select(line => line.Split('x').Select(Int32.Parse).ToArray()))
            {
                totalLength += 2 * measurements.Sum() - 2 * measurements.Max();
                totalLength += measurements[0] * measurements[1] * measurements[2];
            }

            return totalLength.ToString();
        }
    }
}
