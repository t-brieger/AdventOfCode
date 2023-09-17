using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2018Day23 : Solution
{
    public override string Part1(string input)
    {
        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        List<(int x, int y, int z, int r)> bots = new();
        foreach (string line in lines)
        {
            string[] l = line["pos=<".Length..].Split(">, r=");
            int r = int.Parse(l[1]);
            string[] coords = l[0].Split(',');
            bots.Add((int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]), r));
        }

        (int x, int y, int z, int r) largestRadiusBot = bots.MaxBy(b => b.r);
        return bots.Count(bot =>
            Math.Abs(bot.x - largestRadiusBot.x) + Math.Abs(bot.y - largestRadiusBot.y) +
            Math.Abs(bot.z - largestRadiusBot.z) <= largestRadiusBot.r).ToString();
    }

    public override string Part2(string input)
    {
        throw new System.NotImplementedException();
    }
}