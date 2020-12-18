using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2020Day18 : Solution
    {
        private static long eval(string expr, bool strange_precedence = false)
        {
            List<string> tokens = new List<string>();
            Stack<char> operators = new Stack<char>();
            for (int i = 0; i < expr.Length; i++)
            {
                if (char.IsNumber(expr[i])) //my input, at least, only has single-digit numbers
                    tokens.Add(expr[i].ToString());
                else if (expr[i] == '(')
                    operators.Push(expr[i]);
                else if (expr[i] == '*')
                {
                    while (operators.TryPeek(out char op) && !"()".Contains(op))
                        tokens.Add(operators.Pop().ToString());
                    operators.Push(expr[i]);
                }
                else if (expr[i] == '+')
                {
                    while (operators.TryPeek(out char op) && !(strange_precedence ? "()*" : "()").Contains(op))
                        tokens.Add(operators.Pop().ToString());
                    operators.Push(expr[i]);
                }
                else if (expr[i] == ')')
                {
                    while (operators.Peek() != '(')
                        tokens.Add(operators.Pop().ToString());
                    operators.Pop();
                }
            }

            while (operators.Count > 0)
                tokens.Add(operators.Pop().ToString());

            Stack<long> evalStack = new Stack<long>();
            foreach (string s in tokens)
            {
                if (s.All(c => c >= '0' && c <= '9'))
                    evalStack.Push(long.Parse(s));
                else if (s == "*")
                    evalStack.Push(evalStack.Pop() * evalStack.Pop());
                else if (s == "+")
                    evalStack.Push(evalStack.Pop() + evalStack.Pop());
                else
                    throw new Exception(s);
            }

            return evalStack.Pop();
        }

        public override string Part1(string input)
        {
            input = input.Replace(" ", "");
            long total = 0;
            foreach (string line in input.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                total += eval(line);
            }

            return total.ToString();
        }

        public override string Part2(string input)
        {
            input = input.Replace(" ", "");
            long total = 0;
            foreach (string line in input.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                total += eval(line, true);
            }

            return total.ToString();
        }
    }
}
