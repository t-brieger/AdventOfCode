using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions;

public class Year2022Day10 : Solution
{
    public override string Part1(string input)
    {
        Dictionary<int, int> valueHistory = new Dictionary<int, int>();
        valueHistory.Add(0, 0);

        int cycle = 0;
        int x = 1;
        foreach (string inst in input.Split('\n'))
        {
            if (inst.Contains("noop"))
            {
                cycle++;
                valueHistory.Add(cycle, x);
                continue;
            }

            int val = int.Parse(inst.Split(' ')[1]);
            cycle++;
            valueHistory.Add(cycle, x);
            cycle++;
            valueHistory.Add(cycle, x);
            x += val;
        }

        int result = 0;
        for (int i = 20; i <= 220; i += 40)
            result += i * valueHistory[i];

        return result.ToString();
    }

    public override string Part2(string input)
    {
        int cycle = 0;
        int x = 1;

        StringBuilder s = new StringBuilder();

        void IncCycle()
        {
            if (cycle % 40 == 0) s.Append('\n');
            s.Append(Math.Abs((cycle % 40) - x) <= 1 ? '#' : ' ');
            cycle++;
        }

        foreach (string inst in input.Split('\n'))
        {
            if (inst.Contains("noop"))
            {
                IncCycle();
                continue;
            }

            int val = int.Parse(inst.Split(' ')[1]);
            IncCycle();
            IncCycle();
            x += val;
        }

        return s.ToString();
    }
}
