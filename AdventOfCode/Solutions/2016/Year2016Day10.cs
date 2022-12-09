using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2016Day10 : Solution
{
    public override string Part1(string input)
    {
        List<string[]> instructions = input.Split('\n').Select(l => l.Split(' ')).ToList();

        // very safe estimate, there can never be more bots than lines in the input, and there are usually less.
        (int a, int b)[] bots = new (int, int)[instructions.Count];
        for (int i = 0; i < bots.Length; i++)
            bots[i] = (-1, -1);

        while (instructions.Count > 0)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                string[] instruction = instructions[i];

                if (instruction[0] == "value")
                {
                    // "value X goes to bot Y"
                    int val = int.Parse(instruction[1]);
                    int botNum = int.Parse(instruction[5]);

                    if (bots[botNum].a == -1)
                        bots[botNum] = (val, -1);
                    else
                        bots[botNum] = (bots[botNum].a, val);
                }
                else
                {
                    // bot X gives low to bot/output Y and high to bot/output Z

                    // we ignore outputs in part 1, since they don't matter to us.
                    int botNum = int.Parse(instruction[1]);

                    if (bots[botNum].a == -1 || bots[botNum].b == -1)
                        // can't compare yet, only 1 (or no) value
                        continue;

                    int hi = Math.Max(bots[botNum].a, bots[botNum].b);
                    int lo = Math.Min(bots[botNum].a, bots[botNum].b);

                    if (hi == 61 && lo == 17)
                        return botNum.ToString();

                    if (instruction[5] == "bot")
                    {
                        int loBotNum = int.Parse(instruction[6]);

                        if (bots[loBotNum].a == -1)
                            bots[loBotNum].a = lo;
                        else
                            bots[loBotNum].b = lo;
                    }

                    if (instruction[10] == "bot")
                    {
                        int hiBotNum = int.Parse(instruction[11]);

                        if (bots[hiBotNum].a == -1)
                            bots[hiBotNum].a = hi;
                        else
                            bots[hiBotNum].b = hi;
                    }
                }

                instructions.RemoveAt(i--);
                // keep iterating - this is all out of order anyway
            }
        }

        return null;
    }

    public override string Part2(string input)
    {
        List<string[]> instructions = input.Split('\n').Select(l => l.Split(' ')).ToList();

        (int a, int b)[] bots = new (int, int)[instructions.Count];
        int[] outputs = new int[instructions.Count];
        for (int i = 0; i < bots.Length; i++)
        {
            bots[i] = (-1, -1);
            outputs[i] = 0;
        }

        while (instructions.Count > 0)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                string[] instruction = instructions[i];

                if (instruction[0] == "value")
                {
                    // "value X goes to bot Y"
                    int val = int.Parse(instruction[1]);
                    int botNum = int.Parse(instruction[5]);

                    if (bots[botNum].a == -1)
                        bots[botNum] = (val, -1);
                    else
                        bots[botNum] = (bots[botNum].a, val);
                }
                else
                {
                    // bot X gives low to bot/output Y and high to bot/output Z

                    // we ignore outputs in part 1, since they don't matter to us.
                    int botNum = int.Parse(instruction[1]);

                    if (bots[botNum].a == -1 || bots[botNum].b == -1)
                        // can't compare yet, only 1 (or no) value
                        continue;

                    int hi = Math.Max(bots[botNum].a, bots[botNum].b);
                    int hiNum = int.Parse(instruction[11]);
                    int lo = Math.Min(bots[botNum].a, bots[botNum].b);
                    int loNum = int.Parse(instruction[6]);

                    if (instruction[5] == "bot")
                    {
                        if (bots[loNum].a == -1)
                            bots[loNum].a = lo;
                        else
                            bots[loNum].b = lo;
                    }
                    else
                    {
                        outputs[loNum] = lo;
                    }

                    if (instruction[10] == "bot")
                    {
                        if (bots[hiNum].a == -1)
                            bots[hiNum].a = hi;
                        else
                            bots[hiNum].b = hi;
                    }
                    else
                    {
                        outputs[hiNum] = hi;
                    }
                }

                instructions.RemoveAt(i--);
                // keep iterating - this is all out of order anyway
            }
        }

        return (outputs[0] * outputs[1] * outputs[2]).ToString();
    }
}
