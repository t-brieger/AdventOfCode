using System;

namespace AdventOfCode.Solutions;

public class Year2016Day02 : Solution
{
    public override string Part1(string input)
    {
        input += "\n";
        (int x, int y) loc = (0, 0);
        string output = "";
        foreach (char c in input)
        {
            switch (c)
            {
                case 'U':
                    loc = (loc.x, loc.y - 1);
                    break;
                case 'R':
                    loc = (loc.x + 1, loc.y);
                    break;
                case 'D':
                    loc = (loc.x, loc.y + 1);
                    break;
                case 'L':
                    loc = (loc.x - 1, loc.y);
                    break;
                case '\n':
                    output += 5 + 3 * loc.y + loc.x;
                    break;
            }

            loc = (Math.Clamp(loc.x, -1, 1), Math.Clamp(loc.y, -1, 1));
        }
        return output;
    }

    public override string Part2(string input)
    {
        input += "\n";
        (int x, int y) loc = (-2, 0);
        string output = "";
        foreach (char c in input)
        {
            (int, int) oldLoc = loc;
            switch (c)
            {
                case 'U':
                    loc = (loc.x, loc.y - 1);
                    break;
                case 'R':
                    loc = (loc.x + 1, loc.y);
                    break;
                case 'D':
                    loc = (loc.x, loc.y + 1);
                    break;
                case 'L':
                    loc = (loc.x - 1, loc.y);
                    break;
                case '\n':
                    // warning that the switch doesn't cover all possible cases - however, it does, because
                    // |x| + |y| <= 2
#pragma warning disable 8509
                    output += loc switch
#pragma warning restore 8509
                    {
                        (0, -2) => '1',
                        (-1, -1) => '2',
                        (0, -1) => '3',
                        (1, -1) => '4',
                        (-2, 0) => '5',
                        (-1, 0) => '6',
                        (0, 0) => '7',
                        (1, 0) => '8',
                        (2, 0) => '9',
                        (-1, 1) => 'A',
                        (0, 1) => 'B',
                        (1, 1) => 'C',
                        (0, 2) => 'D'
                    };
                    break;
            }

            if (Math.Abs(loc.x) + Math.Abs(loc.y) > 2)
                loc = oldLoc;
        }

        return output;
    }
}