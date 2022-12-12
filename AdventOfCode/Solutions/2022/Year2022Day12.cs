using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day12 : Solution
{
    public override string Part1(string input)
    {
        string[] lines = input.Split('\n');
        (int, int) position = (0, 0);
        for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[y].Length; x++)
                if (lines[y][x] == 'S')
                    position = (x, y);
        ((int, int) _, int steps) = Util.Djikstra(position, (p, c) =>
        {
            List<(int, int)> newPositions = new List<(int, int)>();
            if (p.Item1 >= 1)
                newPositions.Add((p.Item1 - 1, p.Item2));
            if (p.Item2 >= 1)
                newPositions.Add((p.Item1, p.Item2 - 1));
            if (p.Item1 < lines[p.Item2].Length - 1)
                newPositions.Add((p.Item1 + 1, p.Item2));
            if (p.Item2 < lines.Length - 1)
                newPositions.Add((p.Item1, p.Item2 + 1));
            return newPositions.Where(pos => lines[pos.Item2][pos.Item1] <= lines[p.Item2][p.Item1] + 1 || lines[p.Item2][p.Item1] == 'S' || lines[pos.Item2][pos.Item1] == 'E')
                .Select(e => (e, c + 1));
        }, p => lines[p.Item2][p.Item1] == 'E');
        return steps.ToString();
    }

    public override string Part2(string input)
    {
        string[] lines = input.Split('\n');
        (int, int) position = (0, 0);
        for (int y = 0; y < lines.Length; y++)
        for (int x = 0; x < lines[y].Length; x++)
            if (lines[y][x] == 'E')
                position = (x, y);
        ((int, int) _, int steps) = Util.Djikstra(position, (p, c) =>
        {
            List<(int, int)> newPositions = new List<(int, int)>();
            if (p.Item1 >= 1)
                newPositions.Add((p.Item1 - 1, p.Item2));
            if (p.Item2 >= 1)
                newPositions.Add((p.Item1, p.Item2 - 1));
            if (p.Item1 < lines[p.Item2].Length - 1)
                newPositions.Add((p.Item1 + 1, p.Item2));
            if (p.Item2 < lines.Length - 1)
                newPositions.Add((p.Item1, p.Item2 + 1));
            return newPositions.Where(pos => lines[pos.Item2][pos.Item1] + 1 >= lines[p.Item2][p.Item1] || lines[p.Item2][p.Item1] == 'S' || lines[pos.Item2][pos.Item1] == 'E')
                .Select(e => (e, c + 1));
        }, p => lines[p.Item2][p.Item1] == 'a');
        return steps.ToString();
    }
}
