using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2020Day03 : Solution
    {
        public override string Part1(string input)
        {
            bool[][] isTree = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Select(c => c == '#').ToArray()).ToArray();

            int x = 0, y = 0;
            int treesEncountered = 0;

            while (y < isTree.Length)
            {
                if (isTree[y][x % isTree[0].Length])
                    treesEncountered++;
                x += 3;
                y++;
            }

            return treesEncountered.ToString();
        }

        public override string Part2(string input)
        {
            bool[][] isTree = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Select(c => c == '#').ToArray()).ToArray();

            int x = 0, y = 0;
            int[] treesEncountereds = { 0, 0, 0, 0, 0 };

            for (int i = 0; i < treesEncountereds.Length; i++)
            {
                while (y < isTree.Length)
                {
                    if (isTree[y][x % isTree[0].Length])
                        treesEncountereds[i]++;
                    x += i switch
                    {
                        1 => 3,
                        2 => 5,
                        3 => 7,
                        _ => 1
                    };
                    y++;
                    if (i == treesEncountereds.Length - 1)
                        y++;
                }

                y = 0;
                x = 0;
            }

            return treesEncountereds.Aggregate(1, (total, i) => total * i).ToString();
        }
    }
}