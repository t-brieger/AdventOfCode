using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;

namespace AdventOfCode.Solutions;

public class Year2023Day04 : Solution
{
    public override string Part1(string input)
    {
        string[] lines = input.Split('\n');
        long pointTotal = 0;
        foreach (string line in lines)
        {
            string l = line.Split(':', 2)[1];
            (string winning, string have) = l.Split('|');
            HashSet<int> winningNums = new(winning.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
            HashSet<int> haveNums = new(have.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));

            winningNums.IntersectWith(haveNums);
            int overlap = winningNums.Count;

            pointTotal += overlap == 0 ? 0 : (1 << (overlap - 1));
        }

        return pointTotal.ToString();
    }

    public override string Part2(string input)
    {
        string[] lines = input.Split('\n');
        Dictionary<int, int> repetitions = new();
        for (int i = 0; i < lines.Length; i++)
            repetitions[i] = 1;
        for (int i = 0; i < lines.Length; i++)
        {
            string l = lines[i].Split(':', 2)[1];
            (string winning, string have) = l.Split('|');
            HashSet<int> winningNums = new(winning.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
            HashSet<int> haveNums = new(have.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));

            winningNums.IntersectWith(haveNums);
            int overlap = winningNums.Count;

            for (int j = i + 1; j <= i + overlap && j < lines.Length; j++)
            {
                repetitions[j] += repetitions[i];
            }
        }

        return repetitions.Select(kvp => kvp.Value).Sum().ToString();
    }
}
