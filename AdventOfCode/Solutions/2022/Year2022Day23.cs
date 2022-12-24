using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day23 : Solution
{
    private static HashSet<(int x, int y)> DoRound(HashSet<(int, int)> elves, int index) =>
        DoRound(elves, index, out _);

    private static HashSet<(int x, int y)> DoRound(HashSet<(int, int)> elves, int index, out int changeCount)
    {
        HashSet<(int, int)> next = new();
        Dictionary<(int, int), (int, int)> targets = new();
        Dictionary<(int, int), int> counts = new();

        (int, int)[][] offsets =
        {
            new[]
            {
                (-1, -1),
                (0, -1),
                (1, -1)
            },
            new[]
            {
                (-1, 1),
                (0, 1),
                (1, 1)
            },
            new[]
            {
                (-1, -1),
                (-1, 0),
                (-1, 1)
            },
            new[]
            {
                (1, -1),
                (1, 0),
                (1, 1)
            }
        };

        foreach ((int x, int y) in elves)
        {
            if (!elves.Contains((x + 1, y)) &&
                !elves.Contains((x + 1, y - 1)) &&
                !elves.Contains((x, y - 1)) &&
                !elves.Contains((x - 1, y - 1)) &&
                !elves.Contains((x - 1, y)) &&
                !elves.Contains((x - 1, y + 1)) &&
                !elves.Contains((x, y + 1)) &&
                !elves.Contains((x + 1, y + 1)))
            {
                continue;
            }
            
            (int, int) nextLocation = (x, y);
            for (int i = 0; i < 4; i++)
            {
                if (offsets[(index + i) % 4].All(o => !elves.Contains((x + o.Item1, y + o.Item2))))
                {
                    nextLocation = (x + offsets[(index + i) % 4][1].Item1, y + offsets[(index + i) % 4][1].Item2);
                    break;
                }
            }

            targets[(x, y)] = nextLocation;
            counts[nextLocation] = counts.ContainsKey(nextLocation) ? counts[nextLocation] + 1 : 1;
        }

        // loop again, but this time assign the location (if its valid)
        // this is surely not an optimal way of doing it, but it works well enough
        changeCount = 0;
        foreach ((int x, int y) in elves)
        {
            if (!elves.Contains((x + 1, y)) &&
                !elves.Contains((x + 1, y - 1)) &&
                !elves.Contains((x, y - 1)) &&
                !elves.Contains((x - 1, y - 1)) &&
                !elves.Contains((x - 1, y)) &&
                !elves.Contains((x - 1, y + 1)) &&
                !elves.Contains((x, y + 1)) &&
                !elves.Contains((x + 1, y + 1)))
            {
                next.Add((x, y));
                continue;
            }

            changeCount++;
            (int, int) target = targets[(x, y)];
            if (counts[target] == 1)
                next.Add(target);
            else
                next.Add((x, y));
        }

        return next;
    }

    public override string Part1(string input)
    {
        HashSet<(int x, int y)> elves = new();
        string[] lines = input.Split('\n');
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#')
                    elves.Add((x, y));
            }
        }

        for (int i = 0; i < 10; i++)
            elves = DoRound(elves, i);

        return ((elves.Max(e => e.x) - elves.Min(e => e.x) + 1) * (elves.Max(e => e.y) - elves.Min(e => e.y) + 1) -
                elves.Count)
            .ToString();
    }

    public override string Part2(string input)
    {
        HashSet<(int x, int y)> elves = new();
        string[] lines = input.Split('\n');
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#')
                    elves.Add((x, y));
            }
        }

        int index = 0;
        int changeCount = elves.Count;
        
        RescaleBar(elves.Count);
        while (changeCount > 0)
        {
            elves = DoRound(elves, index++, out changeCount);
            SetBar(elves.Count - changeCount);
        }

        return index.ToString();
    }
}
