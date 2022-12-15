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

        RescaleBar(2 * sensors.Count);

        List<(int, int)> emptyRanges = new List<(int, int)>();

        foreach ((int sensX, int sensY, int beacX, int beacY) in sensors)
        {
            int radius = Math.Abs(sensX - beacX) + Math.Abs(sensY - beacY);
            int range = radius - Math.Abs(sensY - 2_000_000);

            IncreaseBar();

            if (range < 0)
            {
                RescaleBar(GetBarScale() - 1);
                continue;
            }


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

            IncreaseBar();
        }

        return (emptyRanges.Sum(r => (r.Item2 - r.Item1) + 1) -
                sensors.Where(sr => sr.beacY == 2_000_000).Select(b => b.beacX).Distinct().Count()).ToString();
    }

    public override string Part2(string input)
    {
        List<(int sensX, int sensY, int radius)> sensors = new();

        foreach (string sensorReading in input.Split('\n'))
        {
            string[] parts = sensorReading.Replace(",", "").Replace(":", "").Split(' ');

            (int, int, int, int) tmpTuple = (int.Parse(parts[2][2..]), int.Parse(parts[3][2..]),
                int.Parse(parts[8][2..]),
                int.Parse(parts[9][2..]));

            sensors.Add((tmpTuple.Item1, tmpTuple.Item2, Math.Abs(tmpTuple.Item1 - tmpTuple.Item3) + Math.Abs(tmpTuple.Item2 - tmpTuple.Item4)));
        }

        RescaleBar(4 * sensors.Count * sensors.Count);

        List<int> linesLeft = new(), linesRight = new();


        foreach ((int x, int y, int radius) in sensors)
        {
            linesRight.Add(y - x + radius + 1);
            linesRight.Add(y - x - radius - 1);
            
            linesLeft.Add(x + y + radius + 1);
            linesLeft.Add(x + y - radius - 1);
            
        }

        foreach (int l in linesLeft)
        {
            foreach (int r in linesRight)
            {
                //both have same value: intersection = (0, val)
                //r = l + 2: intersection = (1, l + r / 2)
                (int x, int y) intersection = ((l - r) / 2, (l + r) / 2);

                if (intersection.x is >= 0 and <= 4_000_000 && intersection.y is >= 0 and <= 4_000_000)
                {
                    if (sensors.All(s =>
                            Math.Abs(s.sensX - intersection.x) + Math.Abs(s.sensY - intersection.y) > s.radius))
                    {
                        SetBar(GetBarScale());
                        return (intersection.x * 4_000_000L + intersection.y).ToString();
                    }
                }

                IncreaseBar();
            }
        }
        
        return null;
    }
}
