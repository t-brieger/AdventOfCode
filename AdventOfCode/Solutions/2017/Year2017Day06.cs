using System;
using System.Collections.Generic;
using System.Linq;

using veryLongTuple = System.Tuple<int, int, int, int, int, int, int, System.Tuple<int, int, int, int, int, int, int, System.Tuple<int, int>>>;

namespace AdventOfCode.Solutions
{
    public class Year2017Day06 : Solution
    {
        //TODO: un-jankify
        private static veryLongTuple GetLongAssTupleFromArray(int[] state) => new veryLongTuple(state[0], state[1], state[2], state[3], state[4], state[5], state[6], new Tuple<int, int, int, int, int, int, int, Tuple<int, int>>(state[7], state[8], state[9], state[10], state[11], state[12], state[13], new Tuple<int, int>(state[14], state[15])));

        public override string Part1(string input)
        {
            HashSet<veryLongTuple> seen = new HashSet<veryLongTuple>();

            int[] currentState = input.Split('\t').Select(Int32.Parse).ToArray();

            int cycles = 0;
            while (true)
            {
                cycles++;
                seen.Add(GetLongAssTupleFromArray(currentState));
                int max = 0;
                for (int i = 1; i < currentState.Length; i++)
                {
                    if (currentState[i] > currentState[max])
                        max = i;
                }

                int blocksToDistribute = currentState[max];
                currentState[max] = 0;
                for (int i = 1; i <= blocksToDistribute; i++)
                {
                    currentState[(max + i) % currentState.Length]++;
                }

                if (seen.Contains(GetLongAssTupleFromArray(currentState)))
                    return cycles.ToString();
            }
        }

        public override string Part2(string input)
        {
            Dictionary<veryLongTuple, int> seen = new Dictionary<veryLongTuple, int>();

            int[] currentState = input.Split('\t').Select(Int32.Parse).ToArray();

            int cycles = 0;
            while (true)
            {
                seen.Add(GetLongAssTupleFromArray(currentState), cycles);
                int max = 0;
                for (int i = 1; i < currentState.Length; i++)
                {
                    if (currentState[i] > currentState[max])
                        max = i;
                }

                int blocksToDistribute = currentState[max];
                currentState[max] = 0;
                for (int i = 1; i <= blocksToDistribute; i++)
                {
                    currentState[(max + i) % currentState.Length]++;
                }

                cycles++;
                if (seen.ContainsKey(GetLongAssTupleFromArray(currentState)))
                    return (cycles - seen[GetLongAssTupleFromArray(currentState)]).ToString();
            }
        }
    }
}