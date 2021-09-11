using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2017Day13 : Solution
    {
        public override string Part1(string input)
        {
            IEnumerable<(int, int)> scanners = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                {
                    string[] y = x.Split(':');
                    return (Int32.Parse(y[0]), Int32.Parse(y[1]));
                });

            int severity = scanners.Where(scanner => scanner.Item1 % (2 * (scanner.Item2 - 1)) == 0)
                .Sum(scanner => scanner.Item1 * scanner.Item2);

            return severity.ToString();
        }

        public override string Part2(string input)
        {
            (int depth, int range)[] scanners = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select<string, (int depth, int range)>(x =>
                {
                    string[] y = x.Split(':');
                    return (Int32.Parse(y[0]), Int32.Parse(y[1]));
                }).ToArray();

            for (int i = 0;; i++)
                if (scanners.All(scanner => (scanner.depth + i) % (2 * (scanner.range - 1)) != 0))
                    return i.ToString();
        }
    }
}