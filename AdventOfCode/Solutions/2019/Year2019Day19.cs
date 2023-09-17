using System;
using AdventOfCode.Solutions.IntCode;

namespace AdventOfCode.Solutions;

public class Year2019Day19 : Solution
{
    public override string Part1(string input)
    {
        int counter = 0;
        for (int x = 0; x < 50; x++)
        {
            for (int y = 0; y < 50; y++)
            {
                Computer c = new(input);
                c.EnqueueInput(x);
                c.EnqueueInput(y);
                c.RunUntilHalted();
                if (c.output.Dequeue() == 1)
                    counter++;
            }
        }

        return counter.ToString();
    }

    public override string Part2(string input)
    {
        for (int y = 99;; y++)
        {
            for (int x = 0;; x++)
            {
                Computer c = new(input);
                c.EnqueueInput(x);
                c.EnqueueInput(y);
                c.RunUntilHalted();
                if (c.output.Dequeue() == 1)
                {
                    c = new(input);
                    c.EnqueueInput(x + 99);
                    c.EnqueueInput(y - 99);
                    c.RunUntilHalted();
                    if (c.output.Dequeue() == 1)
                        return (x * 10_000 + (y - 99)).ToString();
                    break;
                }
            }
        }
    }
}