using System;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2020Day09 : Solution
{
    public override string Part1(string input)
    {
        long[] numbers = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse).ToArray();

        for (int i = 25; i < numbers.Length; i++)
        {
            bool combinationFound = false;
            for (int j = i - 25; j < i - 1; j++)
            {
                for (int k = j + 1; k < i; k++)
                {
                    if (numbers[j] + numbers[k] != numbers[i]) continue;
                    combinationFound = true;
                    break;
                }

                if (combinationFound)
                    break;
            }

            if (!combinationFound) return numbers[i].ToString();
        }

        return null;
    }

    public override string Part2(string input)
    {
        //lol
        long numberToSearch = long.Parse(this.Part1(input));

        long[] numbers = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse).ToArray();

        for (int start = 0; start < numbers.Length - 1; start++)
        for (int end = start + 1; end < numbers.Length; end++)
        {
            long[] slice = numbers.Skip(start).Take(end - start + 1).ToArray();

            if (slice.Sum() == numberToSearch)
                return (slice.Min() + slice.Max()).ToString();
        }

        return null;
    }
}