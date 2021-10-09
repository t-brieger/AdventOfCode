using System;
using System.Collections.Generic;
using AdventOfCode.Solutions.IntCode;

namespace AdventOfCode.Solutions
{
    public class Year2019Day13 : Solution
    {
        public override string Part1(string input)
        {
            Computer c = new(input);
            c.RunUntilHalted();
            
            HashSet<(long, long)> blockTiles = new();
            
            while (c.output.Count > 0)
            {
                long x = c.output.Dequeue();
                long y = c.output.Dequeue();
                long tileType = c.output.Dequeue();

                if (tileType == 2)
                    blockTiles.Add((x, y));
                else
                    blockTiles.Remove((x, y));
            }

            return blockTiles.Count.ToString();
        }

        public override string Part2(string input)
        {
            Computer c = new(input);
            c.SetMemoryAt(0, 2);
            long lastScore = -1;
            while (!c.hasHalted)
            {
                c.RunUntilHalted();
                long ballX = 0, paddleX = 0;

                while (c.output.Count > 0)
                {
                    long x = c.output.Dequeue();
                    long y = c.output.Dequeue();
                    long type = c.output.Dequeue();

                    if (x == -1 && y == 0)
                    {
                        lastScore = type;
                        continue;
                    }

                    if (type == 3)
                        paddleX = x;
                    if (type == 4)
                        ballX = x;
                }

                long dx = ballX - paddleX;

                c.EnqueueInput(dx / (dx == 0 ? 1 : Math.Abs(dx)));
            }

            return lastScore.ToString();
        }
    }
}