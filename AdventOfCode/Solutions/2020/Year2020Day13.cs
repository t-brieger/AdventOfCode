using System;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Solutions
{
    public class Year2020Day13 : Solution
    {
        public override string Part1(string input)
        {
            string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            int[] busses = lines[1].Split(',').Where(s => s != "x").Select(int.Parse).ToArray();
            int origTimestamp = int.Parse(lines[0]);
            for (int timestamp = origTimestamp;; timestamp++)
                foreach (int bus in busses.Where(bus => timestamp % bus == 0))
                    return (bus * (timestamp - origTimestamp)).ToString();
        }

        private static long ModInv(long a, long m)
        {
            return (long)BigInteger.ModPow(a, m - 2, m);
        }

        // https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
        private static long ChineseRemainderTheorem((long mod, long a)[] items)
        {
            long prod = items.Aggregate(1L, (acc, item) => acc * item.mod);
            long sum = items.Select((item, _) =>
            {
                (long mod, long a) = item;
                long p = prod / mod;
                return a * ModInv(p, mod) * p;
            }).Sum();

            return sum % prod;
        }

        public override string Part2(string input)
        {
            (long, int)[] busses = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)[1]
                .Split(',').Select((str, ix) => (str, ix)).Where(x => x.str != "x")
                .Select(i => (long.Parse(i.str), i.ix)).ToArray();
            return ChineseRemainderTheorem(busses.Select(bus => (mod: bus.Item1, a: bus.Item1 - bus.Item2)).ToArray())
                .ToString();
        }
    }
}