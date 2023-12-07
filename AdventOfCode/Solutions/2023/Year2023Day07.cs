using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2023Day07 : Solution
{
    private enum HandTypes
    {
        HIGH_CARD,
        ONE_PAIR,
        TWO_PAIR,
        THREE_KIND,
        FULL_HOUSE,
        FOUR_KIND,
        FIVE_KIND
    }

    private static HandTypes GetHandType(string hand, bool p2)
    {
        if (p2 && hand.Contains('J'))
        {
            int i = hand.IndexOf('J');
            HandTypes best = HandTypes.HIGH_CARD;
            foreach (char c in "23456789TQKA")
            {
                string copy = hand[..i] + c + hand[(i + 1)..];
                HandTypes type = GetHandType(copy, p2);
                if (type > best)
                    best = type;
            }

            return best;
        }
        
        Dictionary<char, int> counts = new();
        foreach (char c in hand)
            counts[c] = counts.ContainsKey(c) ? counts[c] + 1 : 1;

        if (counts.Any(kvp => kvp.Value == 5))
            return HandTypes.FIVE_KIND;
        if (counts.Any(kvp => kvp.Value == 4))
            return HandTypes.FOUR_KIND;
        if (counts.Any(kvp => kvp.Value == 3) && counts.Any(kvp => kvp.Value == 2))
            return HandTypes.FULL_HOUSE;
        if (counts.Any(kvp => kvp.Value == 3))
            return HandTypes.THREE_KIND;
        if (counts.Count(kvp => kvp.Value == 2) == 2)
            return HandTypes.TWO_PAIR;
        if (counts.Any(kvp => kvp.Value == 2))
            return HandTypes.ONE_PAIR;
        return HandTypes.HIGH_CARD;
    }

    private int GetHandScore(string hand, bool p2 = false)
    {
        int result = (int) GetHandType(hand, p2);
        for (int i = 0; i < 5; i++)
        {
            result *= 13;
            result += (p2 ? "J23456789TQKA" : "23456789TJQKA").IndexOf(hand[i]);
        }

        return result;
    }

    public override string Part1(string input)
    {
        (int score, int bid)[] hands = input.Split('\n')
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(arr => (GetHandScore(arr[0]), int.Parse(arr[1]))).ToArray();

        hands = hands.OrderBy(hand => hand.score).ToArray();
        long sum = 0;
        for (int i = 0; i < hands.Length; i++)
            sum += hands[i].bid * (i + 1);
        return sum.ToString();
    }

    public override string Part2(string input)
    {
        (int score, int bid)[] hands = input.Split('\n')
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(arr => (GetHandScore(arr[0], true), int.Parse(arr[1]))).ToArray();

        hands = hands.OrderBy(hand => hand.score).ToArray();
        long sum = 0;
        for (int i = 0; i < hands.Length; i++)
            sum += hands[i].bid * (i + 1);
        return sum.ToString();
    }
}
