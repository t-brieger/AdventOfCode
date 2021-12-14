using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2015Day17 : Solution
{
    public override string Part1(string input)
    {
        int Combinations(IReadOnlyList<int> bottles, int ix, int sum = 0)
        {
            if (sum == 150)
                return 1;
            if (ix == bottles.Count || sum > 150)
                return 0;

            int result = 0;
            result += Combinations(bottles, ix + 1, sum);
            result += Combinations(bottles, ix + 1, sum + bottles[ix]);
            return result;
        }

        return Combinations(input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray(), 0)
            .ToString();
    }

    public override string Part2(string input)
    {
        int minSoFar = int.MaxValue;

        void FindMinCombinationLength(IReadOnlyList<int> bottles, int ix, int sum = 0, int currentLength = 0)
        {
            while (true)
            {
                if (sum == 150) minSoFar = Math.Min(currentLength, minSoFar);
                if (ix == bottles.Count || sum > 150) return;

                FindMinCombinationLength(bottles, ix + 1, sum, currentLength);
                int ix1 = ix;
                ix += 1;
                sum += bottles[ix1];
                currentLength += 1;
            }
        }

        int Combinations(IReadOnlyList<int> bottles, int ix, int maxBottles, int sum = 0, int numBottles = 0)
        {
            if (sum == 150 && numBottles == maxBottles)
                return 1;
            if (ix == bottles.Count || sum > 150)
                return 0;

            int result = 0;
            result += Combinations(bottles, ix + 1, maxBottles, sum, numBottles);
            result += Combinations(bottles, ix + 1, maxBottles, sum + bottles[ix], numBottles + 1);
            return result;
        }

        int[] bottles = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        FindMinCombinationLength(bottles, 0);
        return Combinations(bottles, 0, minSoFar).ToString();
    }
}