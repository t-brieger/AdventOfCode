using System;

namespace AdventOfCode.Solutions;

public class Year2021Day25 : Solution
{
    private enum States
    {
        EMPTY,
        RIGHT_CUCUMBER,
        DOWN_CUCUMBER
    }

    private static (States[,] s, bool change) DoStep(States[,] grid)
    {
        bool changes = false;
        States[,] newGrid = new States[grid.GetLength(0), grid.GetLength(1)];
        
        for (int x = 0; x < grid.GetLength(0); x++)
        for (int y = 0; y < grid.GetLength(1); y++)
            newGrid[x, y] = grid[x, y];
        
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] != States.RIGHT_CUCUMBER || grid[(x + 1) % grid.GetLength(0), y] != States.EMPTY)
                    continue;
                newGrid[x, y] = States.EMPTY;
                newGrid[(x + 1) % grid.GetLength(0), y] = States.RIGHT_CUCUMBER;
                changes = true;
            }
        }

        for (int x = 0; x < grid.GetLength(0); x++)
        for (int y = 0; y < grid.GetLength(1); y++)
            grid[x, y] = newGrid[x, y];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] != States.DOWN_CUCUMBER || grid[x, (y + 1) % grid.GetLength(1)] != States.EMPTY)
                    continue;
                newGrid[x, y] = States.EMPTY;
                newGrid[x, (y + 1) % grid.GetLength(1)] = States.DOWN_CUCUMBER;
                changes = true;
            }
        }

        return (newGrid, changes);
    }

    public override string Part1(string input)
    {
        string[] lines = input.Split('\n');
        int width = lines[0].Length;
        int height = lines.Length;

        States[,] grid = new States[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = lines[y][x] switch
                {
                    '.' => States.EMPTY,
                    'v' => States.DOWN_CUCUMBER,
                    '>' => States.RIGHT_CUCUMBER,
                    _ => throw new ArgumentException(lines[y][x].ToString())
                };
            }
        }

        bool changes = true;
        
        int i = 0;
        while (changes)
        {
            i++;
            (grid, changes) = DoStep(grid);
        }

        return i.ToString();
    }

    public override string Part2(string input)
    {
        return "";
    }
}