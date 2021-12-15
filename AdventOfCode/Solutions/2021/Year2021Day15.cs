using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2021Day15 : Solution
{
    private class KindOfPriorityQueue<TElement> : HashSet<(TElement, int)>
    {
        public (TElement, int) Dequeue()
        {
            (TElement, int) a = this.MinBy(e => e.Item2);
            Remove(a);
            return a;
        }

        public void Enqueue(TElement e, int p)
        {
            Add((e, p));
        }
    }

    private static int GetWeight(string[] lines, int x, int y)
    {
        int bonus = x / 100 + y / 100;

        int raw = lines[y % 100][x % 100] - '0';

        int result = raw + bonus;
        if (result >= 10)
            result = (result % 10) + 1;

        return result;
    }

    public override string Part1(string input)
    {
        // usually this would be a priorityqueue, but that for some reason doesnt allow 1 crucial thing:
        // getting the priority of the dequeued element instead of just the "value"
        KindOfPriorityQueue<(int, int)> paths = new KindOfPriorityQueue<(int, int)>();
        paths.Enqueue((0, 0), 0);

        string[] _ = input.Split('\n');
        int maxX = _[0].Length - 1, maxY = _.Length - 1;

        HashSet<(int, int)> done = new HashSet<(int, int)>();

        while (true)
        {
            ((int x, int y), int priority) = paths.Dequeue();

            if (done.Contains((x, y)))
                continue;

            if (x == maxX && y == maxY)
                return priority.ToString();

            if (x != maxX)
                paths.Enqueue((x + 1, y), priority + GetWeight(_, x + 1, y));
            if (x != 0)
                paths.Enqueue((x - 1, y), priority + GetWeight(_, x - 1, y));
            if (y != maxY)
                paths.Enqueue((x, y + 1), priority + GetWeight(_, x, y + 1));
            if (y != 0)
                paths.Enqueue((x, y - 1), priority + GetWeight(_, x, y - 1));

            done.Add((x, y));
        }
    }


    public override string Part2(string input)
    {
        KindOfPriorityQueue<(int, int)> paths = new KindOfPriorityQueue<(int, int)>();
        paths.Enqueue((0, 0), 0);

        string[] _ = input.Split('\n');
        int maxX = _[0].Length, maxY = _.Length;

        HashSet<(int, int)> done = new HashSet<(int, int)>();

        while (true)
        {
            ((int x, int y), int priority) = paths.Dequeue();

            if (done.Contains((x, y)))
                continue;

            if (x == maxX * 5 - 1 && y == maxY * 5 - 1)
                return priority.ToString();

            if (x != maxX * 5 - 1)
                paths.Enqueue((x + 1, y), priority + GetWeight(_, x + 1, y));
            if (x != 0)
                paths.Enqueue((x - 1, y), priority + GetWeight(_, x - 1, y));
            if (y != maxY * 5 - 1)
                paths.Enqueue((x, y + 1), priority + GetWeight(_, x, y + 1));
            if (y != 0)
                paths.Enqueue((x, y - 1), priority + GetWeight(_, x, y - 1));

            done.Add((x, y));
        }
    }
}