using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    // ReSharper disable once InconsistentNaming
    public static class Util
    {
        //https://stackoverflow.com/a/41766138
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

        public static IEnumerable<T[]> GetPermutations<T>(T[] values)
        {
            if (values.Length == 1)
                return new[] { values };

            return values.SelectMany(v => GetPermutations(values.Except(new[] { v }).ToArray()),
                (v, p) => new[] { v }.Concat(p).ToArray());
        }
    }
}