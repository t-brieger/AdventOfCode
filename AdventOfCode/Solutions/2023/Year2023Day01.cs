using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2023Day01 : Solution
{
    public override string Part1(string input)
    {
        return input.Split('\n')
            .Select(line => (line.First(c => c is >= '0' and <= '9'), line.Last(c => c is >= '0' and <= '9')))
            .Select(t => (t.Item1 - '0') * 10 + (t.Item2 - '0')).Sum().ToString();
    }

    public override string Part2(string input)
    {
        //for some reason, "zero" doesn't count.
        // also, we do strange things to the strings because of special cases like "eightwo", which we want to count as both eight and two.
        string[] names = {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
        for (int i = 1; i <= 9; i++)
        {
            // ie "one" becomes "on 1 ne" to account for beginning/end being part of other words
            input = input.Replace(names[i - 1], $"{names[i - 1][..^1]} {i} {names[i - 1][1..]}");
        }
        
        return input.Split('\n')
            .Select(line => (line.First(c => c is >= '0' and <= '9'), line.Last(c => c is >= '0' and <= '9')))
            .Select(t => (t.Item1 - '0') * 10 + (t.Item2 - '0')).Sum().ToString();
    }
}
