using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions;

public class Year2023Day10 : Solution
{
    private enum PipeTypes
    {
        VERTICAL,
        HORIZONTAL,
        NORTH_EAST,
        NORTH_WEST,
        SOUTH_EAST,
        SOUTH_WEST
    }

    private (bool north, bool east, bool south, bool west) getConnections(PipeTypes pt)
    {
        return pt switch
        {
            PipeTypes.VERTICAL => (true, false, true, false),
            PipeTypes.HORIZONTAL => (false, true, false, true),
            PipeTypes.NORTH_EAST => (true, true, false, false),
            PipeTypes.NORTH_WEST => (true, false, false, true),
            PipeTypes.SOUTH_EAST => (false, true, true, false),
            PipeTypes.SOUTH_WEST => (false, false, true, true),
            _ => throw new ArgumentException($"{pt} is not a valid PipeType")
        };
    }

    public override string Part1(string input)
    {
        Dictionary<(int, int), PipeTypes> map = new();
        string[] lines = input.Split('\n');
        (int x, int y) start = (-1, -1);
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '.')
                    continue;
                if (lines[y][x] == 'S')
                    start = (x, y);
                else
                    map[(x, y)] = lines[y][x] switch
                    {
                        '|' => PipeTypes.VERTICAL,
                        '-' => PipeTypes.HORIZONTAL,
                        'L' => PipeTypes.NORTH_EAST,
                        'J' => PipeTypes.NORTH_WEST,
                        'F' => PipeTypes.SOUTH_EAST,
                        '7' => PipeTypes.SOUTH_WEST,
                        _ => throw new ArgumentException($"Unknown Map Symbol {lines[y][x]}")
                    };
            }
        }

        {
            (bool n, bool e, bool s, bool w)[] startNeighbours =
            {
                map.ContainsKey((start.x, start.y - 1))
                    ? getConnections(map[(start.x, start.y - 1)])
                    : (false, false, false, false),
                map.ContainsKey((start.x + 1, start.y))
                    ? getConnections(map[(start.x + 1, start.y)])
                    : (false, false, false, false),
                map.ContainsKey((start.x, start.y + 1))
                    ? getConnections(map[(start.x, start.y + 1)])
                    : (false, false, false, false),
                map.ContainsKey((start.x - 1, start.y))
                    ? getConnections(map[(start.x - 1, start.y)])
                    : (false, false, false, false)
            };

            foreach (PipeTypes pt in Enum.GetValues(typeof(PipeTypes)))
            {
                if (getConnections(pt) == (startNeighbours[0].s, startNeighbours[1].w, startNeighbours[2].n,
                        startNeighbours[3].e))
                    map[(start.x, start.y)] = pt;
            }
        }

        int initialFacing = -1;
        (bool north, bool east, bool south, bool west) connections = getConnections(map[start]);
        if (connections.north)
            initialFacing = 0;
        if (connections.east)
            initialFacing = 1;
        if (connections.south)
            initialFacing = 2;
        if (connections.west)
            initialFacing = 3;

        (_, int cycleLength) = Util.Djikstra((start, initialFacing), (state, cost) =>
        {
            ((int x , int y), int facing) = state;
            
            (bool north, bool east, bool south, bool west) = getConnections(map[(x, y)]);
            List<(((int, int), int), int)> ret = new(2);
            if (north && facing != 2)
                ret.Add((((x, y - 1), 0), cost + 1));
            if (east && facing != 3)
                ret.Add((((x + 1, y), 1), cost + 1));
            if (south && facing != 0)
                ret.Add((((x, y + 1), 2), cost + 1));
            if (west && facing != 1)
                ret.Add((((x - 1, y), 3), cost + 1));

            return ret;
        }, state =>
        {
            ((int, int) position, int facing) = state;
            return position == start && facing != initialFacing;
        });

        return (cycleLength / 2).ToString();
    }

    public override string Part2(string input)
    {
        throw new System.NotImplementedException();
    }
}