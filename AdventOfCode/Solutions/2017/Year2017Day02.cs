using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2017Day02 : Solution
    {
        public override string Part1(string input)
        {
            int sum = 0;
            foreach (string s in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                int max = Int32.MinValue;
                int min = Int32.MaxValue;
                foreach (int i in s.Split('\t').Select(Int32.Parse))
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
            int Foo(int[] row)
            {
                for (int j = 0; j < row.Length; j++)
                {
                    for (int k = j + 1; k < row.Length; k++)
                    {
                        if (row[j] % row[k] == 0 && row[j] != row[k])
                            return row[j] / row[k];
                        if (row[k] % row[j] == 0 && row[j] != row[k])
                            return row[k] / row[j];
                    }
                }

                return 0;
            }
            
            int[][] grid = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split('\t').Select(Int32.Parse).ToArray()).ToArray();

            int sum = grid.Sum(Foo);

            return sum.ToString();
        }
    }
}
