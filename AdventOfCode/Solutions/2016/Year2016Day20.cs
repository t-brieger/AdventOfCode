using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2016Day20 : Solution
{
    public override string Part1(string input)
    {
        (uint, uint)[] ranges = input.Split('\n').Select(l => l.Split('-').Select(uint.Parse).ToArray())
            .Select(x => (x[0], x[1])).ToArray();

        if (!ranges.Any(x => x.Item1 <= 0))
            return "0";

        uint[] candidates = ranges.Select(r => r.Item2 + 1).OrderBy(x => x).ToArray();

        foreach (uint candidate in candidates)
        {
            bool valid = true;
            foreach ((uint start, uint end) in ranges)
            {
                if (candidate >= start && candidate <= end)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
                return candidate.ToString();
        }

        return null;
    }

    public override string Part2(string input)
    {
        (uint, uint)[] ranges = input.Split('\n').Select(l => l.Split('-').Select(uint.Parse).ToArray())
            .Select(x => (x[0], x[1])).ToArray();

        List<(uint, uint)> nonOverlappingRanges = new List<(uint, uint)>();

        foreach ((uint s, uint e) in ranges)
        {
            uint start = s, end = e;
            
            List<(uint, uint)> toRemove = new List<(uint, uint)>();
            foreach ((uint start, uint end) other in nonOverlappingRanges)
            {
                // if the range overlaps:
                if (start <= other.start ? end >= other.start : other.end >= start)
                {
                    // if existing range is completely covered by our current one, remove it from the list
                    if (other.start >= start && other.end >= start && other.start <= end && other.end <= end)
                    {
                        toRemove.Add(other);
                        continue;
                    }
                    
                    // if it does not, instead just merge the two.
                    start = Math.Min(start, other.start);
                    end = Math.Max(end, other.end);
                    toRemove.Add(other);
                }
            }
            
            nonOverlappingRanges.Add((start, end));

            foreach ((uint, uint) rem in toRemove)
                nonOverlappingRanges.Remove(rem);
        }

        return (uint.MaxValue - nonOverlappingRanges.Sum(x => x.Item2 - x.Item1 + 1) + 1L).ToString();
    }
}
