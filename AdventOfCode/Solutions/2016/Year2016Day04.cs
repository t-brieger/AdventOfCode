using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2016Day04 : Solution
{
    public override string Part1(string input)
    {
        (string, int, string)[] rooms = input.Split('\n')
            .Select(r => r.Split(new[] { '-', '[', ']' }, StringSplitOptions.RemoveEmptyEntries)).Select(parts =>
                (String.Join("", parts[..^2]), int.Parse(parts[^2]), parts[^1])).ToArray();
        int sum = 0;

        foreach ((string name, int sid, string check) in rooms)
        {
            Dictionary<char, int> numOccurences = new();

            foreach (char c in name)
            {
                if (numOccurences.ContainsKey(c))
                    numOccurences[c]++;
                else
                    numOccurences.Add(c, 1);
            }

            string correctCheck =
                String.Join("",
                    numOccurences.Keys.OrderByDescending(c => numOccurences[c] * 1000 + ('z' - c)).Take(5));
            if (check == correctCheck)
                sum += sid;
        }

        return sum.ToString();
    }

    public override string Part2(string input)
    {
        (string, int, string)[] rooms = input.Split('\n')
            .Select(r => r.Split(new[] { '-', '[', ']' }, StringSplitOptions.RemoveEmptyEntries)).Select(parts =>
                (String.Join("", parts[..^2]), int.Parse(parts[^2]), parts[^1])).ToArray();

        foreach ((string name, int sid, string check) in rooms)
        {
            Dictionary<char, int> numOccurences = new();

            foreach (char c in name)
            {
                if (numOccurences.ContainsKey(c))
                    numOccurences[c]++;
                else
                    numOccurences.Add(c, 1);
            }

            string correctCheck =
                String.Join("",
                    numOccurences.Keys.OrderByDescending(c => numOccurences[c] * 1000 + ('z' - c)).Take(5));
            if (check == correctCheck)
            {
                string decrypted = String.Join("",
                    name.Select(c => c - 'a').Select(i => (i + sid) % 26).Select(i => (char)(i + 'a')));
                if (decrypted.Contains("north"))
                    return (sid, decrypted).ToString();
            }
        }

        return null;
    }
}