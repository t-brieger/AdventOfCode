using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2017
{
    public class Year2017Day02 : Solution
    {
        public override string Part1(string input)
        {
            int sum = 0;
            foreach (string s in input.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries))
            {
                int max = int.MinValue;
                int min = int.MaxValue;
                foreach (int i in s.Split('\t').Select(int.Parse))
                {
                    if (i < min)
                        min = i;
                    if (i > max)
                        max = i;
                }

                sum += max - min;
            }

            return sum.ToString();
        }

        public override string Part2(string input)
        {
            int foo(int[] row)

        {
                for (int j = 0; j < row.Length; j++)
                {
                    for (int k = j + 1; k < row.Length; k++)
                    {
                        if (row[j] % row[k] == 0 && row[j] != row[k])
                            return row[j] / row[k];
                        else if (row[k] % row[j] == 0 && row[j] != row[k])
                            return row[k] / row[j];
                    }
                }

                return 0;
            }

            //for duplicates
            HashSet<int> alreadyChecked = new HashSet<int>();

            int[][] grid = input.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split('\t').Select(int.Parse).ToArray()).ToArray();

            int sum = 0;
            for (int i = 0; i < grid.Length; i++)
            {
                sum += foo(grid[i]);
            }

            return sum.ToString();
        }
    }
}