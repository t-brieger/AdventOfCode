using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day09 : Solution
{
    public override string Part1(string input)
    {
        HashSet<(int, int)> visited = new HashSet<(int, int)>();

        visited.Add((0, 0));

        (int, int) HeadPos = (0, 0);
        (int, int) TailPos = (0, 0);

        foreach (string s in input.Split('\n'))
        {
            int arg = int.Parse(s.Split(' ')[1]);

            for (int i = 0; i < arg; i++)
            {
                if (s[0] == 'R')
                    HeadPos = (HeadPos.Item1 + 1, HeadPos.Item2);
                if (s[0] == 'L')
                    HeadPos = (HeadPos.Item1 - 1, HeadPos.Item2);
                if (s[0] == 'U')
                    HeadPos = (HeadPos.Item1, HeadPos.Item2 - 1);
                if (s[0] == 'D')
                    HeadPos = (HeadPos.Item1, HeadPos.Item2 + 1);

                if (Math.Abs(HeadPos.Item1 - TailPos.Item1) > 1)
                {
                    if (HeadPos.Item1 - TailPos.Item1 == 2)
                        TailPos = (TailPos.Item1 + 1, TailPos.Item2);
                    if (HeadPos.Item1 - TailPos.Item1 == -2)
                        TailPos = (TailPos.Item1 - 1, TailPos.Item2);
                    if (HeadPos.Item2 - TailPos.Item2 != 0)
                        TailPos = (TailPos.Item1, TailPos.Item2 + (HeadPos.Item2 - TailPos.Item2));
                }

                if (Math.Abs(HeadPos.Item2 - TailPos.Item2) > 1)
                {
                    if (HeadPos.Item2 - TailPos.Item2 == 2)
                        TailPos = (TailPos.Item1, TailPos.Item2 + 1);
                    if (HeadPos.Item2 - TailPos.Item2 == -2)
                        TailPos = (TailPos.Item1, TailPos.Item2 - 1);
                    if (HeadPos.Item1 - TailPos.Item1 != 0)
                        TailPos = (TailPos.Item1 + (HeadPos.Item1 - TailPos.Item1), TailPos.Item2);
                }

                visited.Add(TailPos);
            }
        }

        return visited.Count.ToString();
    }

    public override string Part2(string input)
    {
        HashSet<(int, int)> visited = new HashSet<(int, int)>();

        visited.Add((0, 0));

        (int, int)[] pos = new (int, int)[10];
        for (int i = 0; i < pos.Length; i++)
            pos[i] = (0, 0);

        foreach (string s in input.Split('\n'))
        {
            int arg = int.Parse(s.Split(' ')[1]);

            for (int i = 0; i < arg; i++)
            {
                if (s[0] == 'R')
                    pos[0] = (pos[0].Item1 + 1, pos[0].Item2);
                if (s[0] == 'L')
                    pos[0] = (pos[0].Item1 - 1, pos[0].Item2);
                if (s[0] == 'U')
                    pos[0] = (pos[0].Item1, pos[0].Item2 - 1);
                if (s[0] == 'D')
                    pos[0] = (pos[0].Item1, pos[0].Item2 + 1);

                for (int j = 1; j < pos.Length; j++)
                {
                    if (Math.Abs(pos[j - 1].Item1 - pos[j].Item1) + Math.Abs(pos[j - 1].Item2 - pos[j].Item2) > 2)
                    {
                        //this is VERY dumb but hey it works
                        // basically, check which of the diagonally adjacent cells are the closest to the previous
                        // rope segment, then go to that one
                        pos[j] = new[]
                        {
                            (pos[j].Item1 + 1, pos[j].Item2 + 1), (pos[j].Item1 - 1, pos[j].Item2 + 1),
                            (pos[j].Item1 + 1, pos[j].Item2 - 1), (pos[j].Item1 - 1, pos[j].Item2 - 1)
                        }.MinBy(p => Math.Abs(p.Item1 - pos[j - 1].Item1) + Math.Abs(p.Item2 - pos[j - 1].Item2));
                    }

                    if (pos[j - 1].Item1 - pos[j].Item1 == 2)
                    {
                        pos[j] = (pos[j].Item1 + 1, pos[j].Item2);
                    }

                    if (pos[j - 1].Item1 - pos[j].Item1 == -2)
                    {
                        pos[j] = (pos[j].Item1 - 1, pos[j].Item2);
                    }

                    if (pos[j - 1].Item2 - pos[j].Item2 == 2)
                    {
                        pos[j] = (pos[j].Item1, pos[j].Item2 + 1);
                    }

                    if (pos[j - 1].Item2 - pos[j].Item2 == -2)
                    {
                        pos[j] = (pos[j].Item1, pos[j].Item2 - 1);
                    }
                }

                visited.Add(pos[^1]);
            }
        }

        return visited.Count.ToString();
    }
}
