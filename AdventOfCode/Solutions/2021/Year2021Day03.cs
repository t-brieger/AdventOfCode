using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2021Day03 : Solution
    {
        public override string Part1(string input)
        {
            string[] numbers = input.Split('\n');
            int bits = numbers[0].Length;

            int gamma = 0;

            for (int i = 0; i < bits; i++)
            {
                gamma <<= 1;
                int highAmount = numbers.Count(num => num[i] == '1');

                if (highAmount > numbers.Length / 2)
                    gamma++;
            }

            return (gamma * (~gamma & ((1 << bits) - 1))).ToString();
        }

        public override string Part2(string input)
        {
            string[] numbers = input.Split('\n');
            HashSet<string> candidates = new(numbers);

            for (int i = 0; i < numbers[0].Length && candidates.Count > 1; i++)
            {
                int highCount = candidates.Count(s => s[i] == '1');
                char neededToPass = highCount >= candidates.Count / 2f ? '1' : '0';
                candidates = candidates.Where(s => s[i] == neededToPass).ToHashSet();
            }
            int oxygenRating = Convert.ToInt32(candidates.First(), 2);
            
            candidates = new HashSet<string>(numbers);
            for (int i = 0; i < numbers[0].Length && candidates.Count > 1; i++)
            {
                int highCount = candidates.Count(s => s[i] == '1');
                char neededToPass = highCount >= candidates.Count / 2f ? '0' : '1';
                candidates = candidates.Where(s => s[i] == neededToPass).ToHashSet();
            }
            int co2Rating = Convert.ToInt32(candidates.First(), 2);

            return (co2Rating * oxygenRating).ToString();
        }
    }
}