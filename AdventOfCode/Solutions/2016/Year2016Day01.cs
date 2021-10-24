using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2016Day01 : Solution
    {
        public override string Part1(string input)
        {
            (bool, int)[] directions = input.Split(',').Select(d => d.Trim())
                .Select(d => (d[0] == 'R', int.Parse(d[1..]))).ToArray();
            (int, int) facing = (0, -1);
            int x = 0;
            int y = 0;
            foreach ((bool turn, int steps) in directions)
            {
                facing = turn ? (-facing.Item2, facing.Item1) : (facing.Item2, -facing.Item1);
                x += facing.Item1 * steps;
                y += facing.Item2 * steps;
            }

            return (Math.Abs(x) + Math.Abs(y)).ToString();
        }

        public override string Part2(string input)
        {
            (bool, int)[] directions = input.Split(',').Select(d => d.Trim())
                .Select(d => (d[0] == 'R', int.Parse(d[1..]))).ToArray();
            (int, int) facing = (0, -1);
            int x = 0;
            int y = 0;
            HashSet<(int, int)> seen = new();
            foreach ((bool turn, int steps) in directions)
            {
                facing = turn ? (-facing.Item2, facing.Item1) : (facing.Item2, -facing.Item1);
                for (int i = 0; i < steps; i++)
                {
                    // only one of the two will ever be non-zero at a time, so we only have to add 1 location per iter
                    if (seen.Contains((x, y)))
                        return (Math.Abs(x) + Math.Abs(y)).ToString();
                    seen.Add((x, y));
                    x += facing.Item1;
                    y += facing.Item2;
                }
            }

            return null;
        }
    }
}