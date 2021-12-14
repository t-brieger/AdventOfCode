using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2021Day04 : Solution
{
    private static bool IsBingo(int[][] board, int row, int col)
    {
        return board[row].All(i => i < 0) || board.All(r => r[col] < 0);
    }

    private static bool FlipNumberOnCard(int[][] card, int n)
    {
        for (int row = 0; row < card.Length; row++)
        {
            for (int col = 0; col < card[row].Length; col++)
            {
                if (card[row][col] != n) continue;
                card[row][col] *= -1;
                return IsBingo(card, row, col);
            }
        }

        return false;
    }

    public override string Part1(string input)
    {
        string[] parts = input.Split("\n\n");

        int[] drawnNumbers = parts[0].Split(',').Select(int.Parse).ToArray();
        int[][][] bingoCards = parts.Skip(1).Select(c =>
            c.Split('\n')
                .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
                .ToArray()).ToArray();

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (int n in drawnNumbers)
        {
            foreach (int[][] card in bingoCards.Where(card => FlipNumberOnCard(card, n)))
            {
                return (card.Sum(row => row.Sum(x => x <= 0 ? 0 : x)) * n).ToString();
            }
        }

        return null;
    }

    public override string Part2(string input)
    {
        string[] parts = input.Split("\n\n");

        int[] drawnNumbers = parts[0].Split(',').Select(int.Parse).ToArray();
        HashSet<int[][]> bingoCards = parts.Skip(1).Select(c =>
            c.Split('\n')
                .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
                .ToArray()).ToHashSet();

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (int n in drawnNumbers)
        {
            List<int[][]> won = new();
            foreach (int[][] card in bingoCards.Where(card => FlipNumberOnCard(card, n)))
            {
                if (bingoCards.Count > 1)
                    won.Add(card);
                else
                    return (card.Sum(row => row.Sum(x => x <= 0 ? 0 : x)) * n).ToString();
            }

            foreach (int[][] wonCard in won)
                bingoCards.Remove(wonCard);
        }

        return null;
    }
}