using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2018Day18 : Solution
{
    private enum State
    {
        Wooded,
        Lumber
    }

    private Dictionary<(int, int), State> DoMinute(Dictionary<(int, int), State> Map, int maxX, int maxY)
    {
        Dictionary<(int, int), State> ret = new();

        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                int adjacentOpen = 0, adjacentWooded = 0, adjacentLumber = 0;
                for (int xo = -1; xo <= 1; xo++)
                {
                    for (int yo = -1; yo <= 1; yo++)
                    {
                        if (xo == 0 && yo == 0)
                            continue;
                        if (!Map.ContainsKey((x + xo, y + yo)))
                            adjacentOpen++;
                        else if (Map[(x + xo, y + yo)] == State.Wooded)
                            adjacentWooded++;
                        else
                            adjacentLumber++;
                    }
                }

                if (!Map.ContainsKey((x, y)))
                {
                    if (adjacentWooded >= 3)
                        ret.Add((x, y), State.Wooded);
                }
                else if (Map[(x, y)] == State.Wooded)
                {
                    if (adjacentLumber >= 3)
                        ret.Add((x, y), State.Lumber);
                    else
                        ret.Add((x, y), State.Wooded);
                }
                else
                {
                    if (adjacentLumber >= 1 && adjacentWooded >= 1)
                        ret.Add((x, y), State.Lumber);
                }
            }
        }

        return ret;
    }

    public override string Part1(string input)
    {
        Dictionary<(int, int), State> Map = new();

        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == '.')
                    continue;

                Map.Add((j, i), lines[i][j] switch
                {
                    '|' => State.Wooded,
                    '#' => State.Lumber
                });
            }
        }

        int maxX = lines[0].Length, maxY = lines.Length;

        for (int i = 0; i < 10; i++)
            Map = DoMinute(Map, maxX, maxY);

        return (Map.Count(kvp => kvp.Value == State.Lumber) * Map.Count(kvp => kvp.Value == State.Wooded)).ToString();
    }

    public override string Part2(string input)
    {
        throw new System.NotImplementedException();
    }
}