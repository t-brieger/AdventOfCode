using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2023Day05 : Solution
{
    public override string Part1(string input)
    {
        string[] parts = input.Split("\n\n");

        long[] numbers = parts[0].Split(": ")[1].Split(' ').Select(long.Parse).ToArray();

        // this isnt technically guaranteed but we assume the lists are in order of mapping (ie seed -> soil -> fertilizer -> ...)
        for (int i = 1; i < parts.Length; i++)
        {
            string[] ranges = parts[i].Split("\n")[1..];
            long[][] parsedRanges = ranges.Select(r => r.Split(' ').Select(long.Parse).ToArray()).ToArray();

            long[] newNumbers = new long[numbers.Length];
            Array.Fill(newNumbers, -1);

            foreach (long[] range in parsedRanges)
            {
                long sourceStart = range[1];
                long sourceEnd = range[1] + range[2];

                long targetStart = range[0];

                for (int j = 0; j < numbers.Length; j++)
                {
                    if (numbers[j] >= sourceStart && numbers[j] < sourceEnd)
                        newNumbers[j] = targetStart + (numbers[j] - sourceStart);
                }
            }

            for (int j = 0; j < newNumbers.Length; j++)
                if (newNumbers[j] == -1)
                    newNumbers[j] = numbers[j];

            numbers = newNumbers;
        }

        return numbers.Min().ToString();
    }

    public override string Part2(string input)
    {
        string[] parts = input.Split("\n\n");

        long[] intervalBoundaries = parts[0].Split(": ")[1].Split(' ').Select(long.Parse).ToArray();
        List<(long, long)> intervals = new List<(long, long)>(intervalBoundaries.Length / 2);
        for (int i = 0; i < intervalBoundaries.Length; i += 2)
            intervals.Add((intervalBoundaries[i], intervalBoundaries[i] + intervalBoundaries[i + 1]));

        for (int i = 1; i < parts.Length; i++)
        {
            string[] ranges = parts[i].Split("\n")[1..];
            long[][] parsedRanges = ranges.Select(r => r.Split(' ').Select(long.Parse).ToArray()).ToArray();

            List<(long, long)> newIntervals = new();
            
            foreach (long[] range in parsedRanges)
            {
                long sourceStart = range[1];
                long sourceEnd = range[1] + range[2];

                long targetStart = range[0];

                long offset = targetStart - sourceStart;

                for (int j = 0; j < intervals.Count; j++)
                {
                    (long iStart, long iEnd) = intervals[j];
                    if (iStart >= sourceEnd || iEnd <= sourceStart)
                        continue;
                    intervals.RemoveAt(j);
                    j--;
                    
                    newIntervals.Add((offset + Math.Max(iStart, sourceStart), offset + Math.Min(iEnd, sourceEnd)));
                    if (iStart < sourceStart)
                        intervals.Add((iStart, sourceStart));

                    if (iEnd > sourceEnd)
                        intervals.Add((sourceEnd, iEnd));
                }
            }
            newIntervals.AddRange(intervals);

            intervals = newIntervals;
        }

        return intervals.MinBy(i => i.Item1).Item1.ToString();
    }
}
