using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2020Day22 : Solution
{
    public override string Part1(string input)
    {
        string[] parts = input.Split("\n\n");

        Queue<int>[] Players = new Queue<int>[2];
        Players[0] = new Queue<int>();
        Players[1] = new Queue<int>();

        for (int i = 0; i < 2; i++)
            foreach (string card in parts[i].Split('\n').Skip(1))
                Players[i].Enqueue(int.Parse(card));

        while (Players[0].Count > 0 && Players[1].Count > 0)
        {
            if (Players[0].Peek() > Players[1].Peek())
            {
                Players[0].Enqueue(Players[0].Dequeue());
                Players[0].Enqueue(Players[1].Dequeue());
            }
            else
            {
                Players[1].Enqueue(Players[1].Dequeue());
                Players[1].Enqueue(Players[0].Dequeue());
            }
        }

        long score = 0;
        int winner = Players.FirstIndexOf(q => q.Count > 0);

        while (Players[winner].Count > 0)
        {
            score += Players[winner].Peek() * Players[winner].Count;
            Players[winner].Dequeue();
        }

        return score.ToString();
    }

    public override string Part2(string input)
    {
        // TODO
        return null;
    }
}
