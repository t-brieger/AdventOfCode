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

    private static Direction GetInverseDirection(Direction d)
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
        floorTiles.Add((0, 0), (new Direction[] { }).ToList());

        PriorityQueue<(int, int, int), int> toExplore = new();
        toExplore.Enqueue((0, 0, 0), 0);

        while (toExplore.Count != 0)
        {
            // assume we are at (0, 0) at the start of each iteration

            (int exploreX, int exploreY, int cost) = toExplore.Dequeue();

            // go to field
            List<Direction> path = floorTiles[(exploreX, exploreY)];
            foreach (Direction d in path)
                explorer.EnqueueInput((int) d);
            explorer.RunUntilHalted();
            foreach (Direction _ in path)
                if (explorer.output.Dequeue() != (long) TileTypes.FLOOR)
                    throw new Exception();

            // try out all 4 directions, see which are floor
            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                explorer.EnqueueInput((int) d);
                explorer.RunUntilHalted();
                TileTypes t = (TileTypes) explorer.output.Dequeue();
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
                    explorer.EnqueueInput((long) GetInverseDirection(d));
                    explorer.RunUntilHalted();
                    if (explorer.output.Dequeue() != (long) TileTypes.FLOOR)
                        throw new ArgumentException();
                }
                else if (t == TileTypes.GENERATOR)
                {
                    return (cost + 1).ToString();
                }
            }

            // move back to 0,0
            for (int i = path.Count - 1; i >= 0; i--)
            {
                explorer.EnqueueInput((long) GetInverseDirection(path[i]));
                explorer.RunUntilHalted();
                if (explorer.output.Dequeue() != (long) TileTypes.FLOOR)
                    throw new ArgumentException();
            }
        }


        return null;
    }

    // EXTREMELY lazy - copy paste part 1 code, find generator, then djikstra *again* (starting at gen) and find longest distance
    public override string Part2(string input)
    {
        Computer explorer = new(input);
        Dictionary<(int, int), List<Direction>> floorTiles = new();
        floorTiles.Add((0, 0), (new Direction[] { }).ToList());

        PriorityQueue<(int, int, int), int> toExplore = new();
        toExplore.Enqueue((0, 0, 0), 0);

        int genX = int.MaxValue, genY = int.MaxValue;
        Direction[] genPath = null;

        while (toExplore.Count != 0 && genPath == null)
        {
            (int exploreX, int exploreY, int cost) = toExplore.Dequeue();

            // go to field
            List<Direction> path = floorTiles[(exploreX, exploreY)];
            foreach (Direction d in path)
                explorer.EnqueueInput((int) d);
            explorer.RunUntilHalted();
            foreach (Direction _ in path)
                if (explorer.output.Dequeue() != (long) TileTypes.FLOOR)
                    throw new Exception();

            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                explorer.EnqueueInput((int) d);
                explorer.RunUntilHalted();
                TileTypes t = (TileTypes) explorer.output.Dequeue();
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
                    explorer.EnqueueInput((long) GetInverseDirection(d));
                    explorer.RunUntilHalted();
                    if (explorer.output.Dequeue() != (long) TileTypes.FLOOR)
                        throw new ArgumentException();
                }
                else if (t == TileTypes.GENERATOR)
                {
                    genX = coordinates.Item1;
                    genY = coordinates.Item2;
                    genPath = newPath.ToArray();
                    break;
                }
            }

            // move back to 0,0
            for (int i = path.Count - 1; i >= 0; i--)
            {
                explorer.EnqueueInput((long) GetInverseDirection(path[i]));
                explorer.RunUntilHalted();
                while (explorer.output.Count != 0)
                    explorer.output.Dequeue();
            }
        }

        if (genPath == null)
            throw new ArgumentException("no accessible generator");

        int longestPathLength = -1;
        // reset, then do the same thing again, but start at generator
        explorer = new(input);
        floorTiles = new();
        floorTiles.Add((genX, genY), (new Direction[] { }).ToList());
        toExplore = new();
        toExplore.Enqueue((genX, genY, 0), 0);

        foreach (Direction d in genPath)
            explorer.EnqueueInput((long) d);
        explorer.RunUntilHalted();
        while (explorer.output.Count != 0)
            explorer.output.Dequeue();

        while (toExplore.Count != 0)
        {
            (int exploreX, int exploreY, int cost) = toExplore.Dequeue();

            // go to field
            List<Direction> path = floorTiles[(exploreX, exploreY)];
            foreach (Direction d in path)
                explorer.EnqueueInput((int) d);
            explorer.RunUntilHalted();
            while (explorer.output.Count != 0)
                explorer.output.Dequeue();

            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                explorer.EnqueueInput((int) d);
                explorer.RunUntilHalted();
                TileTypes t = (TileTypes) explorer.output.Dequeue();
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
                        longestPathLength = (cost + 1) > longestPathLength ? cost + 1 : longestPathLength;
                    }

                    // move back
                    explorer.EnqueueInput((long) GetInverseDirection(d));
                    explorer.RunUntilHalted();
                    while (explorer.output.Count != 0)
                        explorer.output.Dequeue();
                }
            }

            // move back to gen position
            for (int i = path.Count - 1; i >= 0; i--)
            {
                explorer.EnqueueInput((long) GetInverseDirection(path[i]));
                explorer.RunUntilHalted();
                while (explorer.output.Count != 0)
                    explorer.output.Dequeue();
            }
        }


        return longestPathLength.ToString();
    }
}
