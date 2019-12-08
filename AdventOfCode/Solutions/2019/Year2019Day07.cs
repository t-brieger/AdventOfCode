using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2019
{
    public class Year2019Day07 : Solution
    {
        private class Day7Comp : ICComputer
        {
            public override int? isDone()
            {
                return hasHalted || pc >= memory.Length ? output.LastOrDefault() : (int?)null;
            }

            public Day7Comp(int[] mem, Queue<int> input) : base(mem, input)
            {
            }

            public Day7Comp(int[] mem) : base(mem)
            {
            }
        }

        public override string Part1(string input)
        {
            int[] mem = input.Split(',').Select(int.Parse).ToArray();

            IEnumerable<int[]> permutations = Util.GetPermutations(new int[] {0, 1, 2, 3, 4});

            int max = int.MinValue;
            foreach (int[] settings in permutations)
            {
                int lastOutput = 0;
                
                for (int i = 0; i < 5; i++)
                {
                    int[] memcpy = new int[mem.Length];
                    Array.Copy(mem, 0, memcpy, 0, mem.Length);
                    Day7Comp computer = new Day7Comp(memcpy, new Queue<int>(new[] { settings[i], lastOutput }));
                    
                    while (computer.isDone() == null)
                        computer.doInstruction();

                    lastOutput = (int) computer.isDone();
                }

                if (lastOutput > max)
                    max = lastOutput;
            }

            return max.ToString();
        }

        public override string Part2(string input)
        {
            //*
            input = "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5";
            //*/

            int[] mem = input.Split(',').Select(int.Parse).ToArray();

            IEnumerable<int[]> permutations = Util.GetPermutations(new int[] {5, 6, 7, 8, 9});

            int max = int.MinValue;

            foreach (int[] perm in permutations)
            {
                Day7Comp[] computers = new Day7Comp[5];
            
                for (int i = 0; i < computers.Length; i++)
                {
                    var memcopy = new int[mem.Length];
                    Array.Copy(mem, memcopy, mem.Length);

                    computers[i] = new Day7Comp(mem);
                }

                for (int i = 0; i < computers.Length; i++)
                {
                    computers[(i + 1) % 5].input = computers[i].output;
                    computers[(i + 1) % 5].input.Enqueue(perm[(i + 1) % 5]);
                }
                
                computers[0].input.Enqueue(0);
                
                while (!computers.Any(c => c.hasHalted))
                    foreach (var c in computers)
                        c.doInstruction();

                var result = computers[0].input.Dequeue();
                if (result > max)
                    max = result;
            }
            
            return max.ToString();
        }
    }
}