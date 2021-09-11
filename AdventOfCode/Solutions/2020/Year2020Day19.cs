using System.Collections.Generic;
using System.Linq;

// ReSharper disable InconsistentNaming

namespace AdventOfCode.Solutions
{
    public class Year2020Day19 : Solution
    {
        private bool match(string message, IList<int> nums, IReadOnlyDictionary<int, Rule> rules)
        {
            if (nums.Count == 0)
                return message.Length == 0;
            Rule rule = rules[nums[0]];
            nums.RemoveAt(0);
            if (rule is CharRule cr) return message.StartsWith(cr.value) && this.match(message[1..], nums, rules);

            int[][] ints = (rule as NestedRule)?.sub;
            if (ints == null) return false;
            foreach (List<int> tempList in ints.Select(sub => sub.ToList()))
            {
                tempList.AddRange(nums);
                if (this.match(message, tempList, rules)) return true;
            }

            return false;
        }

        public override string Part1(string input)
        {
            string[] splitByDoubleNewline = input.Split("\n\n");
            string[] ruleStrings = splitByDoubleNewline[0].Split("\n");
            string[] messages = splitByDoubleNewline[1].Split("\n");
            Dictionary<int, Rule> rules = new();
            foreach (string line in ruleStrings)
            {
                string[] split = line.Split(": ");
                int id = int.Parse(split[0]);
                if (split[1].Contains("\""))
                {
                    split = split[1].Split("\"");
                    rules.Add(id, new CharRule { value = split[1][0] });
                }
                else
                {
                    split = split[1].Split(" | ");
                    rules.Add(id, new NestedRule
                    {
                        sub = split.Select(x => x.Split(" ")
                            .Select(int.Parse).ToArray()).ToArray()
                    });
                }
            }

            return messages.Count(m => this.match(m, new[] { 0 }.ToList(), rules)).ToString();
        }

        public override string Part2(string input)
        {
            string[] splitByDoubleNewline = input.Split("\n\n");
            string[] ruleStrings = splitByDoubleNewline[0].Split("\n");
            string[] messages = splitByDoubleNewline[1].Split("\n");
            Dictionary<int, Rule> rules = new();
            foreach (string line in ruleStrings)
            {
                string[] split = line.Split(": ");
                int id = int.Parse(split[0]);
                if (split[1].Contains("\""))
                {
                    split = split[1].Split("\"");
                    rules.Add(id, new CharRule { value = split[1][0] });
                }
                else
                {
                    split = split[1].Split(" | ");
                    rules.Add(id, new NestedRule
                    {
                        sub = split.Select(x => x.Split(" ")
                            .Select(int.Parse).ToArray()).ToArray()
                    });
                }
            }

            rules[8] = new NestedRule { sub = new[] { new[] { 42 }, new[] { 42, 8 } } };
            rules[11] = new NestedRule { sub = new[] { new[] { 42, 31 }, new[] { 42, 11, 31 } } };

            return messages.Count(m => this.match(m, new[] { 0 }.ToList(), rules)).ToString();
        }

        private class Rule
        {
        }

        private class CharRule : Rule
        {
            public char value;
        }

        private class NestedRule : Rule
        {
            public int[][] sub;
        }
    }
}