using System;
using System.Collections.Generic;
using System.Linq;
using ILGPU;

namespace AdventOfCode.Solutions;

public class Year2018Day15 : Solution
{
    private class Entity
    {
        public bool IsElf;
        public (int x, int y) Position;
        public int Hitpoints;
        public int Id;
    }

    private void SimulateTurn(Dictionary<int, Entity> entities, bool[,] walls)
    {
        List<int> turnOrder = new();
        for (int y = 0; y < walls.GetLength(1); y++)
        {
            for (int x = 0; x < walls.GetLength(0); x++)
            {
                Entity e = entities.Values.FirstOrDefault(e => e.Position == (x, y));
                if (e != null)
                    turnOrder.Add(e.Id);
            }
        }

        (int, int)[] offsets =
        {
            (0, -1), (-1, 0), (1, 0), (0, 1)
        };
        foreach (int turn in turnOrder)
        {
            Entity turnEntity = entities[turn];
            // the return of spaghetti linq
            (int x, int y)[] targetSquares = entities.Values.Where(e => e.IsElf != turnEntity.IsElf)
                .SelectMany(e => offsets.Select(o => (e.Position.x + o.Item1, e.Position.y + o.Item2)))
                .Where(pos => !entities.Any(kvp => kvp.Key != turn && kvp.Value.Position == pos)).Distinct().ToArray();
            if (!targetSquares.Contains(turnEntity.Position))
            {
                // try to move, we can't attack yet
            }
            // attack if we can
            
        }
    }

    private bool IsGameOver(Dictionary<int, Entity> entities)
    {
        return entities.All(e => e.Value.IsElf) || entities.All(e => !e.Value.IsElf);
    }

    public override string Part1(string input)
    {
        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // key is used for turn order.
        Dictionary<int, Entity> entities = new();

        bool[,] walls = new bool[lines[0].Length, lines.Length];

        int maxEntityId = 0;
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                char c = lines[y][x];
                walls[x, y] = c == '#';

                if (c is 'E' or 'G')
                {
                    Entity e = new Entity
                    {
                        Hitpoints = 200,
                        Position = (x, y),
                        IsElf = c == 'E',
                        Id = maxEntityId
                    };
                    entities[maxEntityId++] = e;
                }
            }
        }

        int rounds = 0;
        while (!IsGameOver(entities))
        {
            rounds++;
            SimulateTurn(entities, walls);
        }

        return (rounds * entities.Values.Sum(e => e.Hitpoints)).ToString();
    }

    public override string Part2(string input)
    {
        throw new System.NotImplementedException();
    }
}