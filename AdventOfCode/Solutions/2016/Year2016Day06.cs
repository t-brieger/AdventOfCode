using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2016Day06 : Solution
{
    public override string Part1(string input)
    {
        string[] lines = input.Split('\n');

        string s = "";
        
        for (int i = 0; i < lines[0].Length; i++)
        {
            Dictionary<char, int> counts = new Dictionary<char, int>();
            for (char c = 'a'; c <= 'z'; c++)
            {
                counts[c] = lines.Count(x => x[i] == c);
            }

            s += counts.MaxBy(kvp => kvp.Value).Key;
        }

        return s;
    }

    public override string Part2(string input)
    {
        string[] lines = input.Split('\n');

        string s = "";
        
        for (int i = 0; i < lines[0].Length; i++)
        {
            Dictionary<char, int> counts = new Dictionary<char, int>();
            for (char c = 'a'; c <= 'z'; c++)
            {
                int count = lines.Count(x => x[i] == c);
                if (count != 0)
                    counts[c] = count;
            }

            s += counts.MinBy(kvp => kvp.Value).Key;
        }

        return s;
    }
}
