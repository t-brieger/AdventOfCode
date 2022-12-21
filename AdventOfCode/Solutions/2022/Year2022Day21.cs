using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions;

public class Year2022Day21 : Solution
{
    private class Monkey
    {
        private Func<Dictionary<string, Monkey>, long> getResult;
        private readonly Func<Dictionary<string, Monkey>, long, long> findNeededReplacement;
        private readonly string id;

        public Monkey(long x, string id = null)
        {
            this.id = id;
            if (id == "humn")
            {
                getResult = _ => long.MaxValue;
                findNeededReplacement = (d, wanted) => wanted;
            }
            else
            {
                getResult = _ => x;
                findNeededReplacement = (_, _) => throw new Exception();
            }
        }

        public Monkey(char op, string op1, string op2, string id = null)
        {
            this.id = id;

            getResult = dict =>
            {
                long o1 = dict[op1].getResult(dict);
                long o2 = dict[op2].getResult(dict);
                if (o1 == long.MaxValue || o2 == long.MaxValue)
                    return long.MaxValue;
                long result = op switch
                {
                    '+' => o1 + o2,
                    '-' => o1 - o2,
                    '*' => o1 * o2,
                    '/' => o1 / o2,
                    _ => throw new ArgumentOutOfRangeException($"unrecognized operator {op}")
                };
                getResult = _ => result;
                return result;
            };
            findNeededReplacement = (dict, wanted) =>
            {
                long o1 = dict[op1].getResult(dict);
                long o2 = dict[op2].getResult(dict);
                if (id == "root")
                    return dict[o1 == long.MaxValue ? op1 : op2].findNeededReplacement(dict, Math.Min(o1, o2));
                if (o1 == long.MaxValue)
                    return dict[op1].findNeededReplacement(dict, op switch
                    {
                        '+' => wanted - o2,
                        '-' => wanted + o2,
                        '*' => wanted / o2,
                        '/' => wanted * o2,
                        _ => throw new ArgumentOutOfRangeException($"unrecognized operator {op}")
                    });
                return dict[op2].findNeededReplacement(dict, op switch
                {
                    '+' => wanted - o1,
                    '-' => o1 - wanted,
                    '*' => wanted / o1,
                    '/' => o1 / wanted,
                    _ => throw new ArgumentOutOfRangeException($"unrecognized operator {op}")
                });
            };
        }

        public long GetValue(Dictionary<string, Monkey> d) => getResult(d);

        public long FindReplacement(Dictionary<string, Monkey> d)
        {
            if (id != "root")
                throw new ArgumentException();
            return findNeededReplacement(d, 0);
        }
    }

    public override string Part1(string input)
    {
        Dictionary<string, Monkey> monkeys = new Dictionary<string, Monkey>();
        foreach (string line in input.Split('\n'))
        {
            string[] split = line.Split(new[] {':', ' '}, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length == 2)
                monkeys.Add(split[0], new Monkey(long.Parse(split[1])));
            else
                monkeys.Add(split[0], new Monkey(split[2][0], split[1], split[3]));
        }

        return monkeys["root"].GetValue(monkeys).ToString();
    }

    public override string Part2(string input)
    {
        Dictionary<string, Monkey> monkeys = new Dictionary<string, Monkey>();
        foreach (string line in input.Split('\n'))
        {
            string[] split = line.Split(new[] {':', ' '}, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length == 2)
                monkeys.Add(split[0], new Monkey(long.Parse(split[1]), split[0]));
            else
                monkeys.Add(split[0], new Monkey(split[2][0], split[1], split[3], split[0]));
        }

        return monkeys["root"].FindReplacement(monkeys).ToString();
    }
}
