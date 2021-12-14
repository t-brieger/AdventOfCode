using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2015Day11 : Solution
{
    private static void IncreaseString(IList<char> str)
    {
        bool carry = false;
        for (int i = str.Count - 1; i >= 0; i--)
        {
            if (i != str.Count - 1 && !carry) continue;
            str[i]++;
            str[i] = str[i] == 'z' + 1 ? 'a' : str[i];
            carry = str[i] == 'a';
        }
    }

    private static bool ValidatePass(IReadOnlyList<char> s)
    {
        bool meetsCond1 = false;
        for (int i = 0; i < s.Count - 2; i++)
            if (s[i] == s[i + 1] - 1 && s[i] == s[i + 2] - 2)
                meetsCond1 = true;

        if (!meetsCond1)
            return false;

        if (s.Any(c => c is 'i' or 'o' or 'l'))
            return false;

        int pairs = 0;
        for (int i = 0; i < s.Count - 1; i++)
        {
            if (s[i] != s[i + 1]) continue;
            i++;
            pairs++;
        }

        return pairs >= 2;
    }

    public override string Part1(string input)
    {
        char[] pass = input.Split("\n")[0].ToCharArray();
        while (!ValidatePass(pass))
            IncreaseString(pass);
        return new string(pass);
    }

    public override string Part2(string input)
    {
        char[] pass = this.Part1(input).ToCharArray();
        IncreaseString(pass);
        while (!ValidatePass(pass))
            IncreaseString(pass);
        return new string(pass);
    }
}