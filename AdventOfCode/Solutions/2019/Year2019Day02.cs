using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2019
{
    public class Year2019Day02 : Solution
    {
        private class Day2Comp : ICComputer
        {
            public Day2Comp(int[] mem) : base(mem, new Queue<int>())
            {
            }

            public override int? isDone()
            {
                return pc >= memory.Length || hasHalted ? memory[0] : (int?)null;
            }
        }

        public override string Part1(string input)
        {
            int[] mem = input.Split(',').Select(int.Parse).ToArray();

            mem[1] = 12;
            mem[2] = 02;

            Day2Comp computer = new Day2Comp(mem);

            while (computer.isDone() == null)
                computer.doInstruction();

            return computer.isDone().ToString();
        }

        // not the most elegant solution, but still completes in 0.1 seconds on my machine, so it's not a giant problem
        public override string Part2(string input)
        {
            int[] oldmem = input.Split(',').Select(int.Parse).ToArray();

            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    int[] mem = new int[oldmem.Length];
                    System.Array.Copy(oldmem, 0, mem, 0, mem.Length);

                    mem[1] = noun;
                    mem[2] = verb;

                    Day2Comp computer = new Day2Comp(mem);

                    while (computer.isDone() == null)
                        computer.doInstruction();

                    if (computer.isDone() == 19690720)
                        return (100 * noun + verb).ToString();
                }
            }

            return "";
        }
    }
}