using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2016Day24 : Solution
{
    private static int ShortestDistanceBetweenPoints(Dictionary<(int, int), bool> map, (int x, int y) p1,
        (int x, int y) p2)
    {
        //TODO A* instead of raw dskjskjtra
        if (p1 == p2)
            return 0;

        PriorityQueue<(int x, int y, int w), int> points = new PriorityQueue<(int x, int y, int w), int>();
        HashSet<(int x, int y)> seen = new HashSet<(int x, int y)>();

        points.Enqueue((p1.x, p1.y, 0), 0);

        while (points.Count != 0)
        {
            (int x, int y, int w) = points.Dequeue();

            if (seen.Contains((x, y)))
                continue;

            seen.Add((x, y));

            if ((x, y) == p2)
                return w;

            if (map[(x - 1, y)])
                points.Enqueue((x - 1, y, w + 1), w + 1);
            if (map[(x + 1, y)])
                points.Enqueue((x + 1, y, w + 1), w + 1);
            if (map[(x, y + 1)])
                points.Enqueue((x, y + 1, w + 1), w + 1);
            if (map[(x, y - 1)])
                points.Enqueue((x, y - 1, w + 1), w + 1);
        }

        return int.MaxValue;
    }

    private static int Solve(string input, bool returnTo0 = false)
    {
        char[][] chars = input.Split('\n').Select(l => l.ToCharArray()).ToArray();

        // (x, y) -> isOpen
        Dictionary<(int, int), bool> map = new Dictionary<(int, int), bool>();

        // (ID, x, y)
        HashSet<(char, int, int)> waypoints = new HashSet<(char, int, int)>();

        for (int i = 0; i < chars.Length; i++)
        {
            for (int j = 0; j < chars[i].Length; j++)
            {
                // probably swapped i and j here, but the whole thing does not care about orientation anyway
                map.Add((i, j), chars[i][j] != '#');
                if (chars[i][j] is not '#' and not '.')
                    waypoints.Add((chars[i][j], i, j));
            }
        }

        // most of these are presumably redundant, but the map isn't quite big enough to where it's worth it to optimize
        Dictionary<(char, char), int> waypointDistances =
            new Dictionary<(char, char), int>((waypoints.Count + 1) * (waypoints.Count + 1));

        foreach ((char c, int x, int y) in waypoints)
        {
            foreach ((char c2, int x2, int y2) in waypoints)
            {
                if (waypointDistances.ContainsKey((c, c2)))
                    continue;

                int dist = ShortestDistanceBetweenPoints(map, (x, y), (x2, y2));

                waypointDistances.Add((c, c2), dist);
                if (c != c2)
                    waypointDistances.Add((c2, c), dist);
            }
        }

        char[] waypointIds = waypoints.Select(x => x.Item1).Where(c => c != '0').ToArray();

        int shortest = int.MaxValue;

        foreach (char[] path in Util.GetPermutations(waypointIds, waypointIds.Length))
        {
            int currentLength = 0;

            for (int i = 0; i < path.Length; i++)
                currentLength += waypointDistances[(i == 0 ? '0' : path[i - 1], path[i])];

            if (returnTo0)
                currentLength += waypointDistances[(path[^1], '0')];
            
            if (currentLength < shortest)
                shortest = currentLength;
        }

        return shortest;
    }
    
    public override string Part1(string input)
    {
        return Solve(input).ToString();
    }

    public override string Part2(string input)
    {
        return Solve(input, true).ToString();
    }
}
