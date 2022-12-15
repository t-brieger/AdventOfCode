using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day15 : Solution
{
    public override string Part1(string input)
    {
        List<(int sensX, int sensY, int beacX, int beacY)> sensors = new();

        foreach (string sensorReading in input.Split('\n'))
        {
            string[] parts = sensorReading.Replace(",", "").Replace(":", "").Split(' ');

            sensors.Add((int.Parse(parts[2][2..]), int.Parse(parts[3][2..]), int.Parse(parts[8][2..]),
                int.Parse(parts[9][2..])));
        }

        List<(int, int)> emptyRanges = new List<(int, int)>();

        foreach ((int sensX, int sensY, int beacX, int beacY) in sensors)
        {
            int radius = Math.Abs(sensX - beacX) + Math.Abs(sensY - beacY);
            int range = radius - Math.Abs(sensY - 2_000_000);

            if (range < 0)
                continue;

            emptyRanges.Add((sensX - range, sensX + range));
        }

        bool changed = true;
        while (changed)
        {
            emptyRanges = emptyRanges.Distinct().ToList();
            changed = false;
            foreach ((int start, int end) in emptyRanges)
            {
                if (emptyRanges.Any(r => r != (start, end) && r.Item1 <= start && r.Item2 >= end))
                {
                    emptyRanges.Remove((start, end));
                    changed = true;
                    break;
                }

                IEnumerable<(int, int)> otherPossibilities;
                otherPossibilities = emptyRanges.Where(r =>
                    r != (start, end) && r.Item1 <= start && r.Item2 <= end && r.Item2 >= start);
                if (otherPossibilities.Any())
                {
                    (int, int) other = otherPossibilities.First();
                    emptyRanges = emptyRanges.Select(e => e == other ? (other.Item1, end) : e).ToList();
                    emptyRanges.Remove((start, end));
                    changed = true;
                    break;
                }

                otherPossibilities = emptyRanges.Where(r =>
                    r != (start, end) && r.Item1 >= start && r.Item1 <= end && r.Item2 >= end);
                if (otherPossibilities.Any())
                {
                    (int, int) other = otherPossibilities.First();
                    emptyRanges = emptyRanges.Select(e => e == other ? (start, other.Item2) : e).ToList();
                    emptyRanges.Remove((start, end));
                    changed = true;
                    break;
                }
            }
        }

        return (emptyRanges.Sum(r => (r.Item2 - r.Item1) + 1) -
                sensors.Where(sr => sr.beacY == 2_000_000).Select(b => b.beacX).Distinct().Count()).ToString();
    }

    public override string Part2(string input)
    {
        List<(int sensX, int sensY, int beacX, int beacY)> sensors = new();

        foreach (string sensorReading in input.Split('\n'))
        {
            string[] parts = sensorReading.Replace(",", "").Replace(":", "").Split(' ');

            sensors.Add((int.Parse(parts[2][2..]), int.Parse(parts[3][2..]), int.Parse(parts[8][2..]),
                int.Parse(parts[9][2..])));
        }

        List<(int, int)>[] emptyRanges = new List<(int, int)>[4_000_001];

        foreach ((int sensX, int sensY, int beacX, int beacY) in sensors)
        {
            int radius = Math.Abs(sensX - beacX) + Math.Abs(sensY - beacY);
            for (int i = Math.Max(sensY - radius, 0); i <= Math.Min(sensY + radius, 4_000_000); i++)
            {
                int range = radius - Math.Abs(sensY - i);

                if (range < 0)
                    continue;

                if (emptyRanges[i] == null)
                    emptyRanges[i] = new List<(int, int)>();

                emptyRanges[i].Add((sensX - range, sensX + range));
            }
        }

        for (int i = 0; i < emptyRanges.Length; i++)
        {
            bool changed = true;
            while (changed)
            {
                emptyRanges[i] = emptyRanges[i].Distinct().ToList();
                changed = false;
                foreach ((int start, int end) in emptyRanges[i])
                {
                    if (emptyRanges[i].Any(r => r != (start, end) && r.Item1 <= start && r.Item2 >= end))
                    {
                        emptyRanges[i].Remove((start, end));
                        changed = true;
                        break;
                    }

                    IEnumerable<(int, int)> otherPossibilities;
                    otherPossibilities = emptyRanges[i].Where(r =>
                        r != (start, end) && r.Item1 <= start && r.Item2 <= end && r.Item2 >= start);
                    if (otherPossibilities.Any())
                    {
                        (int, int) other = otherPossibilities.First();
                        emptyRanges[i] = emptyRanges[i].Select(e => e == other ? (other.Item1, end) : e).ToList();
                        emptyRanges[i].Remove((start, end));
                        changed = true;
                        break;
                    }

                    otherPossibilities = emptyRanges[i].Where(r =>
                        r != (start, end) && r.Item1 >= start && r.Item1 <= end && r.Item2 >= end);
                    if (otherPossibilities.Any())
                    {
                        (int, int) other = otherPossibilities.First();
                        emptyRanges[i] = emptyRanges[i].Select(e => e == other ? (start, other.Item2) : e).ToList();
                        emptyRanges[i].Remove((start, end));
                        changed = true;
                        break;
                    }
                }
            }

            if (emptyRanges[i]
                .Any(range => range.Item1 is >= 0 and <= 4_000_000 || range.Item2 is >= 0 and <= 4_000_000))
                return (4_000_000L * (emptyRanges[i][0].Item2 + 1) + i).ToString();
        }

        return null;
    }
}
