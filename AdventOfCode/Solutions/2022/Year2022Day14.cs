using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day14 : Solution
{
    private enum TileType
    {
        SAND, WALL
    }
    
    public override string Part1(string input)
    {
        Dictionary<(int, int), TileType> tiles = new Dictionary<(int, int), TileType>();
        int maxY = int.MinValue;
        
        foreach (string path in input.Split('\n'))
        {
            (int x, int y)[] controlPoints = path.Split(" -> ")
                .Select(t => t.Split(',').Select(int.Parse).ToArray()).Select(a => (a[0], a[1])).ToArray();

            for (int i = 1; i < controlPoints.Length; i++)
            {
                if (controlPoints[i].x > controlPoints[i - 1].x)
                {
                    for (int x = controlPoints[i - 1].x; x <= controlPoints[i].x; x++)
                    {
                        tiles[(x, controlPoints[i].y)] = TileType.WALL;
                        maxY = Math.Max(maxY, controlPoints[i].y);
                    }
                }
                if (controlPoints[i].x < controlPoints[i - 1].x)
                {
                    for (int x = controlPoints[i - 1].x; x >= controlPoints[i].x; x--)
                    {
                        tiles[(x, controlPoints[i].y)] = TileType.WALL;
                        maxY = Math.Max(maxY, controlPoints[i].y);
                    }
                }
                if (controlPoints[i].y > controlPoints[i - 1].y)
                {
                    for (int y = controlPoints[i - 1].y; y <= controlPoints[i].y; y++)
                    {
                        tiles[(controlPoints[i].x, y)] = TileType.WALL;
                        maxY = Math.Max(maxY, controlPoints[i].y);
                    }
                }
                if (controlPoints[i].y < controlPoints[i - 1].y)
                {
                    for (int y = controlPoints[i - 1].y; y >= controlPoints[i].y; y--)
                    {
                        tiles[(controlPoints[i].x, y)] = TileType.WALL;
                        maxY = Math.Max(maxY, controlPoints[i].y);
                    }
                }
            }
        }

        int sandCount = 0;
        
        while (true)
        {
            int sandX = 500, sandY = 0;
            while (true)
            {
                if (sandY > maxY)
                    return sandCount.ToString();
                
                if (!tiles.ContainsKey((sandX, sandY + 1)))
                {
                    sandY++;
                }
                else if (!tiles.ContainsKey((sandX - 1, sandY + 1)))
                {
                    sandY++;
                    sandX--;
                }
                else if (!tiles.ContainsKey((sandX + 1, sandY + 1)))
                {
                    sandY++;
                    sandX++;
                }
                else
                {
                    tiles[(sandX, sandY)] = TileType.SAND;
                    break;
                }
            }

            sandCount++;
        }
    }

    public override string Part2(string input)
    {
        Dictionary<(int, int), TileType> tiles = new Dictionary<(int, int), TileType>();
        int maxY = int.MinValue;
        
        foreach (string path in input.Split('\n'))
        {
            (int x, int y)[] controlPoints = path.Split(" -> ")
                .Select(t => t.Split(',').Select(int.Parse).ToArray()).Select(a => (a[0], a[1])).ToArray();

            for (int i = 1; i < controlPoints.Length; i++)
            {
                if (controlPoints[i].x > controlPoints[i - 1].x)
                {
                    for (int x = controlPoints[i - 1].x; x <= controlPoints[i].x; x++)
                    {
                        tiles[(x, controlPoints[i].y)] = TileType.WALL;
                        maxY = Math.Max(maxY, controlPoints[i].y);
                    }
                }
                if (controlPoints[i].x < controlPoints[i - 1].x)
                {
                    for (int x = controlPoints[i - 1].x; x >= controlPoints[i].x; x--)
                    {
                        tiles[(x, controlPoints[i].y)] = TileType.WALL;
                        maxY = Math.Max(maxY, controlPoints[i].y);
                    }
                }
                if (controlPoints[i].y > controlPoints[i - 1].y)
                {
                    for (int y = controlPoints[i - 1].y; y <= controlPoints[i].y; y++)
                    {
                        tiles[(controlPoints[i].x, y)] = TileType.WALL;
                        maxY = Math.Max(maxY, controlPoints[i].y);
                    }
                }
                if (controlPoints[i].y < controlPoints[i - 1].y)
                {
                    for (int y = controlPoints[i - 1].y; y >= controlPoints[i].y; y--)
                    {
                        tiles[(controlPoints[i].x, y)] = TileType.WALL;
                        maxY = Math.Max(maxY, controlPoints[i].y);
                    }
                }
            }
        }

        int sandCount = 0;
        
        while (true)
        {
            if (tiles.ContainsKey((500, 0)) && tiles[(500, 0)] == TileType.SAND)
                return sandCount.ToString();
            
            int sandX = 500, sandY = 0;
            while (true)
            {
                if (sandY == maxY + 1)
                {
                    tiles[(sandX - 1, sandY + 1)] = TileType.WALL;
                    tiles[(sandX, sandY + 1)] = TileType.WALL;
                    tiles[(sandX + 1, sandY + 1)] = TileType.WALL;
                }
                
                if (!tiles.ContainsKey((sandX, sandY + 1)))
                {
                    sandY++;
                }
                else if (!tiles.ContainsKey((sandX - 1, sandY + 1)))
                {
                    sandY++;
                    sandX--;
                }
                else if (!tiles.ContainsKey((sandX + 1, sandY + 1)))
                {
                    sandY++;
                    sandX++;
                }
                else
                {
                    tiles[(sandX, sandY)] = TileType.SAND;
                    break;
                }
            }

            sandCount++;
        }
    }
}
