using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2015Day17 : Solution
    {
        public override string Part1(string input)
        {
            int combinations(int[] bottles, int ix, int sum = 0)
            {
                if (sum == 150)
                    return 1;
                if (ix == bottles.Length || sum > 150)
                    return 0;

                int result = 0;
                result += combinations(bottles, ix + 1, sum);
                result += combinations(bottles, ix + 1, sum + bottles[ix]);
                return result;
            }
            return combinations(input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray(), 0).ToString();
        }

        public override string Part2(string input)
        {
            int minSoFar = int.MaxValue;
            void findMinCombinationLength(int[] bottles, int ix, int sum = 0, int currentLength = 0)
            {
                if (sum == 150)
                    minSoFar = Math.Min(currentLength, minSoFar);
                if (ix == bottles.Length || sum > 150)
                    return;

                findMinCombinationLength(bottles, ix + 1, sum, currentLength);
                findMinCombinationLength(bottles, ix + 1, sum + bottles[ix], currentLength + 1);
            }

            int combinations(int[] bottles, int ix, int maxBottles, int sum = 0, int numBottles = 0)
            {
                if (sum == 150 && numBottles == maxBottles)
                    return 1;
                if (ix == bottles.Length || sum > 150)
                    return 0;

                int result = 0;
                result += combinations(bottles, ix + 1, maxBottles, sum, numBottles);
                result += combinations(bottles, ix + 1, maxBottles, sum + bottles[ix], numBottles + 1);
                return result;
            }

            int[] bottles = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            findMinCombinationLength(bottles, 0);
            return combinations(bottles, 0, minSoFar).ToString();
        }
    }
}