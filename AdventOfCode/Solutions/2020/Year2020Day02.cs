using System;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2020Day02 : Solution
{
    public override string Part1(string input)
    {
        (int min, int max, char letter, string pass)[] lines = input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split(new[] { '-', ' ', ':' }, StringSplitOptions.RemoveEmptyEntries))
            .Select(x => (int.Parse(x[0]), int.Parse(x[1]), x[2][0], x[3])).ToArray();
        int valid = 0;
        foreach ((int min, int max, char letter, string pass) in lines)
        {
            int count = pass.Count(x => x == letter);
            if (count <= max && count >= min)
                valid++;
        }

        return valid.ToString();
    }

    public override string Part2(string input)
    {
        (int, int, char, string)[] lines = input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split(new[] { '-', ' ', ':' }, StringSplitOptions.RemoveEmptyEntries))
            .Select(x => (int.Parse(x[0]), int.Parse(x[1]), x[2][0], x[3])).ToArray();
        int valid = 0;
        foreach ((int pos1, int pos2, char letter, string pass) in lines)
            if ((pass[pos1 - 1] == letter) ^ (pass[pos2 - 1] == letter))
                valid++;
        return valid.ToString();
    }
}