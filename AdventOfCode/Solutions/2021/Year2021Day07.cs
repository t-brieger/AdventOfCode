using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2021Day07 : Solution
    {
        public override string Part1(string input)
        {
            int[] positions = input.Split(',').Select(int.Parse).ToArray();
            int leftMost = positions.Min();
            int rightMost = positions.Max();

            int minFuel = int.MaxValue;
            for (int i = leftMost; i <= rightMost; i++)
            {
                minFuel = Math.Min(positions.Sum(e => Math.Abs(e - i)), minFuel);
            }

            return minFuel.ToString();
        }

        public override string Part2(string input)
        {
            int[] positions = input.Split(',').Select(int.Parse).ToArray();
            int leftMost = positions.Min();
            int rightMost = positions.Max();

            int minFuel = int.MaxValue;
            for (int i = leftMost; i <= rightMost; i++)
            {
                minFuel = Math.Min(positions.Sum(e =>
                {
                    int stepsToMove = Math.Abs(e - i);
                    return stepsToMove * (stepsToMove + 1) / 2;
                }), minFuel);
            }

            return minFuel.ToString();
        }
    }
}