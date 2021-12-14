using System;
using System.Linq;

namespace AdventOfCode.Solutions;

class Year2015Day06 : Solution
{
    public override string Part1(string input)
    {
        bool[] grid = new bool[1_000_000];
        Array.Fill(grid, false);

        foreach (string s in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            int operation = s[..7] switch
            {
                "turn on" => 0,
                "turn of" => 1,
                "toggle " => 2,
                _ => throw new Exception("unrecognized instruction: " + s[..7])
            };

            string[] parts = s.Split(' ');
            string[] point1 = parts[^3].Split(',');
            string[] point2 = parts[^1].Split(',');

            (int x, int y) firstPoint = (Int32.Parse(point1[0]), Int32.Parse(point1[1]));
            (int x, int y) = (Int32.Parse(point2[0]), Int32.Parse(point2[1]));

            for (int i = firstPoint.x; i <= x; i++)
            for (int j = firstPoint.y; j <= y; j++)
                grid[j * 1000 + i] = operation switch
                {
                    0 => true,
                    1 => false,
                    2 => !grid[j * 1000 + i],
                    _ => throw new Exception()
                };
        }

        return grid.Count(x => x).ToString();
    }

    public override string Part2(string input)
    {
        int[] grid = new int[1_000_000];
        Array.Fill(grid, 0);

        foreach (string s in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            int operation = s[..7] switch
            {
                "turn on" => 1,
                "turn of" => -1,
                "toggle " => 2,
                _ => throw new Exception("unrecognized instruction: " + s[..7])
            };

            string[] parts = s.Split(' ');
            string[] point1 = parts[^3].Split(',');
            string[] point2 = parts[^1].Split(',');

            (int x, int y) firstPoint = (Int32.Parse(point1[0]), Int32.Parse(point1[1]));
            (int x, int y) = (Int32.Parse(point2[0]), Int32.Parse(point2[1]));

            for (int i = firstPoint.x; i <= x; i++)
            for (int j = firstPoint.y; j <= y; j++)
            {
                grid[j * 1000 + i] += operation;
                if (grid[j * 1000 + i] < 0)
                    grid[j * 1000 + i] = 0;
            }
        }

        return grid.Sum().ToString();
    }
}