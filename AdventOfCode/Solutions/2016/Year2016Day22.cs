using System;
using System.Linq;
using ILGPU.IR.Values;

namespace AdventOfCode.Solutions;

public class Year2016Day22 : Solution
{
    public override string Part1(string input)
    {
        (int used, int avail)[] nodes = input
            .Split('\n')
            .Skip(2)
            .Select(n =>
                n.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(2)
                    .Select(s => s.Substring(0, s.Length - 1)).ToArray())
            .Select(n => (int.Parse(n[0]), int.Parse(n[1])))
            .OrderBy(n => n.Item2)
            .ToArray();

        int pairCount = 0;
        
        foreach ((int used, int avail) in nodes)
        {
            if (used == 0)
                continue;
            int i = nodes.FirstIndexOf(n => n.avail >= used);
            if (i == -1)
                continue;
            if (avail > used)
                i++;
            pairCount += nodes.Length - i;
        }

        return pairCount.ToString();
    }

    public override string Part2(string input)
    {
        // TODO
        // O(n!) sounds a little problematic for n=960 (in my input)
        return null;
    }
}
