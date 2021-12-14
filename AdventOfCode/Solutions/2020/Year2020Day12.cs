using System;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2020Day12 : Solution
{
    public override string Part1(string input)
    {
        (char, int)[] instructions = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line => (line[0], int.Parse(line[1..]))).ToArray();

        int x = 0;
        int y = 0;
        //N, E, S, W
        int dir = 1;

        foreach ((char ins, int amount) in instructions)
            switch (ins)
            {
                case 'N':
                    y -= amount;
                    break;
                case 'S':
                    y += amount;
                    break;
                case 'E':
                    x += amount;
                    break;
                case 'W':
                    x -= amount;
                    break;
                case 'L':
                    dir += (360 - amount) / 90;
                    dir %= 4;
                    break;
                case 'R':
                    dir += amount / 90;
                    dir %= 4;
                    break;
                case 'F':
                    switch (dir)
                    {
                        case 0:
                            y -= amount;
                            break;
                        case 1:
                            x += amount;
                            break;
                        case 2:
                            y += amount;
                            break;
                        case 3:
                            x -= amount;
                            break;
                    }

                    break;
            }

        return (Math.Abs(x) + Math.Abs(y)).ToString();
    }

    public override string Part2(string input)
    {
        (char, int)[] instructions = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line => (line[0], int.Parse(line[1..]))).ToArray();

        int x = 10;
        int y = -1;
        int xShip = 0;
        int yShip = 0;

        foreach ((char ins, int amount) in instructions)
            switch (ins)
            {
                case 'N':
                    y -= amount;
                    break;
                case 'S':
                    y += amount;
                    break;
                case 'E':
                    x += amount;
                    break;
                case 'W':
                    x -= amount;
                    break;
                case 'L':
                    for (int i = 0; i < (360 - amount) / 90; i++)
                        (x, y) = (-y, x);
                    break;
                case 'R':
                    for (int i = 0; i < amount / 90; i++)
                        (x, y) = (-y, x);
                    break;
                case 'F':
                    xShip += x * amount;
                    yShip += y * amount;
                    break;
            }

        return (Math.Abs(xShip) + Math.Abs(yShip)).ToString();
    }
}