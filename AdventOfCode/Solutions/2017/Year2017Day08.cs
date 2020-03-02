using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2017
{
    public class Year2017Day08 : Solution
    {
        public override string Part1(string input)
        {
            Dictionary<string, int> registers = new Dictionary<string, int>();

            foreach (string s in input.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries))
            {
                // 0 - register
                // 1 - inc/dec
                // 2 - operand
                // 3 - "if"
                // 4 - register
                // 5 - comparison operator
                // 6 - operand
                string[] words = s.Split(' ');
                if (!registers.ContainsKey(words[0]))
                    registers.Add(words[0], 0);
                if (!registers.ContainsKey(words[4]))
                    registers.Add(words[4], 0);

                int comparisonOperand = int.Parse(words[6]);

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

                int otherOperand = int.Parse(words[2]);

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

            foreach (string s in input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                // 0 - register
                // 1 - inc/dec
                // 2 - operand
                // 3 - "if"
                // 4 - register
                // 5 - comparison operator
                // 6 - operand
                string[] words = s.Split(' ');
                if (!registers.ContainsKey(words[0]))
                    registers.Add(words[0], 0);
                if (!registers.ContainsKey(words[4]))
                    registers.Add(words[4], 0);

                int comparisonOperand = int.Parse(words[6]);

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

                int otherOperand = int.Parse(words[2]);

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