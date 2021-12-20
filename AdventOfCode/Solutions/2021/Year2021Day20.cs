using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2021Day20 : Solution
{
    public override string Part1(string input)
    {
        string[] lines = input.Split("\n\n");
        string replacement = lines[0].Replace("\n", "");
        string[] image = lines[1].Split('\n');
        HashSet<(int, int)> enabledPixels = new HashSet<(int, int)>();

        for (int i = 0; i < image.Length; i++)
        for (int j = 0; j < image[i].Length; j++)
            if (image[i][j] == '#')
                enabledPixels.Add((j, i));

        for (int i = 1; i <= 2; i++)
        {
            HashSet<(int, int)> newEnabled = new HashSet<(int, int)>();

            for (int x = -i - 5; x < image[0].Length + i + 5; x++)
            {
                for (int y = -i - 5; y < image.Length + i + 5; y++)
                {
                    int num = 0;
                    if (enabledPixels.Contains((x - 1, y - 1)))
                        num |= 256;
                    if (enabledPixels.Contains((x, y - 1)))
                        num |= 128;
                    if (enabledPixels.Contains((x + 1, y - 1)))
                        num |= 64;

                    if (enabledPixels.Contains((x - 1, y)))
                        num |= 32;
                    if (enabledPixels.Contains((x, y)))
                        num |= 16;
                    if (enabledPixels.Contains((x + 1, y)))
                        num |= 8;

                    if (enabledPixels.Contains((x - 1, y + 1)))
                        num |= 4;
                    if (enabledPixels.Contains((x, y + 1)))
                        num |= 2;
                    if (enabledPixels.Contains((x + 1, y + 1)))
                        num |= 1;

                    if (replacement[num] == '#')
                        newEnabled.Add((x, y));
                }
            }

            enabledPixels = newEnabled;
        }

        return enabledPixels.Count(e =>
                (e.Item1 >= -2 && e.Item1 <= image[0].Length + 1) && (e.Item2 >= -2 && e.Item2 <= image.Length + 1))
            .ToString();
    }

    public override string Part2(string input)
    {
        string[] lines = input.Split("\n\n");
        string replacement = lines[0].Replace("\n", "");
        string[] image = lines[1].Split('\n');
        HashSet<(int, int)> enabledPixels = new HashSet<(int, int)>();

        for (int i = 0; i < image.Length; i++)
        for (int j = 0; j < image[i].Length; j++)
            if (image[i][j] == '#')
                enabledPixels.Add((j, i));

        for (int i = 1; i <= 50; i++)
        {
            HashSet<(int, int)> newEnabled = new HashSet<(int, int)>();

            for (int x = -i - 101; x < image[0].Length + i + 101; x++)
            {
                for (int y = -i - 101; y < image.Length + i + 101; y++)
                {
                    int num = 0;
                    if (enabledPixels.Contains((x - 1, y - 1)))
                        num |= 256;
                    if (enabledPixels.Contains((x, y - 1)))
                        num |= 128;
                    if (enabledPixels.Contains((x + 1, y - 1)))
                        num |= 64;

                    if (enabledPixels.Contains((x - 1, y)))
                        num |= 32;
                    if (enabledPixels.Contains((x, y)))
                        num |= 16;
                    if (enabledPixels.Contains((x + 1, y)))
                        num |= 8;

                    if (enabledPixels.Contains((x - 1, y + 1)))
                        num |= 4;
                    if (enabledPixels.Contains((x, y + 1)))
                        num |= 2;
                    if (enabledPixels.Contains((x + 1, y + 1)))
                        num |= 1;

                    if (replacement[num] == '#')
                        newEnabled.Add((x, y));
                }
            }

            enabledPixels = newEnabled;
        }

        return enabledPixels.Count(e =>
                (e.Item1 >= -50 && e.Item1 <= image[0].Length + 49) && (e.Item2 >= -50 && e.Item2 <= image.Length + 49))
            .ToString();
    }
}