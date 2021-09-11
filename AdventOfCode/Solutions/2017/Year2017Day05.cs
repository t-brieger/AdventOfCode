using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2017Day05 : Solution
    {
        public override string Part1(string input)
        {
            int[] jumps = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();

            int cycles = 0;
            int pc = 0;

            while (true)
            {
                cycles++;
                int destination = pc + jumps[pc]++;
                if (destination >= jumps.Length || destination < 0)
                    return cycles.ToString();
                pc = destination;
            }
        }

        public override string Part2(string input)
        {
            int[] jumps = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();

            int cycles = 0;
            int pc = 0;

            while (true)
            {
                cycles++;
                int destination = pc + (jumps[pc] >= 3 ? jumps[pc]-- : jumps[pc]++);
                if (destination >= jumps.Length || destination < 0)
                    return cycles.ToString();
                pc = destination;
            }
        }
    }
}