using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.IntCode;

namespace AdventOfCode.Solutions
{
    public class Year2019Day07 : Solution
    {
        public override string Part1(string input)
        {
            int[] nums = input.Split(',').Select(int.Parse).ToArray();
            Dictionary<int, int> program = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length; i++)
                program.Add(i, nums[i]);

            IEnumerable<int[]> possiblePhaseSettings = Util.GetPermutations(new[] {0, 1, 2, 3, 4});

            long highestValue = int.MinValue;
            foreach (int[] phaseSettings in possiblePhaseSettings)
            {
                long lastValue = 0;
                for (int i = 0; i < 5; i++)
                {
                    Computer c = new Computer(program);
                    c.input.Enqueue(phaseSettings[i]);
                    c.input.Enqueue(lastValue);
                    while (c.output.Count == 0)
                        c.Step();
                    lastValue = c.output.Dequeue();
                }

                highestValue = Math.Max(highestValue, lastValue);
            }

            return highestValue.ToString();
        }

        public override string Part2(string input)
        {
            int[] nums = input.Split(',').Select(int.Parse).ToArray();
            Dictionary<int, int> program = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length; i++)
                program.Add(i, nums[i]);

            IEnumerable<int[]> possiblePhaseSettings = Util.GetPermutations(new[] {5, 6, 7, 8, 9});

            long highestValue = int.MinValue;
            foreach (int[] phaseSettings in possiblePhaseSettings)
            {
                Computer A = new Computer(program);
                A.input.Enqueue(phaseSettings[0]);
                A.input.Enqueue(0);
                Computer B = new Computer(program);
                B.input.Enqueue(phaseSettings[1]);
                Computer C = new Computer(program);
                C.input.Enqueue(phaseSettings[2]);
                Computer D = new Computer(program);
                D.input.Enqueue(phaseSettings[3]);
                Computer E = new Computer(program);
                E.input.Enqueue(phaseSettings[4]);

                long lastEOutput = int.MinValue;

                while (!A.hasHalted)
                {
                    do
                        A.Step();
                    while (!A.waitingForInput && !A.hasHalted);
                    B.input.Enqueue(A.output.Dequeue());

                    do
                        B.Step();
                    while (!B.waitingForInput && !B.hasHalted);
                    C.input.Enqueue(B.output.Dequeue());

                    do
                        C.Step();
                    while (!C.waitingForInput && !C.hasHalted);
                    D.input.Enqueue(C.output.Dequeue());

                    do
                        D.Step();
                    while (!D.waitingForInput && !D.hasHalted);
                    E.input.Enqueue(D.output.Dequeue());

                    do
                        E.Step();
                    while (!E.waitingForInput && !E.hasHalted);
                    A.input.Enqueue(E.output.Dequeue());

                    lastEOutput = A.input.Peek();
                }

                highestValue = Math.Max(highestValue, lastEOutput);
            }

            return highestValue.ToString();
        }
    }
}