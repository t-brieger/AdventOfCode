using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.IntCode;

namespace AdventOfCode.Solutions;

public class Year2019Day15 : Solution
{
    private enum TileTypes
    {
        WALL = 0,
        FLOOR = 1,
        GENERATOR = 2
    }
    
    private enum Direction
    {
        NORTH = 1,
        SOUTH = 2,
        WEST = 3,
        EAST = 4
    }

    private Direction getInverseDirection(Direction d)
    {
        switch (d)
        {
            case Direction.NORTH:
                return Direction.SOUTH;
            case Direction.SOUTH:
                return Direction.NORTH;
            case Direction.WEST:
                return Direction.EAST;
            case Direction.EAST:
                return Direction.WEST;
        }

        throw new ArgumentException();
    }
    
    public override string Part1(string input)
    {
        Computer explorer = new(input);
        Dictionary<(int, int), List<Direction>> floorTiles = new();
        floorTiles.Add((0, 0), (new Direction[]{ }).ToList());

        PriorityQueue<(int, int, int), int> toExplore = new();
        toExplore.Enqueue((0, 0, 0), 0);
        
        while (toExplore.Count != 0)
        {
            // assume we are at (0, 0) at the start of each iteration

            (int exploreX, int exploreY, int cost) = toExplore.Dequeue();
            
            // go to field
            List<Direction> path = floorTiles[(exploreX, exploreY)];
            foreach (Direction d in path)
                explorer.EnqueueInput((int)d);
            explorer.RunUntilHalted();
            foreach (Direction _ in path)
                if (explorer.output.Dequeue() != (long)TileTypes.FLOOR)
                    throw new Exception();
            
            // try out all 4 directions, see which are floor
            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                explorer.EnqueueInput((int)d);
                explorer.RunUntilHalted();
                TileTypes t = (TileTypes)explorer.output.Dequeue();
                List<Direction> newPath = new List<Direction>(path);
                newPath.Add(d);
                (int, int) coordinates = (exploreX + (d == Direction.EAST ? 1 : d == Direction.WEST ? -1 : 0),
                    exploreY + (d == Direction.NORTH ? 1 : d == Direction.SOUTH ? -1 : 0));
                if (t == TileTypes.FLOOR)
                {
                    if (!floorTiles.ContainsKey(coordinates))
                    {
                        floorTiles.Add(coordinates, newPath);
                        toExplore.Enqueue((coordinates.Item1, coordinates.Item2, cost + 1), cost + 1);
                    }
                    
                    // move back
                    explorer.EnqueueInput((long)this.getInverseDirection(d));
                    explorer.RunUntilHalted();
                    if (explorer.output.Dequeue() != (long)TileTypes.FLOOR)
                        throw new ArgumentException();
                } else if (t == TileTypes.GENERATOR)
                {
                    return (cost + 1).ToString();
                }
            }
            
            // move back to 0,0
            for (int i = path.Count - 1; i >= 0; i--)
            {
                explorer.EnqueueInput((long)this.getInverseDirection(path[i]));
                explorer.RunUntilHalted();
                if (explorer.output.Dequeue() != (long)TileTypes.FLOOR)
                    throw new ArgumentException();
            }
        }
        
        
        return null;
    }

    public override string Part2(string input)
    {
        // TODO
        return null;
    }
}