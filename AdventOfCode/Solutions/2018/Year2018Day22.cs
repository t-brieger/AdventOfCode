using System;
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
        throw new System.NotImplementedException();
    }
}