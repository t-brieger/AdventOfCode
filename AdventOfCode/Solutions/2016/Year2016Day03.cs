using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2016Day03 : Solution
    {
        public override string Part1(string input)
        {
            (int a, int b, int c)[] triangles = input.Split('\n')
                .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
                .Select(a => (a[0], a[1], a[2])).ToArray();
            int count = 0;
            foreach ((int a, int b, int c) in triangles)
            {
                if (a + b > c && a + c > b && b + c > a)
                    count++;
            }

            return count.ToString();
        }

        public override string Part2(string input)
        {
            int[] triangles = input.Split(new[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
                .ToArray();
            int count = 0;
            for (int col = 0; col < 3; col++)
            {
                int row = 0;
                while (row < triangles.Length / 3)
                {
                    int a = triangles[row * 3 + col];
                    int b = triangles[row * 3 + 3 + col];
                    int c = triangles[row * 3 + 6 + col];
                    if (a + b > c && a + c > b && b + c > a)
                        count++;
                    row += 3;
                }
            }

            return count.ToString();
        }
    }
}