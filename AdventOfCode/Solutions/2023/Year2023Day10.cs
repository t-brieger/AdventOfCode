using System;
using System.Collections.Generic;
using System.Linq;

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

    private static (bool north, bool east, bool south, bool west) GetConnections(PipeTypes pt)
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

    private static HashSet<(int, int)> GetMainLoop(Dictionary<(int, int), PipeTypes> map, (int, int) start)
    {
        HashSet<(int, int)> mainLoopTiles = new();

        int facing = -1;

        (int x, int y) nextTile = start;
        do
        {
            mainLoopTiles.Add(nextTile);

            (bool n, bool e, bool s, bool w) = GetConnections(map[nextTile]);

            if (n && facing != 2)
            {
                nextTile = (nextTile.x, nextTile.y - 1);
                facing = 0;
            }
            else if (e && facing != 3)
            {
                nextTile = (nextTile.x + 1, nextTile.y);
                facing = 1;
            }
            else if (s && facing != 0)
            {
                nextTile = (nextTile.x, nextTile.y + 1);
                facing = 2;
            }
            else if (w && facing != 1)
            {
                nextTile = (nextTile.x - 1, nextTile.y);
                facing = 3;
            }
        } while (nextTile != start);

        return mainLoopTiles;
    }

    private static Dictionary<(int, int), PipeTypes> ParseMap(string input, out (int x, int y) start)
    {
        Dictionary<(int, int), PipeTypes> map = new();
        string[] lines = input.Split('\n');
        start = (-1, -1);
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


        (bool n, bool e, bool s, bool w)[] startNeighbours =
        {
            map.ContainsKey((start.x, start.y - 1))
                ? GetConnections(map[(start.x, start.y - 1)])
                : (false, false, false, false),
            map.ContainsKey((start.x + 1, start.y))
                ? GetConnections(map[(start.x + 1, start.y)])
                : (false, false, false, false),
            map.ContainsKey((start.x, start.y + 1))
                ? GetConnections(map[(start.x, start.y + 1)])
                : (false, false, false, false),
            map.ContainsKey((start.x - 1, start.y))
                ? GetConnections(map[(start.x - 1, start.y)])
                : (false, false, false, false)
        };

        foreach (PipeTypes pt in Enum.GetValues(typeof(PipeTypes)))
        {
            if (GetConnections(pt) == (startNeighbours[0].s, startNeighbours[1].w, startNeighbours[2].n,
                    startNeighbours[3].e))
                map[(start.x, start.y)] = pt;
        }

        return map;
    }

    public override string Part1(string input)
    {
        Dictionary<(int, int), PipeTypes> map = ParseMap(input, out (int x, int y) start);
        HashSet<(int, int)> loop = GetMainLoop(map, start);
        return (loop.Count / 2).ToString();
    }

    public override string Part2(string input)
    {
        Dictionary<(int, int), PipeTypes> map = ParseMap(input, out (int x, int y) start);
        HashSet<(int, int)> loop = GetMainLoop(map, start);

        HashSet<(int, int)> walls = new();

        Dictionary<PipeTypes, (int, int)[]> tilesTo3X3 = new();
        tilesTo3X3.Add(PipeTypes.HORIZONTAL, new[] {(-1, 0), (0, 0), (1, 0)});
        tilesTo3X3.Add(PipeTypes.VERTICAL, new[] {(0, -1), (0, 0), (0, 1)});
        tilesTo3X3.Add(PipeTypes.NORTH_EAST, new[] {(0, -1), (0, 0), (1, 0)});
        tilesTo3X3.Add(PipeTypes.NORTH_WEST, new[] {(0, -1), (0, 0), (-1, 0)});
        tilesTo3X3.Add(PipeTypes.SOUTH_WEST, new[] {(0, 1), (0, 0), (-1, 0)});
        tilesTo3X3.Add(PipeTypes.SOUTH_EAST, new[] {(0, 1), (0, 0), (1, 0)});
        
        string[] lines = input.Split('\n');
        int maxX = -1, maxY = -1;
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (loop.Contains((x, y)))
                {
                    (int x, int y) tileCenter = (3 * x + 2, 3 * y + 2);
                    foreach ((int xwall, int ywall) in tilesTo3X3[map[(x, y)]]
                                 .Select(t => (t.Item1 + tileCenter.x, t.Item2 + tileCenter.y)))
                        walls.Add((xwall, ywall));
                }

                maxX = x;
            }

            maxY = y;
        }
        
        // floodfill (we left a border of 1/3 tile (ie 1 tile after scaling it up by factor 3) to make this possible)
        // we get the area of the outer region. since we know the following, we can then easily get the inner area.
        //   - if we only count outside cells where x+1 and y+1 are divisible by 3, we'll get the same amount as in the non-scaled puzzle
        //   - the total amount of tiles that are part of the pre-scaling loop is `loop.Count`
        //   - the entire pre-scaling rectangle we are looking at is of size (maxX + 1)*(maxY + 1)
        // (also, the floodfill itself is possible because the loop is non-self-intersecting)
        HashSet<(int, int)> outerArea = Util.FloodFill(new[]{(0, 0)}, pos =>
        {
            List<(int, int)> ret = new();
            foreach ((int ox, int oy) in new[] { (-1, 0), (1, 0), (0, 1), (0, -1) })
            {
                (int x, int y) otherPos = (pos.Item1 + ox, pos.Item2 + oy);
                if (!walls.Contains(otherPos))
                    if (otherPos.x >= 0 && otherPos.x <= 3 * maxX + 3)
                        if (otherPos.y >= 0 && otherPos.y <= 3 * maxY + 3)
                            ret.Add(otherPos);
            }

            return ret;
        });
        int outerAreaSize = outerArea.Count(pos => ((pos.Item1 + 1) % 3 == 0) && ((pos.Item2 + 1) % 3 == 0));
        int wallArea = loop.Count;
        int totalArea = (maxX + 1) * (maxY + 1);
        int innerArea = totalArea - outerAreaSize - wallArea;

        return innerArea.ToString();
    }
}
