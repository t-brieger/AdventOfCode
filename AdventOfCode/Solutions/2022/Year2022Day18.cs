using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day18 : Solution
{
    public override string Part1(string input)
    {
        HashSet<(int, int, int)> cubes = input.Split('\n').Select(l => l.Split(',').Select(int.Parse).ToArray())
            .Select(a => (a[0], a[1], a[2])).ToHashSet();
        int exposed = 0;
        foreach ((int x, int y, int z) in cubes)
        {
            if (!cubes.Contains((x + 1, y, z)))
                exposed++;
            if (!cubes.Contains((x - 1, y, z)))
                exposed++;
            if (!cubes.Contains((x, y + 1, z)))
                exposed++;
            if (!cubes.Contains((x, y - 1, z)))
                exposed++;
            if (!cubes.Contains((x, y, z + 1)))
                exposed++;
            if (!cubes.Contains((x, y, z - 1)))
                exposed++;
        }

        return exposed.ToString();
    }

    public override string Part2(string input)
    {
        HashSet<(int, int, int)> cubes = input.Split('\n').Select(l => l.Split(',').Select(int.Parse).ToArray())
            .Select(a => (a[0], a[1], a[2])).ToHashSet();
        HashSet<(int, int, int)> exterior = new();

        int xMin = cubes.Min(c => c.Item1);
        int xMax = cubes.Max(c => c.Item1);
        int yMin = cubes.Min(c => c.Item2);
        int yMax = cubes.Max(c => c.Item2);
        int zMin = cubes.Min(c => c.Item3);
        int zMax = cubes.Max(c => c.Item3);

        Queue<(int, int, int)> floodFill = new();
        floodFill.Enqueue((xMin - 1, yMin - 1, zMin - 1));
        while (floodFill.Count > 0)
        {
            (int x, int y, int z) = floodFill.Dequeue();
            if (cubes.Contains((x, y, z)) || exterior.Contains((x, y, z)))
                continue;
            if (x < xMin - 1 || x > xMax + 1 || y < yMin - 1 || y > yMax + 1 || z < zMin - 1 || z > zMax + 1)
                continue;
            exterior.Add((x, y, z));
            floodFill.Enqueue((x + 1, y, z));
            floodFill.Enqueue((x - 1, y, z));
            floodFill.Enqueue((x, y + 1, z));
            floodFill.Enqueue((x, y - 1, z));
            floodFill.Enqueue((x, y, z + 1));
            floodFill.Enqueue((x, y, z - 1));
        }

        int exposed = 0;
        foreach ((int x, int y, int z) in cubes)
        {
            if (exterior.Contains((x + 1, y, z)))
                exposed++;
            if (exterior.Contains((x - 1, y, z)))
                exposed++;
            if (exterior.Contains((x, y + 1, z)))
                exposed++;
            if (exterior.Contains((x, y - 1, z)))
                exposed++;
            if (exterior.Contains((x, y, z + 1)))
                exposed++;
            if (exterior.Contains((x, y, z - 1)))
                exposed++;
        }

        return exposed.ToString();
    }
}