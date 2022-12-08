using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day08 : Solution
{
    public override string Part1(string input)
    {
        string[] lines = input.Split('\n');
        char[][] chars = lines.Select(l => l.ToCharArray()).ToArray();
        HashSet<(int, int)> visible = new HashSet<(int, int)>();
        for (int i = 0; i < chars.Length; i++)
        {
            int max = -1;
            for (int j = 0; j < chars[i].Length; j++)
            {
                if (chars[i][j] > max)
                {
                    visible.Add((i, j));
                    max = chars[i][j];
                }
            }

            max = -1;
            for (int j = chars[i].Length - 1; j >= 0; j--)
            {
                if (chars[i][j] > max)
                {
                    visible.Add((i, j));
                    max = chars[i][j];
                }
            }
        }

        for (int i = 0; i < chars[0].Length; i++)
        {
            int max = -1;
            for (int j = 0; j < chars.Length; j++)
            {
                if (chars[j][i] > max)
                {
                    visible.Add((j, i));
                    max = chars[j][i];
                }
            }
            max = -1;
            for (int j = chars.Length - 1; j >= 0; j--)
            {
                if (chars[j][i] > max)
                {
                    visible.Add((j, i));
                    max = chars[j][i];
                }
            }
        }

        return visible.Count.ToString();
    }

    public override string Part2(string input)
    {
        string[] lines = input.Split('\n');
        char[][] chars = lines.Select(l => l.ToCharArray()).ToArray();

        int score = 0;

        for (int i = 0; i < chars.Length; i++)
        {
            for (int j = 0; j < chars[i].Length; j++)
            {
                int visTop = 0;
                for (int k = i - 1; k >= 0; k--)
                {
                    visTop++;
                    if (chars[k][j] >= chars[i][j])
                        break;
                }

                int visDown = 0;
                for (int k = i + 1; k < chars.Length; k++)
                {
                    visDown++;
                    if (chars[k][j] >= chars[i][j])
                        break;
                }
                
                int visLeft = 0;
                for (int k = j - 1; k >= 0; k--)
                {
                    visLeft++;
                    if (chars[i][k] >= chars[i][j])
                        break;
                }
                
                int visRight = 0;
                for (int k = j + 1; k < chars[i].Length; k++)
                {
                    visRight++;
                    if (chars[i][k] >= chars[i][j])
                        break;
                }

                score = Math.Max(score, visDown * visLeft * visTop * visRight);
            }
        }
        
        return score.ToString();
    }
}
