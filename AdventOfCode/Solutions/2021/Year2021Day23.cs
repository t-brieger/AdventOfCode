using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2021Day23 : Solution
{
    private enum FieldState
    {
        A,
        B,
        C,
        D,
        EMPTY
    }

    private class Burrow
    {
        public FieldState[] hallway;
        public Stack<FieldState>[] sides;

        private Burrow Clone()
        {
            Burrow n = new Burrow();
            n.hallway = new FieldState[11];
            for (int i = 0; i < 11; i++)
                n.hallway[i] = this.hallway[i];

            n.sides = new Stack<FieldState>[4];
            for (int i = 0; i < 4; i++)
            {
                List<FieldState> l = this.sides[i].ToList();
                l.Reverse();
                n.sides[i] = new Stack<FieldState>(l);
            }

            return n;
        }


        private static int Cost(FieldState fs)
        {
            // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
            return fs switch
            {
                FieldState.A => 1,
                FieldState.B => 10,
                FieldState.C => 100,
                FieldState.D => 1000,
                _ => throw new ArgumentOutOfRangeException(nameof(fs), fs, null)
            };
        }

        public IEnumerable<(Burrow newstate, int cost)> GetNextStates(bool p2 = false)
        {
            // move out of side corridors
            for (int i = 0; i < 4; i++)
            {
                if (sides[i].Count == 0)
                    continue;
                if (sides[i].All(e => e == (FieldState)i))
                    continue;
                // empty spaces to the left
                for (int j = 2 * i + 2; j >= 0; j--)
                {
                    if (this.hallway[j] != FieldState.EMPTY)
                        break;
                    if (j is 2 or 4 or 6 or 8)
                        continue;
                    Burrow b = Clone();
                    FieldState x = b.sides[i].Pop();
                    b.hallway[j] = x;
                    yield return (b, Cost(x) * ((p2 ? 4 : 2) - b.sides[i].Count + Math.Abs(2 * i + 2 - j)));
                }

                // ...to the right
                for (int j = 2 * i + 2; j < 11; j++)
                {
                    if (this.hallway[j] != FieldState.EMPTY)
                        break;
                    if (j is 2 or 4 or 6 or 8)
                        continue;
                    Burrow b = Clone();
                    FieldState x = b.sides[i].Pop();
                    b.hallway[j] = x;
                    yield return (b, Cost(x) * ((p2 ? 4 : 2) - b.sides[i].Count + Math.Abs(2 * i + 2 - j)));
                }
            }

            // move into rooms
            for (int i = 0; i < 11; i++)
            {
                if (this.hallway[i] == FieldState.EMPTY)
                    continue;
                int sideIndex = (int)this.hallway[i];
                int goalHallwayPos = sideIndex * 2 + 2;
                bool foundConflict = false;
                for (int j = Math.Min(i, goalHallwayPos); j <= Math.Max(i, goalHallwayPos); j++)
                {
                    if (j == i)
                        continue;
                    if (this.hallway[j] != FieldState.EMPTY)
                    {
                        foundConflict = true;
                        break;
                    }
                }

                if (foundConflict) continue;

                // no obstacles to moving into our matching room - check if it is even possible by the rules
                if (this.sides[sideIndex].All(e => e == this.hallway[i]))
                {
                    Burrow b = this.Clone();
                    b.hallway[i] = FieldState.EMPTY;
                    b.sides[sideIndex].Push(this.hallway[i]);

                    yield return (b,
                        Cost(this.hallway[i]) * (Math.Abs(i - (sideIndex * 2 + 2)) + (p2 ? 4 : 2) -
                                                 this.sides[sideIndex].Count));
                }
            }
        }

        public long Hash()
        {
            // hash -> 4 * <side> (12 bits) <hallway> (21 bits)
            //      -> 33 bits (almost an int, oh well)
            // side -> <stack count (2 bits)> <top element matches? (1 bit)> 
            // hallway -> 3 bit enum value for every field not directly above a side corridor (= 21)
            long val = 0;
            for (int i = 0; i < 4; i++)
            {
                val <<= 2;
                val |= (uint)this.sides[i].Count;
                val <<= 1;
                val |= (uint)(this.sides[i].Count != 0 && this.sides[i].Peek() == (FieldState)i ? 1 : 0);
            }

            for (int i = 0; i < 11; i++)
            {
                if (i is 2 or 4 or 6 or 8)
                    continue;
                val <<= 3;
                val |= (long)this.hallway[i];
            }

            return val;
        } 
    }

    public override string Part1(string input)
    {
        Burrow b = new()
        {
            sides = new Stack<FieldState>[4],
            hallway = new FieldState[11]
        };
        for (int i = 0; i < 11; i++)
            b.hallway[i] = FieldState.EMPTY;
        for (int i = 0; i < 4; i++)
            b.sides[i] = new Stack<FieldState>();
        b.sides[0].Push((FieldState)(input[45] - 'A'));
        b.sides[0].Push((FieldState)(input[31] - 'A'));

        b.sides[1].Push((FieldState)(input[47] - 'A'));
        b.sides[1].Push((FieldState)(input[33] - 'A'));

        b.sides[2].Push((FieldState)(input[49] - 'A'));
        b.sides[2].Push((FieldState)(input[35] - 'A'));

        b.sides[3].Push((FieldState)(input[51] - 'A'));
        b.sides[3].Push((FieldState)(input[37] - 'A'));

        PriorityQueue<(Burrow b, int weight), int> queue = new();
        queue.Enqueue((b, 0), 0);
        HashSet<long> seenStates = new HashSet<long>();

        while (true)
        {
            (Burrow state, int weight) = queue.Dequeue();
            if (seenStates.Contains(state.Hash()))
                continue;
            seenStates.Add(state.Hash());

            if (state.hallway.All(x => x == FieldState.EMPTY) && state.sides.All(s => s.Count == 2))
            {
                bool err = false;
                for (int i = 0; i < 4; i++)
                    if (state.sides[i].Any(e => e != (FieldState)i))
                        err = true;
                if (!err)
                    return weight.ToString();
            }

            foreach ((Burrow next, int cost) in state.GetNextStates())
                queue.Enqueue((next, weight + cost), weight + cost);
        }
    }

    public override string Part2(string input)
    {
        Burrow b = new()
        {
            sides = new Stack<FieldState>[4],
            hallway = new FieldState[11]
        };
        for (int i = 0; i < 11; i++)
            b.hallway[i] = FieldState.EMPTY;
        for (int i = 0; i < 4; i++)
            b.sides[i] = new Stack<FieldState>();
        b.sides[0].Push((FieldState)(input[45] - 'A'));
        b.sides[0].Push(FieldState.D);
        b.sides[0].Push(FieldState.D);
        b.sides[0].Push((FieldState)(input[31] - 'A'));

        b.sides[1].Push((FieldState)(input[47] - 'A'));
        b.sides[1].Push(FieldState.B);
        b.sides[1].Push(FieldState.C);
        b.sides[1].Push((FieldState)(input[33] - 'A'));

        b.sides[2].Push((FieldState)(input[49] - 'A'));
        b.sides[2].Push(FieldState.A);
        b.sides[2].Push(FieldState.B);
        b.sides[2].Push((FieldState)(input[35] - 'A'));

        b.sides[3].Push((FieldState)(input[51] - 'A'));
        b.sides[3].Push(FieldState.C);
        b.sides[3].Push(FieldState.A);
        b.sides[3].Push((FieldState)(input[37] - 'A'));

        PriorityQueue<(Burrow b, int weight), int> queue = new();
        queue.Enqueue((b, 0), 0);
        HashSet<long> seenStates = new HashSet<long>();

        while (true)
        {
            (Burrow state, int weight) = queue.Dequeue();
            if (seenStates.Contains(state.Hash()))
                continue;
            seenStates.Add(state.Hash());

            if (state.hallway.All(x => x == FieldState.EMPTY) && state.sides.All(s => s.Count == 4))
            {
                bool err = false;
                for (int i = 0; i < 4; i++)
                    if (state.sides[i].Any(e => e != (FieldState)i))
                        err = true;
                if (!err)
                    return weight.ToString();
            }

            foreach ((Burrow next, int cost) in state.GetNextStates(true))
                queue.Enqueue((next, weight + cost), weight + cost);
        }
    }
}