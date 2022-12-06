using System.Collections.Generic;

namespace AdventOfCode.Solutions;

public class Year2022Day06 : Solution
{
    // I would have been global lb #16 if I hadn't made a dumb off-by-one mistake (returning i instead of i+1)
    public override string Part1(string input)
    {
        for (int i = 3; i < input.Length; i++)
        {
            if (new HashSet<char>(new char[] {input[i], input[i - 1], input[i - 2], input[i - 3]}).Count == 4)
                return (i + 1).ToString();
        }
        return null;
    }

    public override string Part2(string input)
    {
        for (int i = 13; i < input.Length; i++)
        {
            if (new HashSet<char>(new char[] {input[i], input[i - 1], input[i - 2], input[i - 3], input[i - 4], input[i - 5], input[i - 6], input[i - 7], input[i - 8], input[i - 9], input[i - 10], input[i - 11], input[i - 12], input[i - 13]}).Count == 14)
                return (i + 1).ToString();
        }
        return null;
    }
}
