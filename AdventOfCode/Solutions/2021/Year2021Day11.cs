using System;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2021Day11 : Solution
{
    private static int DoFlashStep(int[][] grid)
    {
        foreach (int[] row in grid)
            for (int j = 0; j < row.Length; j++)
                row[j]++;

        int flashed = 0;

        while (grid.Any(r => r.Any(i => i > 9)))
        {
            for (int r = 0; r < grid.Length; r++)
            {
                for (int c = 0; c < grid[r].Length; c++)
                {
                    if (grid[r][c] > 9)
                    {
                        flashed++;
                        grid[r][c] = int.MinValue;
                        if (r > 0)
                        {
                            grid[r - 1][c]++;
                            if (c > 0)
                                grid[r - 1][c - 1]++;
                            if (c < grid[r].Length - 1)
                                grid[r - 1][c + 1]++;
                        }

                        if (r < grid.Length - 1)
                        {
                            grid[r + 1][c]++;
                            if (c > 0)
                                grid[r + 1][c - 1]++;
                            if (c < grid[r].Length - 1)
                                grid[r + 1][c + 1]++;
                        }

                        if (c > 0)
                            grid[r][c - 1]++;
                        if (c < grid.Length - 1)
                            grid[r][c + 1]++;
                    }
                }
            }
        }

        foreach (int[] row in grid)
            for (int j = 0; j < row.Length; j++)
                if (row[j] < 0)
                    row[j] = 0;

        return flashed;
    }

    public override string Part1(string input)
    {
        int[][] grid = input.Split('\n').Select(line => line.ToCharArray().Select(c => c - '0').ToArray())
            .ToArray();

        int flashes = 0;

        for (int i = 0; i < 100; i++)
        {
            flashes += DoFlashStep(grid);
        }

        return flashes.ToString();
    }

    public override string Part2(string input)
    {
        input =
            "5483143223\n2745854711\n5264556173\n6141336146\n6357385478\n4167524645\n2176841721\n6882881134\n4846848554\n5283751526";
            
        int[][] grid = input.Split('\n').Select(line => line.ToCharArray().Select(c => c - '0').ToArray())
            .ToArray();

        int i = 0;

        while (DoFlashStep(grid) != grid.Length * grid[0].Length)
            i++;

        return (i + 1).ToString();
    }
}