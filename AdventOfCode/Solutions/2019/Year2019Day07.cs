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
            Dictionary<int, int> program = new();
            for (int i = 0; i < nums.Length; i++)
                program.Add(i, nums[i]);

            IEnumerable<int[]> possiblePhaseSettings = Util.GetPermutations(new[] { 0, 1, 2, 3, 4 }, 5);

            long highestValue = int.MinValue;
            foreach (int[] phaseSettings in possiblePhaseSettings)
            {
                long lastValue = 0;
                for (int i = 0; i < 5; i++)
                {
                    Computer c = new(program);
                    c.EnqueueInput(phaseSettings[i]);
                    c.EnqueueInput(lastValue);
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
            Dictionary<int, int> program = new();
            for (int i = 0; i < nums.Length; i++)
                program.Add(i, nums[i]);

            List<int[]> possiblePhaseSettings = new List<int[]>(Util.GetPermutations(new[] { 5, 6, 7, 8, 9 }, 5));

            long highestValue = int.MinValue;
            foreach (int[] phaseSettings in possiblePhaseSettings)
            {
                Computer a = new(program);
                a.EnqueueInput(phaseSettings[0]);
                a.EnqueueInput(0);
                Computer b = new(program);
                b.EnqueueInput(phaseSettings[1]);
                Computer c = new(program);
                c.EnqueueInput(phaseSettings[2]);
                Computer d = new(program);
                d.EnqueueInput(phaseSettings[3]);
                Computer e = new(program);
                e.EnqueueInput(phaseSettings[4]);

                long lastEOutput = int.MinValue;

                while (!a.hasHalted && !b.hasHalted && !c.hasHalted && !d.hasHalted && !e.hasHalted)
                {
                    do
                    {
                        a.Step();
                    } while (!a.waitingForInput && !a.hasHalted);

                    b.EnqueueInput(a.output.Dequeue());

                    do
                    {
                        b.Step();
                    } while (!b.waitingForInput && !b.hasHalted);

                    c.EnqueueInput(b.output.Dequeue());

                    do
                    {
                        c.Step();
                    } while (!c.waitingForInput && !c.hasHalted);

                    d.EnqueueInput(c.output.Dequeue());

                    do
                    {
                        d.Step();
                    } while (!d.waitingForInput && !d.hasHalted);

                    e.EnqueueInput(d.output.Dequeue());

                    do
                    {
                        e.Step();
                    } while (!e.waitingForInput && !e.hasHalted);

                    a.EnqueueInput(e.output.Dequeue());

                    lastEOutput = a.input.Peek();
                }

                highestValue = Math.Max(highestValue, lastEOutput);
            }

            return highestValue.ToString();
        }
    }
}