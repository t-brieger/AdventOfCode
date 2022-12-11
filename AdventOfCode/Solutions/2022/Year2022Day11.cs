using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day11 : Solution
{
    private static (List<long> items, Func<long, long> operation, Func<long, bool> test, int ifTrue, int ifFalse)[]
        ParseMonkeys(string[] monkeyStrings, out long mod)
    {
        (List<long> items, Func<long, long> operation, Func<long, bool> test, int ifTrue, int ifFalse)[]
            monkeys = new (List<long>, Func<long, long>, Func<long, bool>, int, int)[monkeyStrings.Length];

        mod = 1;
        
        for (int i = 0; i < monkeyStrings.Length; i++)
        {
            string monk = monkeyStrings[i];
            string[] lines = monk.Split('\n').Select(l => l.Trim()).ToArray();
            // int id = int.Parse(lines[0].Replace(":", "").Split(' ')[1]);

            List<long> items = new List<long>();
            foreach (string item in lines[1].Split(": ")[1].Split(", "))
                items.Add(long.Parse(item));

            // just assume there is always only one operator in "Operation" and the first operand is always "old"
            lines[2] = lines[2]["Operation: new = old ".Length..];
            Func<long, long> operation = old =>
                lines[2].Contains("old") ? lines[2][0] == '+' ? old + old : old * old :
                lines[2][0] == '+' ? old + long.Parse(lines[2].Split(' ')[1]) :
                old * long.Parse(lines[2].Split(' ')[1]);

            Func<long, bool> test = worry => worry % long.Parse(lines[3].Split(' ')[3]) == 0;
            mod = Util.Lcm(mod, long.Parse(lines[3].Split(' ')[3]));
            
            int trueMonkey = int.Parse(lines[4].Split(' ')[5]);
            int falseMonkey = int.Parse(lines[5].Split(' ')[5]);

            monkeys[i] = (items, operation, test, trueMonkey, falseMonkey);
        }

        return monkeys;
    }

    public override string Part1(string input)
    {
        string[] rawMonkeys = input.Split("\n\n");
        (List<long> items, Func<long, long> operation, Func<long, bool> test, int ifTrue, int ifFalse)[] monkeys = ParseMonkeys(rawMonkeys, out long _);

        int[] inspectCount = new int[monkeys.Length];
        for (int i = 0; i < inspectCount.Length; i++)
            inspectCount[i] = 0;

        for (int i = 0; i < 20; i++)
        {
            for (int m = 0; m < monkeys.Length; m++)
            {
                while (monkeys[m].items.Count != 0)
                {
                    inspectCount[m]++;

                    long item = monkeys[m].items[0];
                    monkeys[m].items.RemoveAt(0);

                    long newItem = monkeys[m].operation(item);
                    newItem /= 3;
                    monkeys[monkeys[m].test(newItem) ? monkeys[m].ifTrue : monkeys[m].ifFalse].items.Add(newItem);
                }
            }
        }

        return inspectCount.OrderByDescending(i => i).Take(2).Aggregate(1, (a, i) => a * i).ToString();
    }

    public override string Part2(string input)
    {
        string[] rawMonkeys = input.Split("\n\n");
        (List<long> items, Func<long, long> operation, Func<long, bool> test, int ifTrue, int ifFalse)[] monkeys = ParseMonkeys(rawMonkeys, out long mod);

        int[] inspectCount = new int[monkeys.Length];
        for (int i = 0; i < inspectCount.Length; i++)
            inspectCount[i] = 0;

        for (int i = 0; i < 10000; i++)
        {
            for (int m = 0; m < monkeys.Length; m++)
            {
                while (monkeys[m].items.Count != 0)
                {
                    inspectCount[m]++;

                    long item = monkeys[m].items[0];
                    monkeys[m].items.RemoveAt(0);

                    long newItem = monkeys[m].operation(item);
                    newItem %= mod;
                    monkeys[monkeys[m].test(newItem) ? monkeys[m].ifTrue : monkeys[m].ifFalse].items.Add(newItem);
                }
            }
        }

        return inspectCount.OrderByDescending(i => i).Take(2).Aggregate(1L, (a, i) => a * i).ToString();
    }
}
