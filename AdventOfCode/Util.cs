using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
    public class Util
    {
        public static IEnumerable<T[]> GetPermutations<T>(T[] values)
        {
            if (values.Length == 1)
                return new[] {values};

            return values.SelectMany(v => GetPermutations(values.Except(new[] {v}).ToArray()),
                (v, p) => new[] {v}.Concat(p).ToArray());
        }
    }
}