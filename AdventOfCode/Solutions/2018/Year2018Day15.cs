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
                // this is very unoptimised but it's good enough:
                // 
                // check distance to all opposite-team entities
                //   (get rid of distances that are larger than current min)
                // select "first" match (shortest distance, reading order)
                // try to pathfind offset to all 4 directions
                // move
                // OR
                // just attack
                Entity turnEntity = entities[turn];
                if (turnEntity.Hitpoints <= 0)
                    continue;

                bool canAttack = false;
                foreach ((int ox, int oy) in offsets)
                {
                    Entity attackee = entities.Values.FirstOrDefault(e =>
                        e.IsElf != turnEntity.IsElf &&
                        e.Position == (turnEntity.Position.x + ox, turnEntity.Position.y + oy));
                    if (attackee != null)
                    {
                        canAttack = true;
                        break;
                    }
                }

                if (!canAttack)
                {
                    ((int, int) TargetTile, int pathLength) = Util.Djikstra(turnEntity.Position, (pos, cost) =>
                        {
                            (int x, int y) = pos;
                            List<((int, int), int)> ret = new();
                            foreach ((int ox, int oy) in offsets)
                            {
                                if (!walls[x + ox, y + oy] && !entities.Values.Any(e => e.Position == (x + ox, y + oy)))
                                    ret.Add(((x + ox, y + oy), cost + 1));
                            }

                            return ret;
                        },
                        pos =>
                        {
                            return entities.Values.Any(e =>
                                e.IsElf != turnEntity.IsElf &&
                                offsets.Any(o => pos == (e.Position.x + o.Item1, e.Position.y + o.Item2)));
                        });

                    if (pathLength == -1)
                        // no attack target found; skip our turn.
                        continue;

                    foreach ((int, int) offset in offsets)
                    {
                        if (walls[turnEntity.Position.x + offset.Item1, turnEntity.Position.y + offset.Item2])
                            continue;
                        if (entities.Values.Any(e =>
                                e.Position == (turnEntity.Position.x + offset.Item1,
                                    turnEntity.Position.y + offset.Item2)))
                            continue;

                        // we expect the (best) path length here to be 1 less than pathLength, because we're starting
                        // 1 tile closer to our target.
                        (_, int length) = Util.Djikstra(
                            (turnEntity.Position.x + offset.Item1, turnEntity.Position.y + offset.Item2),
                            (pos, cost) =>
                            {
                                if (cost >= pathLength - 1)
                                    return new List<((int, int), int)>(Array.Empty<((int, int), int)>());

                                (int x, int y) = pos;
                                List<((int, int), int)> ret = new();
                                foreach ((int ox, int oy) in offsets)
                                {
                                    if (!walls[x + ox, y + oy] &&
                                        !entities.Values.Any(e => e.Position == (x + ox, y + oy)))
                                        ret.Add(((x + ox, y + oy), cost + 1));
                                }

                                return ret;
                            }, pos => pos == TargetTile);
                        if (length == pathLength - 1)
                        {
                            turnEntity.Position = (turnEntity.Position.x + offset.Item1,
                                turnEntity.Position.y + offset.Item2);
                            break;
                        }
                    }
                }

                (int fewestHitpoints, Entity lowestHpEntity) = (int.MaxValue, null);
                foreach ((int ox, int oy) in offsets)
                {
                    Entity attackee = entities.Values.FirstOrDefault(e =>
                        e.IsElf != turnEntity.IsElf &&
                        e.Position == (turnEntity.Position.x + ox, turnEntity.Position.y + oy));
                    if (attackee != null)
                    {
                        if (attackee.Hitpoints >= fewestHitpoints)
                            continue;
                        (fewestHitpoints, lowestHpEntity) = (attackee.Hitpoints, attackee);
                    }
                }

                if (lowestHpEntity != null)
                    lowestHpEntity.Hitpoints -= 3;
            }

            entities = new Dictionary<int, Entity>(entities.Where(kvp => kvp.Value.Hitpoints > 0));
        }

        return (rounds * entities.Values.Sum(e => e.Hitpoints)).ToString();
    }

    public override string Part2(string input)
    {
        throw new System.NotImplementedException();
    }
}