using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day01 : Solution
{
    public override string Part1(string input)
    {
        return input.Split("\n\n").Select(s => s.Split('\n')).Select(a => a.Select(int.Parse).Sum()).OrderByDescending(i => i)
            .First().ToString();
    }

    public override string Part2(string input)
    {
        return input.Split("\n\n").Select(s => s.Split('\n')).Select(a => a.Select(int.Parse).Sum()).OrderByDescending(i => i)
            .Take(3).Sum().ToString();
    }
}
