using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2023Day09 : Solution
{
    private int extrapolateValue(IEnumerable<int> values, bool p2)
    {
        // element 0 is values, element 1 is first derivative, etc
        List<List<int>> derivatives = new();
        derivatives.Add(values.ToList());
        while (derivatives[^1].Any(x => x != 0))
        {
            List<int> last = derivatives[^1];
            List<int> next = new(last.Count - 1);
            for (int i = 0; i < last.Count - 1; i++)
                next.Add(last[i + 1] - last[i]);
            derivatives.Add(next);
        }

        for (int i = derivatives.Count - 1; i >= 0; i--)
        {
            List<int> list = derivatives[i];

            if (i == derivatives.Count - 1)
            {
                list.Add(0);
                continue;
            }

            if (!p2)
                list.Add(list[^1] + derivatives[i + 1][^1]);
            else
                // this is probably very bad for performance, but our lists aren't long enough for it to be a huge issue
                derivatives[i] = list.Prepend(list[0] - derivatives[i + 1][0]).ToList();
        }

        return derivatives[0][p2 ? 0 : ^1];
    }

    public override string Part1(string input)
    {
        string[] lines = input.Split('\n');

        long sum = 0;

        foreach (string line in lines)
        {
            sum += extrapolateValue(line.Split(' ').Select(int.Parse), false);
        }

        return sum.ToString();
    }

    public override string Part2(string input)
    {
        string[] lines = input.Split('\n');

        long sum = 0;

        foreach (string line in lines)
        {
            sum += extrapolateValue(line.Split(' ').Select(int.Parse), true);
        }

        return sum.ToString();
    }
}