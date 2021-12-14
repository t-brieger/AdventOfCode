using System;

namespace AdventOfCode.Solutions;

public class Year2017Day11 : Solution
{
    public override string Part1(string input)
    {
        string[] directions = input.Split(',');

        int x = 0;
        int y = 0;
        int z = 0;

        foreach (string direction in directions)
            switch (direction)
            {
                case "n":
                    y++;
                    z--;
                    break;
                case "ne":
                    x++;
                    z--;
                    break;
                case "se":
                    x++;
                    y--;
                    break;
                case "s":
                    z++;
                    y--;
                    break;
                case "sw":
                    x--;
                    z++;
                    break;
                case "nw":
                    x--;
                    y++;
                    break;
                default:
                    throw new Exception("Malformed input: " + direction);
            }

        return ((Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) / 2).ToString();
    }

    public override string Part2(string input)
    {
        string[] directions = input.Split(',');

        int x = 0;
        int y = 0;
        int z = 0;

        int max = 0;

        foreach (string direction in directions)
        {
            switch (direction)
            {
                case "n":
                    y++;
                    z--;
                    break;
                case "ne":
                    x++;
                    z--;
                    break;
                case "se":
                    x++;
                    y--;
                    break;
                case "s":
                    z++;
                    y--;
                    break;
                case "sw":
                    x--;
                    z++;
                    break;
                case "nw":
                    x--;
                    y++;
                    break;
                default:
                    throw new Exception("Malformed input: " + direction);
            }

            int dist = (Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) / 2;

            max = dist > max ? dist : max;
        }

        return max.ToString();
    }
}