using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2018
{
    public class Year2018Day01 : Solution
    {
        private static IEnumerable<int> Cycle(IEnumerable<int> source)
        {
            List<int> elementBuffer = new List<int>(((ICollection)source).Count);
            elementBuffer.AddRange(source);

            ushort index = 0;
            while (true)
            {
                yield return elementBuffer[index];
                index++;
                index = (ushort)(index % elementBuffer.Count);
            }
            // ReSharper disable once IteratorNeverReturns
        }
        
        public override string Part1(string s)
        {
            List<int> inputs = s.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();
            return inputs.Sum().ToString();
        }

        public override string Part2(string s)
        {
            List<int> inputs = s.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();

            int tmpSum = 0;
            HashSet<int> reachedSums = new HashSet<int>();

            foreach (int i in Cycle(inputs).TakeWhile(i => !reachedSums.Contains(tmpSum)))
            {
                reachedSums.Add(tmpSum);
                tmpSum += i;
            }

            return tmpSum.ToString();
        }
    }
}