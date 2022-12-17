using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day17 : Solution
{
    private static List<(long, long)> GetNextRock(long index, long yPos)
    {
        List<(long, long)> ourRock = new List<(long, long)>();
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

    public override string Part1(string input)
    {
        HashSet<(long, long)> rockCells = new HashSet<(long, long)>();

        int jetIndex = 0;
        RescaleBar(2022);
        for (int i = 0; i < 2022; i++)
        {
            long yPos = rockCells.Count == 0 ? 4 : rockCells.Max(r => r.Item2) + 4;
            List<(long, long)> ourRock = GetNextRock(i, yPos);

            while (!ourRock.Any(r => r.Item2 == 0 || rockCells.Contains(r)))
            {
                bool windRight = input[jetIndex++] == '>';
                jetIndex %= input.Length;

                List<(long, long)> rockMoved = ourRock.Select(r => (r.Item1 + (windRight ? 1 : -1), r.Item2)).ToList();
                if (!rockMoved.Any(r => rockCells.Contains(r) || r.Item1 is < 0 or > 6 || r.Item2 <= 0))
                    ourRock = rockMoved;

                ourRock = ourRock.Select(r => (r.Item1, r.Item2 - 1)).ToList();
            }

            // we only stop after intersecting the ground, so move one back up
            foreach ((long x, long y) in ourRock.Select(r => (r.Item1, r.Item2 + 1)))
                rockCells.Add((x, y));

            IncreaseBar();
        }

        return rockCells.Max(r => r.Item2).ToString();
    }

    public override string Part2(string input)
    {
        HashSet<(long, long)> rockCells = new();

        Dictionary<(int, long), (long, long)> seen = new();
        
        long cycleHeight = -1, cycleLength = -1;

        long lastAnswer = -1;
        
        int jetIndex = 0;
        for (long i = 0; i < 1000000000000L; i++)
        {
            long height = rockCells.Count == 0 ? 0 : rockCells.Max(r => r.Item2);
            long yPos = rockCells.Count == 0 ? 4 : height + 4;

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

            List<(long, long)> ourRock = GetNextRock(i % 5, yPos);

            while (!ourRock.Any(r => r.Item2 == 0 || rockCells.Contains(r)))
            {
                bool windRight = input[jetIndex++] == '>';
                jetIndex %= input.Length;

                List<(long, long)> rockMoved = ourRock.Select(r => (r.Item1 + (windRight ? 1 : -1), r.Item2)).ToList();
                if (!rockMoved.Any(r => rockCells.Contains(r) || r.Item1 is < 0 or > 6 || r.Item2 <= 0))
                    ourRock = rockMoved;

                ourRock = ourRock.Select(r => (r.Item1, r.Item2 - 1)).ToList();
            }

            // we only stop after intersecting the ground, so move one back up
            foreach ((long x, long y) in ourRock)
                rockCells.Add((x, y+1));
        }

        return rockCells.Max(r => r.Item2).ToString();
    }
}