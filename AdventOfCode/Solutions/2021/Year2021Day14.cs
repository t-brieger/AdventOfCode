using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2021Day14 : Solution
{
    private static Dictionary<(char, char), long> Polymerize(Dictionary<(char, char), char> rules, int steps,
        string start)
    {
        Dictionary<(char, char), long> adjCount = new Dictionary<(char, char), long>();
        for (int i = 0; i < start.Length - 1; i++)
        {
            if (adjCount.ContainsKey((start[i], start[i + 1])))
                adjCount[(start[i], start[i + 1])]++;
            else
                adjCount[(start[i], start[i + 1])] = 1;
        }


        for (int i = 0; i < steps; i++)
        {
            Dictionary<(char, char), long> newAdjCount = new Dictionary<(char, char), long>(adjCount.Count * 2 - 1);

            foreach (KeyValuePair<(char, char), long> kvp in adjCount)
            {
                char cleft = kvp.Key.Item1;
                char cright = kvp.Key.Item2;

                char inserted = rules[(cleft, cright)];

                if (newAdjCount.ContainsKey((cleft, inserted)))
                    newAdjCount[(cleft, inserted)] += kvp.Value;
                else
                    newAdjCount[(cleft, inserted)] = kvp.Value;

                if (newAdjCount.ContainsKey((inserted, cright)))
                    newAdjCount[(inserted, cright)] += kvp.Value;
                else
                    newAdjCount[(inserted, cright)] = kvp.Value;
            }

            adjCount = newAdjCount;
        }

        return adjCount;
    }

    public override string Part1(string input)
    {
        string[] parts = input.Split("\n\n");
        string template = parts[0];

        Dictionary<(char, char), char> rules = new Dictionary<(char, char), char>();
        foreach (string line in parts[1].Split('\n'))
        {
            string[] splitLine = line.Split(" -> ");
            rules.Add((splitLine[0][0], splitLine[0][1]), splitLine[1][0]);
        }

        Dictionary<(char, char), long> result = Polymerize(rules, 10, template);
        Dictionary<char, long> frequencies = new Dictionary<char, long>();
        foreach (KeyValuePair<(char, char), long> kvp in result)
        {
            if (frequencies.ContainsKey(kvp.Key.Item1))
                frequencies[kvp.Key.Item1] += kvp.Value;
            else
                frequencies[kvp.Key.Item1] = kvp.Value;

            if (frequencies.ContainsKey(kvp.Key.Item2))
                frequencies[kvp.Key.Item2] += kvp.Value;
            else
                frequencies[kvp.Key.Item2] = kvp.Value;
        }

        // every char gets counted exactly twice, instead of both the "edge" ones only counting once
        frequencies[template[0]]++;
        frequencies[template[^1]]++;

        KeyValuePair<char, long>[] ordered = frequencies.OrderByDescending(kvp => kvp.Value).ToArray();

        return (ordered[0].Value / 2 - ordered[^1].Value / 2).ToString();
    }

    public override string Part2(string input)
    {
        string[] parts = input.Split("\n\n");
        string template = parts[0];

        Dictionary<(char, char), char> rules = new Dictionary<(char, char), char>();
        foreach (string line in parts[1].Split('\n'))
        {
            string[] splitLine = line.Split(" -> ");
            rules.Add((splitLine[0][0], splitLine[0][1]), splitLine[1][0]);
        }

        Dictionary<(char, char), long> result = Polymerize(rules, 40, template);
        Dictionary<char, long> frequencies = new Dictionary<char, long>();
        foreach (KeyValuePair<(char, char), long> kvp in result)
        {
            if (frequencies.ContainsKey(kvp.Key.Item1))
                frequencies[kvp.Key.Item1] += kvp.Value;
            else
                frequencies[kvp.Key.Item1] = kvp.Value;

            if (frequencies.ContainsKey(kvp.Key.Item2))
                frequencies[kvp.Key.Item2] += kvp.Value;
            else
                frequencies[kvp.Key.Item2] = kvp.Value;
        }

        // every char gets counted exactly twice, instead of both the "edge" ones only counting once
        frequencies[template[0]]++;
        frequencies[template[^1]]++;

        KeyValuePair<char, long>[] ordered = frequencies.OrderByDescending(kvp => kvp.Value).ToArray();

        return (ordered[0].Value / 2 - ordered[^1].Value / 2).ToString();
    }
}