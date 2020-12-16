using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2017Day08 : Solution
    {
        public override string Part1(string input)
        {
            Dictionary<string, int> registers = new Dictionary<string, int>();

            foreach (string[] words in input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(' ')))
            {
                if (!registers.ContainsKey(words[0]))
                    registers.Add(words[0], 0);
                if (!registers.ContainsKey(words[4]))
                    registers.Add(words[4], 0);

                int comparisonOperand = Int32.Parse(words[6]);

                switch (words[5])
                {
                    case ">":
                        if (registers[words[4]] <= comparisonOperand)
                            continue;
                        break;
                    case "<":
                        if (registers[words[4]] >= comparisonOperand)
                            continue;
                        break;
                    case ">=":
                        if (registers[words[4]] < comparisonOperand)
                            continue;
                        break;
                    case "<=":
                        if (registers[words[4]] > comparisonOperand)
                            continue;
                        break;
                    case "==":
                        if (registers[words[4]] != comparisonOperand)
                            continue;
                        break;
                    case "!=":
                        if (registers[words[4]] == comparisonOperand)
                            continue;
                        break;
                }

                int otherOperand = Int32.Parse(words[2]);

                if (words[1] == "inc")
                    registers[words[0]] += otherOperand;
                else if (words[1] == "dec")
                    registers[words[0]] -= otherOperand;
                else
                    Console.WriteLine($"invalid: {words[1]} (allowed: \"inc\"/\"dec\")");
            }

            return registers.OrderBy(kvp => kvp.Value).Last().Value.ToString();
        }

        public override string Part2(string input)
        {
            Dictionary<string, int> registers = new Dictionary<string, int>();

            int max = 0;

            foreach (string[] words in input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(' ')))
            {
                if (!registers.ContainsKey(words[0]))
                    registers.Add(words[0], 0);
                if (!registers.ContainsKey(words[4]))
                    registers.Add(words[4], 0);

                int comparisonOperand = Int32.Parse(words[6]);

                switch (words[5])
                {
                    case ">":
                        if (registers[words[4]] <= comparisonOperand)
                            continue;
                        break;
                    case "<":
                        if (registers[words[4]] >= comparisonOperand)
                            continue;
                        break;
                    case ">=":
                        if (registers[words[4]] < comparisonOperand)
                            continue;
                        break;
                    case "<=":
                        if (registers[words[4]] > comparisonOperand)
                            continue;
                        break;
                    case "==":
                        if (registers[words[4]] != comparisonOperand)
                            continue;
                        break;
                    case "!=":
                        if (registers[words[4]] == comparisonOperand)
                            continue;
                        break;
                }

                int otherOperand = Int32.Parse(words[2]);

                if (words[1] == "inc")
                    registers[words[0]] += otherOperand;
                else if (words[1] == "dec")
                    registers[words[0]] -= otherOperand;
                else
                    Console.WriteLine($"invalid: {words[1]} (allowed: \"inc\"/\"dec\")");
                if (registers[words[0]] > max)
                    max = registers[words[0]];
            }

            return max.ToString();
        }
    }
}
