using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions;

public class Year2018Day02 : Solution
{
    public override string Part1(string s)
    {
        string[] inputs = s.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        int numOfTwoElements = 0;
        int numOfThreeElements = 0;
        foreach (string line in inputs)
        {
            Dictionary<char, int> letters = new();
            foreach (char c in line)
                if (letters.ContainsKey(c))
                    letters[c]++;
                else
                    letters.Add(c, 1);

            numOfThreeElements = letters.ContainsValue(3) ? numOfThreeElements + 1 : numOfThreeElements;

            numOfTwoElements = letters.ContainsValue(2) ? numOfTwoElements + 1 : numOfTwoElements;
        }

        return (numOfThreeElements * numOfTwoElements).ToString();
    }

    public override string Part2(string s)
    {
        string[] inputs = s.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        foreach (string s1 in inputs)
        foreach (string s2 in inputs)
        {
            if (s1 == s2) continue;

            bool onlyonediff = true;
            bool canExit = false;
            for (int i = 0; i < s1.Length; i++)
                if (s1[i] != s2[i] && onlyonediff)
                {
                    onlyonediff = false;
                }
                else if (s1[i] != s2[i])
                {
                    canExit = true;
                    break;
                }

            if (onlyonediff) //shouldn't happen, but just in case it was the same string
                continue;
            if (canExit)
                continue;
            StringBuilder res = new();
            for (int i = 0; i < s1.Length; i++)
                if (s1[i] == s2[i])
                    res.Append(s1[i]);
            return res.ToString();
        }

        return "";
    }
}