using System;
using AdventOfCode.Solutions.IntCode;

namespace AdventOfCode.Solutions;

public class Year2019Day21 : Solution
{
    public override string Part1(string input)
    {
        Computer c = new(input);
        // are there bonus points for not using T?
        c.EnqueueInput("NOT J J\nAND A J\nAND B J\nAND C J\nNOT J J\nAND D J\nWALK\n");
        c.RunUntilHalted();
        while (c.output.Count > 0)
        {
            if (c.output.Peek() <= char.MaxValue)
            {
                c.output.Dequeue();
                // Console.Write((char)c.output.Dequeue());
            }
            else
            {
                return c.output.Dequeue().ToString();
            }
        }
        return null;
    }

    public override string Part2(string input)
    {
        Computer c = new(input);
        // I'm not convinced this is a general solution, but it works for my input.
        c.EnqueueInput("NOT J J\nAND A J\nAND B J\nAND C J\nNOT J J\nAND D J\nOR H T\nOR E T\nAND T J\nRUN\n");
        c.RunUntilHalted();
        while (c.output.Count > 0)
        {
            if (c.output.Peek() <= char.MaxValue)
            {
                c.output.Dequeue();
                //Console.Write((char)c.output.Dequeue());
            }
            else
            {
                return c.output.Dequeue().ToString();
            }
        }
        return null;
    }
}