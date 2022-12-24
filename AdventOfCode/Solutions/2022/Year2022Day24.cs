using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day24 : Solution
{
    private static (int x, int y, int facing)[] GetNextTornadoes((int x, int y, int facing)[] last,
        HashSet<(int, int)> walls)
    {
        List<(int, int, int)> next = new();

        foreach ((int x, int y, int dir) in last)
        {
            (int x, int y) vel = dir switch
            {
                0 => (1, 0),
                1 => (0, 1),
                2 => (-1, 0),
                _ => (0, -1)
            };

            (int x, int y) step = (x + vel.x, y + vel.y);
            if (walls.Contains(step))
            {
                while (!walls.Contains((step.x - vel.x, step.y - vel.y)))
                    step = (step.x - vel.x, step.y - vel.y);
            }

            next.Add((step.x, step.y, dir));
        }

        return next.ToArray();
    }

    private static int PathLength(HashSet<(int, int)> walls, Dictionary<int, (int, int, int)[]> tornadoes,
        int startTime, (int, int) start, (int, int) goal, int minX, int minY, int maxX, int maxY)
    {
        HashSet<(int, int, int)> seen = new();
        int tornadoPeriod = (maxX - minX - 1) * (maxY - minY - 1);
        ((int, int) _, int stepCount) = Util.Djikstra(start, (state, step) =>
        {
            if (walls.Contains((state.Item1, state.Item2)) || state.Item1 < minX || state.Item1 > maxX ||
                state.Item2 < minY ||
                state.Item2 > maxY || seen.Contains((state.Item1, state.Item2, (step + startTime) % tornadoPeriod)))
                return Array.Empty<((int, int), int)>();
            seen.Add((state.Item1, state.Item2, (step + startTime) % tornadoPeriod));

            if (!tornadoes.ContainsKey((step + startTime) % tornadoPeriod))
                tornadoes[(step + startTime) % tornadoPeriod] = GetNextTornadoes(tornadoes[((step + startTime) % tornadoPeriod) - 1], walls);

            (int x, int y, int facing)[] hazards = tornadoes[(step + startTime) % tornadoPeriod];
            if (hazards.Any(t => (t.x, t.y) == (state.Item1, state.Item2)))
                return Array.Empty<((int, int), int)>();

            (int, int)[] nextMoves =
            {
                (state.Item1 + 1, state.Item2),
                (state.Item1, state.Item2 + 1),
                (state.Item1 - 1, state.Item2),
                (state.Item1, state.Item2 - 1),
                state
            };
            return nextMoves.Select(m => (m, step + 1));
        }, state => (state.Item1, state.Item2) == goal, false);

        return stepCount;
    }

    public override string Part1(string input)
    {
        string[] lines = input.Split('\n');
        HashSet<(int, int)> walls = new HashSet<(int, int)>();
        (int, int) start = (-1, -1);
        (int, int) goal = (-2, -2);

        List<(int, int, int)> initialTornados = new();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#')
                {
                    walls.Add((x, y));
                    continue;
                }


                if (lines[y][x] == '>')
                {
                    initialTornados.Add((x, y, 0));
                    continue;
                }

                if (lines[y][x] == 'v')
                {
                    initialTornados.Add((x, y, 1));
                    continue;
                }

                if (lines[y][x] == '<')
                {
                    initialTornados.Add((x, y, 2));
                    continue;
                }

                if (lines[y][x] == '^')
                {
                    initialTornados.Add((x, y, 3));
                    continue;
                }

                if (y == 0)
                    start = (x, y);
                if (y == lines.Length - 1)
                    goal = (x, y);
            }
        }

        Dictionary<int, (int x, int y, int facing)[]> tornadoes = new();
        tornadoes[0] = initialTornados.ToArray();

        int minX = 0, minY = 0, maxX = lines[0].Length - 1, maxY = lines.Length - 1;

        return PathLength(walls, tornadoes, 0, start, goal, minX, minY, maxX, maxY).ToString();
    }

    public override string Part2(string input)
    {
        string[] lines = input.Split('\n');
        HashSet<(int, int)> walls = new HashSet<(int, int)>();
        (int, int) start = (-1, -1);
        (int, int) goal = (-2, -2);

        List<(int, int, int)> initialTornados = new();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#')
                {
                    walls.Add((x, y));
                    continue;
                }


                if (lines[y][x] == '>')
                {
                    initialTornados.Add((x, y, 0));
                    continue;
                }

                if (lines[y][x] == 'v')
                {
                    initialTornados.Add((x, y, 1));
                    continue;
                }

                if (lines[y][x] == '<')
                {
                    initialTornados.Add((x, y, 2));
                    continue;
                }

                if (lines[y][x] == '^')
                {
                    initialTornados.Add((x, y, 3));
                    continue;
                }

                if (y == 0)
                    start = (x, y);
                if (y == lines.Length - 1)
                    goal = (x, y);
            }
        }

        Dictionary<int, (int x, int y, int facing)[]> tornadoes = new();
        tornadoes[0] = initialTornados.ToArray();

        int minX = 0, minY = 0, maxX = lines[0].Length - 1, maxY = lines.Length - 1;

        int firstPath = PathLength(walls, tornadoes, 0, start, goal, minX, minY, maxX, maxY);
        int secondPath = PathLength(walls, tornadoes, firstPath, goal, start, minX, minY, maxX, maxY);
        int thirdPath = PathLength(walls, tornadoes, firstPath + secondPath, start, goal, minX, minY, maxX, maxY);
        return (firstPath + secondPath + thirdPath).ToString();
    }
}