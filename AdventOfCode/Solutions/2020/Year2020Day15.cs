using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2020Day15 : Solution
{
    public override string Part1(string input)
    {
        int[] startingNums = input.Split(',').Select(int.Parse).ToArray();

        Dictionary<int, int> lastSpoken = new();
        int lastNum = -999;
        int thisNum = 0;
        int iter = 0;

        foreach (int num in startingNums)
        {
            lastSpoken[lastNum] = iter++;
            lastNum = num;
        }

        iter--;
        while (iter++ < 2019)
        {
            if (lastSpoken.ContainsKey(lastNum))
                thisNum = iter - lastSpoken[lastNum];
            else
                thisNum = 0;

            lastSpoken[lastNum] = iter;
            lastNum = thisNum;
        }

        return lastNum.ToString();
    }

    public override string Part2(string input)
    {
        int[] startingNums = input.Split(',').Select(int.Parse).ToArray();

        Dictionary<int, int> lastSpoken = new();
        int lastNum = -999;
        int thisNum = 0;
        int iter = 0;

        foreach (int num in startingNums)
        {
            lastSpoken[lastNum] = iter++;
            lastNum = num;
        }

        iter--;
        while (iter++ < 29999999)
        {
            if (lastSpoken.ContainsKey(lastNum))
                thisNum = iter - lastSpoken[lastNum];
            else
                thisNum = 0;

            lastSpoken[lastNum] = iter;
            lastNum = thisNum;
        }

        return lastNum.ToString();
    }
}