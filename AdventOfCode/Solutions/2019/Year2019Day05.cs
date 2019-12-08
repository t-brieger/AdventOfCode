using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2019
{
    public class Year2019Day05 : Solution
    {
        private class Day5Comp : ICComputer
        {
            public override int? isDone()
            {
                return hasHalted || pc >= memory.Length ? output.LastOrDefault() : (int?) null;
            }

            public Day5Comp(int[] mem, Queue<int> input) : base(mem, input)
            {
            }

            public Day5Comp(int[] mem) : base(mem)
            {
            }
        }

        public override string Part1(string input)
        {
            /*
            input = "1002,4,3,4,33";
            //*/

            int[] mem = input.Split(',').Select(int.Parse).ToArray();

            var computer = new Day5Comp(mem, new Queue<int>(new[] {1}));

            while (computer.isDone() == null)
                computer.doInstruction();

            return computer.isDone().ToString();
        }

        public override string Part2(string input)
        {
            /*
            input = "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99";
            //*/
            int[] mem = input.Split(',').Select(int.Parse).ToArray();

            var computer = new Day5Comp(mem, new Queue<int>(new[] {5}));

            while (computer.isDone() == null)
                computer.doInstruction();

            return computer.isDone().ToString();
        }
    }
}