using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2021Day21 : Solution
{
    public override string Part1(string input)
    {
        int p1Pos = input[28] - '0';
        int p2Pos = input[58] - '0';
        
        int diceRoll = 2;
        bool p1Turn = true;

        int p1Score = 0;
        int p2Score = 0;

        int turns = 0;
        
        while (p1Score < 1000 && p2Score < 1000)
        {
            turns++;
            if (p1Turn)
            {
                p1Pos += diceRoll * 3;
                p1Pos %= 10;
                p1Score += p1Pos;
                if (p1Pos == 0)
                    p1Score += 10;
            }
            //same thing but for player 2 in this else
            else
            {
                p2Pos += diceRoll * 3;
                p2Pos %= 10;
                p2Score += p2Pos;
                if (p2Pos == 0)
                    p2Score += 10;
            }

            diceRoll += 3;
            diceRoll %= 10;
            p1Turn = !p1Turn;
        }

        return (Math.Min(p1Score, p2Score) * 3 * turns).ToString();
    }

    public override string Part2(string input)
    {
        int initialP1Pos = input[28] - '0';
        int initialP2Pos = input[58] - '0';
        
        Dictionary<(int, int, int, int), ulong> gamesByState = new Dictionary<(int, int, int, int), ulong>
        {
            [(initialP1Pos, 0, initialP2Pos, 0)] = 1
        };
        uint[] rollChances = {1, 3, 6, 7, 6, 3, 1};

        ulong wins1 = 0;
        ulong wins2 = 0;

        while (gamesByState.Count != 0)
        {
            ((int p1Pos, int p1Score, int p2Pos, int p2Score), ulong universes) = gamesByState.MinBy(kvp => kvp.Key.Item4 + kvp.Key.Item2);
            gamesByState.Remove((p1Pos, p1Score, p2Pos, p2Score));

            for (int i = 0; i < rollChances.Length; i++)
            {
                uint chance1 = rollChances[i];

                int newP1Pos = p1Pos + i + 3;
                newP1Pos = (newP1Pos - 1) % 10 + 1;
                int newP1Score = p1Score + newP1Pos;
                if (newP1Score >= 21)
                    wins1 += universes * chance1;
                else
                {
                    for (int j = 0; j < rollChances.Length; j++)
                    {
                        uint chance2 = rollChances[j] * chance1;
                        
                        int newP2Pos = p2Pos + j + 3;
                        newP2Pos = (newP2Pos - 1) % 10 + 1;
                        int newP2Score = p2Score + newP2Pos;
                        if (newP2Score >= 21)
                            wins2 += universes * chance2;
                        else
                        {
                            (int, int, int, int) newState = (newP1Pos, newP1Score, newP2Pos, newP2Score);
                            if (gamesByState.ContainsKey(newState))
                                gamesByState[newState] += universes * chance2;
                            else
                                gamesByState[newState] = universes * chance2;
                        }
                    }
                }
            }
        }

        return Math.Max(wins1, wins2).ToString();
    }
}