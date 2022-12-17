using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day17 : Solution
{
    private static HashSet<(long, long)> GetNextRock(long index, long yPos)
    {
        HashSet<(long, long)> ourRock = new HashSet<(long, long)>();
        switch (index % 5)
        {
            case 0:
                ourRock.Add((2, yPos));
                ourRock.Add((3, yPos));
                ourRock.Add((4, yPos));
                ourRock.Add((5, yPos));
                break;
            case 1:
                ourRock.Add((3, yPos));
                ourRock.Add((2, yPos + 1));
                ourRock.Add((3, yPos + 1));
                ourRock.Add((4, yPos + 1));
                ourRock.Add((3, yPos + 2));
                break;
            case 2:
                ourRock.Add((2, yPos));
                ourRock.Add((3, yPos));
                ourRock.Add((4, yPos));
                ourRock.Add((4, yPos + 1));
                ourRock.Add((4, yPos + 2));
                break;
            case 3:
                ourRock.Add((2, yPos));
                ourRock.Add((2, yPos + 1));
                ourRock.Add((2, yPos + 2));
                ourRock.Add((2, yPos + 3));
                break;
            case 4:
                ourRock.Add((2, yPos));
                ourRock.Add((2, yPos + 1));
                ourRock.Add((3, yPos));
                ourRock.Add((3, yPos + 1));
                break;
        }

        return ourRock;
    }

    private static int  DoSimulationIteration(HashSet<(long, long)> rocks, long iteration, string jets, int index)
    {
        long yPos = rocks.Max(r => r.Item2) + 4;
        HashSet<(long, long)> ourRock = GetNextRock(iteration, yPos);

        int consumed = 0;
        while (ourRock.All(r => !rocks.Contains(r)))
        {
            int xOffset = (jets[(index + consumed++) % jets.Length] == '>' ? 1 : -1);
            if (ourRock.All(r => (r.Item1 + xOffset) is >= 0 and <= 6) &&
                ourRock.All(r => !rocks.Contains((r.Item1 + xOffset, r.Item2))))
                ourRock = ourRock.Select(r => (r.Item1 + xOffset, r.Item2 - 1)).ToHashSet();
            else
                ourRock = ourRock.Select(r => (r.Item1, r.Item2 - 1)).ToHashSet();
        }

        // we only stop after intersecting the ground, so move one back up
        foreach ((long x, long y) in ourRock)
            rocks.Add((x, y + 1));
        return consumed;
    }

    public override string Part1(string input)
    {
        HashSet<(long, long)> rocks = new()
        {
            (0, 0),
            (1, 0),
            (2, 0),
            (3, 0),
            (4, 0),
            (5, 0),
            (6, 0),
        };

        int jetIndex = 0;
        RescaleBar(2022);
        for (int i = 0; i < 2022; i++)
        {
            jetIndex += DoSimulationIteration(rocks, i, input, jetIndex);
            jetIndex %= input.Length;
            IncreaseBar();
        }

        return rocks.Max(r => r.Item2).ToString();
    }

    public override string Part2(string input)
    {
        HashSet<(long, long)> rocks = new()
        {
            (0, 0),
            (1, 0),
            (2, 0),
            (3, 0),
            (4, 0),
            (5, 0),
            (6, 0),
        };
        
        Dictionary<(int, long), (long, long)> seen = new();

        long cycleHeight = -1, cycleLength = -1;

        long lastAnswer = -1;

        int jetIndex = 0;
        for (long i = 0; i < 1000000000000L; i++)
        {
            long height = rocks.Max(r => r.Item2);

            if (seen.ContainsKey((jetIndex, i % 5)))
            {
                cycleHeight = height - seen[(jetIndex, i % 5)].Item1;
                cycleLength = i - seen[(jetIndex, i % 5)].Item2;
            }

            seen[(jetIndex, i % 5)] = (height, i);

            if (cycleHeight > 0 && cycleLength > 0 && (1000000000000L - i) % cycleLength == 0)
            {
                long cycleCount = (1000000000000L - i) / cycleLength;
                long answer = height + cycleCount * cycleHeight;
                if (answer == lastAnswer)
                    return answer.ToString();
                lastAnswer = answer;
            }

            jetIndex += DoSimulationIteration(rocks, i, input, jetIndex);
            jetIndex %= input.Length;
        }

        return rocks.Max(r => r.Item2).ToString();
    }
}