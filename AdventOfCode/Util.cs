using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode
{
    // ReSharper disable once InconsistentNaming
    public static class Util
    {
        // https://stackoverflow.com/a/29717490
        public static long Lcm(params long[] numbers)
        {
            return numbers.Aggregate(lcm);
        }
        private static long lcm(long a, long b)
        {
            return Math.Abs(a * b) / Gcd(a, b);
        }
        
        // https://stackoverflow.com/a/41766138
        public static long Gcd(long a, long b)
        {
            while (true)
            {
                if ((a == 0) ^ (b == 0)) return Math.Abs(a + b);
                if (a == 0 && b == 0) return 1;

                if (Math.Sign(a) != Math.Sign(b))
                {
                    a *= -1;
                    continue;
                }

                if (a < 0 && b < 0)
                {
                    a *= -1;
                    b *= -1;
                }

                while (a != 0 && b != 0)
                    if (a > b)
                        a %= b;
                    else
                        b %= a;

                return a | b;
            }
        }

        // https://stackoverflow.com/a/10629938
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T[]> GetPermutations<T>(IEnumerable<T> list, int length = 2) where T : IComparable
        {
            if (length == 1) return list.Select(t => new[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(o => !t.Contains(o)),
                    (t1, t2) => t1.Concat(new[] { t2 }).ToArray());
        }
    }
}