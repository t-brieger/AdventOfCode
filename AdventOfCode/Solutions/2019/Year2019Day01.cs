using System;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2019Day01 : Solution
{
    public override string Part1(string input)
    {
        int sum = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Sum(line => Int32.Parse(line) / 3 - 2);
        return sum.ToString();
    }

    public override string Part2(string input)
    {
        int sum = 0;
        foreach (int tmpSum in input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                     .Select(line => Int32.Parse(line) / 3 - 2))
        {
            sum += tmpSum;
            int currentMass = tmpSum;
            while (true)
                if (currentMass / 3 - 2 > 0)
                {
                    sum += currentMass / 3 - 2;
                    currentMass = currentMass / 3 - 2;
                }
                else
                {
                    break;
                }
        }

        return sum.ToString();
    }
}