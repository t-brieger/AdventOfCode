using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2020Day18 : Solution
{
    private static long Eval(string expr, bool strangePrecedence = false)
    {
        List<string> tokens = new();
        Stack<char> operators = new();
        foreach (char t in expr)
            if (char.IsNumber(t)) //my input, at least, only has single-digit numbers
                tokens.Add(t.ToString());
            else
                switch (t)
                {
                    case '(':
                        operators.Push(t);
                        break;
                    case '*':
                    {
                        while (operators.TryPeek(out char op) && !"()".Contains(op))
                            tokens.Add(operators.Pop().ToString());
                        operators.Push(t);
                        break;
                    }
                    case '+':
                    {
                        while (operators.TryPeek(out char op) && !(strangePrecedence ? "()*" : "()").Contains(op))
                            tokens.Add(operators.Pop().ToString());
                        operators.Push(t);
                        break;
                    }
                    case ')':
                    {
                        while (operators.Peek() != '(')
                            tokens.Add(operators.Pop().ToString());
                        operators.Pop();
                        break;
                    }
                }

        while (operators.Count > 0)
            tokens.Add(operators.Pop().ToString());

        Stack<long> evalStack = new();
        foreach (string s in tokens)
            if (s.All(c => c is >= '0' and <= '9'))
                evalStack.Push(long.Parse(s));
            else
                switch (s)
                {
                    case "*":
                        evalStack.Push(evalStack.Pop() * evalStack.Pop());
                        break;
                    case "+":
                        evalStack.Push(evalStack.Pop() + evalStack.Pop());
                        break;
                    default:
                        throw new Exception(s);
                }

        return evalStack.Pop();
    }

    public override string Part1(string input)
    {
        input = input.Replace(" ", "");
        long total = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Sum(line => Eval(line));

        return total.ToString();
    }

    public override string Part2(string input)
    {
        input = input.Replace(" ", "");
        long total = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Sum(line => Eval(line, true));

        return total.ToString();
    }
}