using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2018Day22 : Solution
{
    public override string Part1(string input)
    {
        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        int depth = int.Parse(lines[0]["depth: ".Length..]);
        string[] targetStr = lines[1]["target: ".Length..].Split(',');
        (int x, int y) target = (int.Parse(targetStr[0]), int.Parse(targetStr[1]));

        int[] erosionLevels = new int[(target.x + 1) * (target.y + 1)];

        for (int i = 0; i < erosionLevels.Length; i++)
        {
            int x = i % (target.x + 1);
            int y = i / (target.x + 1);

            int geologicalIndex;
            if (x == 0 && y == 0)
                geologicalIndex = 0;
            else if (x == target.x && y == target.y)
                geologicalIndex = 0;
            else if (y == 0)
                geologicalIndex = x * 16807;
            else if (x == 0)
                geologicalIndex = y * 48271;
            else
                geologicalIndex = erosionLevels[(y - 1) * (target.x + 1) + x] *
                                  erosionLevels[y * (target.x + 1) + x - 1];
            erosionLevels[y * (target.x + 1) + x] = (geologicalIndex + depth) % 20183;
        }

        return erosionLevels.Select(el => el % 3).Sum().ToString();
    }

    public override string Part2(string input)
    {
        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        int depth = int.Parse(lines[0]["depth: ".Length..]);
        string[] targetStr = lines[1]["target: ".Length..].Split(',');
        (int x, int y) target = (int.Parse(targetStr[0]), int.Parse(targetStr[1]));

        // 10 as the buffer size is a bit arbitrary, but it seems like it's (usually?) enough
        int[,] erosionLevels = new int[target.x + 10, target.y + 10];

        for (int x = 0; x < erosionLevels.GetLength(0); x++)
        {
            for (int y = 0; y < erosionLevels.GetLength(1); y++)
            {
                int geologicalIndex;
                if (x == 0 && y == 0)
                    geologicalIndex = 0;
                else if (x == target.x && y == target.y)
                    geologicalIndex = 0;
                else if (y == 0)
                    geologicalIndex = x * 16807;
                else if (x == 0)
                    geologicalIndex = y * 48271;
                else
                    geologicalIndex = erosionLevels[x, y - 1] *
                                      erosionLevels[x - 1, y];
                erosionLevels[x, y] = (geologicalIndex + depth) % 20183;
            }
        }

        // for the state, 0 is neither object, 1 is torch, 2 is climbing gear
        // this means that for a tile of type x, all integers from 0-2 (inclusive) except x itself are legal tools.
        ((int, int, int) _, int minutes) = Util.Djikstra((0, 0, 1), (state, cost) =>
        {
            (int x, int y, int tool) = state;

            HashSet<((int, int, int), int)> reachableStates = new();
            for (int newTool = 0; newTool <= 2; newTool++)
                if (newTool != erosionLevels[x, y] % 3)
                    reachableStates.Add(((x, y, newTool), cost + 7));

            int[][] offsets =
            {
                new[] { 0, -1 },
                new[] { -1, 0 },
                new[] { 1, 0 },
                new[] { 0, 1 }
            };
            foreach (int[] offset in offsets)
            {
                int newX = x + offset[0];
                int newY = y + offset[1];
                if (newX >= 0 && newX < erosionLevels.GetLength(0) && newY >= 0 && newY < erosionLevels.GetLength(1) &&
                    erosionLevels[newX, newY] % 3 != tool)
                {
                    reachableStates.Add(((newX, newY, tool), cost + 1));
                }
            }

            return reachableStates;
        }, state =>
        {
            (int x, int y, int tool) = state;
            return x == target.x && y == target.y && tool == 1;
        });

        return minutes.ToString();
    }
}