using System;

namespace AdventOfCode.Solutions;

public class Year2023Day13 : Solution
{
    public override string Part1(string input)
    {
        string[] patterns = input.Split("\n\n");

        long answer = 0;

        foreach (string pattern in patterns)
        {
            string[] lines = pattern.Split('\n');
            bool[,] grid = new bool[lines[0].Length, lines.Length];
            for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[y].Length; x++)
                grid[x, y] = lines[y][x] == '#';

            // check for vertical lines of symmetry
            for (int x = 1; x < grid.GetLength(0); x++)
            {
                bool isLine = true;
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    for (int x2 = 0; x2 < x; x2++)
                    {
                        if (x - x2 - 1 < 0 || x + x2 >= grid.GetLength(0))
                            continue;
                        if (grid[x - x2 - 1, y] != grid[x + x2, y])
                        {
                            isLine = false;
                            break;
                        }
                    }

                    if (!isLine)
                        break;
                }

                if (isLine)
                    answer += x;
            }

            // check for horizontal lines of symmetry
            for (int y = 1; y < grid.GetLength(1); y++)
            {
                bool isLine = true;
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    for (int y2 = 0; y2 < y; y2++)
                    {
                        if (y - y2 - 1 < 0 || y + y2 >= grid.GetLength(1))
                            continue;
                        if (grid[x, y - y2 - 1] != grid[x, y + y2])
                        {
                            isLine = false;
                            break;
                        }
                    }

                    if (!isLine)
                        break;
                }

                if (isLine)
                    answer += 100 * y;
            }
        }

        return answer.ToString();
    }

    public override string Part2(string input)
    {
        string[] patterns = input.Split("\n\n");

        long answer = 0;

        foreach (string pattern in patterns)
        {
            string[] lines = pattern.Split('\n');
            bool[,] originalGrid = new bool[lines[0].Length, lines.Length];
            for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[y].Length; x++)
                originalGrid[x, y] = lines[y][x] == '#';

            // true: vertical, false: horizontal
            bool originalLineDirection = false;
            int originalLineLocation = -1;

            bool smudgeLineFound = false;

            for (int smudge = -1; smudge < originalGrid.GetLength(0) * originalGrid.GetLength(1); smudge++)
            {
                if (smudgeLineFound)
                    break;
                bool validSmudge = false;

                bool[,] grid = new bool[originalGrid.GetLength(0), originalGrid.GetLength(1)];
                for (int xix = 0; xix < originalGrid.GetLength(0); xix++)
                for (int yix = 0; yix < originalGrid.GetLength(1); yix++)
                    grid[xix, yix] = originalGrid[xix, yix];
                if (smudge != -1)
                {
                    int x = smudge % originalGrid.GetLength(0);
                    int y = smudge / originalGrid.GetLength(0);
                    grid[x, y] = !grid[x, y];
                }

                // check for vertical lines of symmetry
                for (int x = 1; x < grid.GetLength(0); x++)
                {
                    bool isLine = true;
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        for (int x2 = 0; x2 < x; x2++)
                        {
                            if (x - x2 - 1 < 0 || x + x2 >= grid.GetLength(0))
                                continue;
                            if (grid[x - x2 - 1, y] != grid[x + x2, y])
                            {
                                isLine = false;
                                break;
                            }
                        }

                        if (!isLine)
                            break;
                    }

                    if (isLine)
                    {
                        if (smudge == -1)
                        {
                            originalLineDirection = true;
                            originalLineLocation = x;
                            break;
                        }
                        
                        if (x != originalLineLocation || !originalLineDirection)
                        {
                            answer += x;
                            smudgeLineFound = true;
                            break;
                        }
                    }
                }

                // check for horizontal lines of symmetry
                for (int y = 1; y < grid.GetLength(1); y++)
                {
                    bool isLine = true;
                    for (int x = 0; x < grid.GetLength(0); x++)
                    {
                        for (int y2 = 0; y2 < y; y2++)
                        {
                            if (y - y2 - 1 < 0 || y + y2 >= grid.GetLength(1))
                                continue;
                            if (grid[x, y - y2 - 1] != grid[x, y + y2])
                            {
                                isLine = false;
                                break;
                            }
                        }

                        if (!isLine)
                            break;
                    }

                    if (isLine)
                    {
                        if (smudge == -1)
                        {
                            originalLineDirection = false;
                            originalLineLocation = y;
                            break;
                        }
                        if (y != originalLineLocation || originalLineDirection)
                        {
                            answer += 100 * y;
                            smudgeLineFound = true;
                            break;
                        }
                    }
                }
            }

            if (!smudgeLineFound)
                throw new Exception();
        }

        return answer.ToString();
    }
}
