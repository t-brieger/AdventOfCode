using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2020Day06 : Solution
    {
        public override string Part1(string input)
        {
            string[] groups = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
            if (groups.Length == 1)
                //unix line endings
                groups = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

            int[] x = groups.Select(g =>
                g.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).SelectMany(x => x).Distinct()
                    .Count()).ToArray();
            
            return x.Sum().ToString();
        }

        public override string Part2(string input)
        {
            string[] groups = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
            if (groups.Length == 1)
                //unix line endings
                groups = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

            int[] x = groups.Select(g =>
                g.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)).Select(group =>
            {
                int letterCount = 0;
                for (char c = 'a'; c <= 'z'; c++)
                    if (group.All(s => s.Contains(c)))
                        letterCount++;

                return letterCount;
            }).ToArray();

            return x.Sum().ToString();
        }
    }
}