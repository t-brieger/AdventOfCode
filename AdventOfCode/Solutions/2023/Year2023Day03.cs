using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2023Day03 : Solution
{
    public override string Part1(string input)
    {
        string[] lines = input.Split('\n');
        HashSet<(int, int)> symbols = new();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] != '.' && lines[y][x] is < '0' or > '9')
                {
                    symbols.Add((x, y));
                }
            }
        }

        (int, int)[] offsets =
        {
            (-1, -1), (0, -1), (1, -1),
            (-1, 0), (1, 0),
            (-1, 1), (0, 1), (1, 1)
        };

        int partNumberSum = 0;

        for (int y = 0; y < lines.Length; y++)
        {
            int currentNum = 0;
            bool isAdjacent = false;

            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] is >= '0' and <= '9')
                {
                    foreach ((int ox, int oy) in offsets)
                    {
                        if (symbols.Contains((x + ox, y + oy)))
                            isAdjacent = true;
                    }

                    currentNum *= 10;
                    currentNum += lines[y][x] - '0';
                }
                else
                {
                    if (isAdjacent)
                        partNumberSum += currentNum;
                    currentNum = 0;
                    isAdjacent = false;
                }
            }

            if (isAdjacent)
                partNumberSum += currentNum;
        }

        return partNumberSum.ToString();
    }

    public override string Part2(string input)
    {
        string[] lines = input.Split('\n');

        (int, int)[] offsets =
        {
            (-1, -1), (0, -1), (1, -1),
            (-1, 0), (1, 0),
            (-1, 1), (0, 1), (1, 1)
        };

        Dictionary<(int, int), int> numbersById = new();
        Dictionary<int, int> idToValue = new();

        for (int y = 0; y < lines.Length; y++)
        {
            int currentNum = 0;
            int len = 0;

            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] is >= '0' and <= '9')
                {
                    currentNum *= 10;
                    currentNum += lines[y][x] - '0';
                    len++;
                }
                else
                {
                    for (int i = 0; i < len; i++)
                    {
                        numbersById.Add((x - 1 - i, y), idToValue.Count);
                    }

                    idToValue[idToValue.Count] = currentNum;

                    currentNum = 0;
                    len = 0;
                }
            }

            if (len == 0) continue;


            for (int i = 0; i < len; i++)
            {
                numbersById.Add((lines[y].Length - 1 - i, y), idToValue.Count);
            }

            idToValue[idToValue.Count] = currentNum;
        }

        long gearRatioSum = 0;
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '*')
                {
                    HashSet<int> numbersAdjacent = new();
                    foreach ((int ox, int oy) in offsets)
                    {
                        if (numbersById.ContainsKey((x + ox, y + oy)))
                            numbersAdjacent.Add(numbersById[(x + ox, y + oy)]);
                    }

                    if (numbersAdjacent.Count == 2)
                        gearRatioSum += numbersAdjacent.Select(i => idToValue[i]).Aggregate(1, (a, b) => a * b);
                }
            }
        }
        
        return gearRatioSum.ToString();
    }
}
