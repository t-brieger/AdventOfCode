using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2017
{
    public class Year2017Day13 : Solution
    {
        public override string Part1(string input)
        {
            IEnumerable<(int, int)> scanners = input.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                {
                    string[] y = x.Split(':');
                    return (int.Parse(y[0]), int.Parse(y[1]));
                });

            int severity = 0;

            foreach ((int, int) scanner in scanners)
                if (scanner.Item1 % (2 * (scanner.Item2 - 1)) == 0)
                    severity += scanner.Item1 * scanner.Item2;

            return severity.ToString();
        }

        public override string Part2(string input)
        {
            IEnumerable<(int depth, int range)> scanners = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select<string, (int depth, int range)>(x =>
                {
                    string[] y = x.Split(':');
                    return (int.Parse(y[0]), int.Parse(y[1]));
                });
            
            for (int i = 0; true; i++)
                if (scanners.All(scanner => (scanner.depth + i) % (2 * (scanner.range - 1)) != 0))
                        return i.ToString();
        }
    }
}