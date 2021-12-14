using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2020Day16 : Solution
{
    public override string Part1(string input)
    {
        HashSet<int> validValues = new();

        string[][] dataGroups = input.Split("\n\n")
            .Select(x => x.Split('\n', StringSplitOptions.RemoveEmptyEntries)).ToArray();
        foreach (int[] vals in dataGroups[0].SelectMany(constraint => constraint.Split(": ")[1].Split(" or ")
                     .Select(x => x.Split('-').Select(int.Parse).ToArray())))
            for (int i = vals[0]; i <= vals[1]; i++)
                validValues.Add(i);

        int error = 0;

        foreach (int[] ticket in dataGroups[2].Skip(1).Select(line => line.Split(',').Select(int.Parse).ToArray()))
        {
            bool isTicketValid = true;
            foreach (int field in from field in ticket
                     let isValid = validValues.Contains(field)
                     where !isValid
                     select field)
            {
                error += field;
                isTicketValid = false;
            }

            if (isTicketValid)
            {
            }
        }

        return error.ToString();
    }

    public override string Part2(string input)
    {
        string[][] dataGroups = input.Split("\n\n")
            .Select(x => x.Split('\n', StringSplitOptions.RemoveEmptyEntries)).ToArray();
        Dictionary<string, HashSet<int>> rules = new(dataGroups[0].Length);


        foreach (string rule in dataGroups[0])
        {
            string name = rule.Split(": ")[0];
            HashSet<int> validValues = new();
            foreach (int[] vals in rule.Split(": ")[1].Split(" or ")
                         .Select(x => x.Split('-').Select(int.Parse).ToArray()))
                for (int i = vals[0]; i <= vals[1]; i++)
                    validValues.Add(i);

            rules.Add(name, validValues);
        }

        int[][] tickets = dataGroups[2].Skip(1).Select(line => line.Split(',').Select(int.Parse).ToArray())
            .Where(ints => ints.All(i => rules.Any(r => r.Value.Contains(i)))).ToArray();

        Dictionary<int, List<string>> possibilities = new(rules.Count);
        foreach ((string key, _) in rules)
            for (int i = 0; i < rules.Count; i++)
            {
                if (!possibilities.ContainsKey(i))
                    possibilities.Add(i, new List<string>());
                if (tickets.All(ticket => rules[key].Contains(ticket[i])))
                    possibilities[i].Add(key);
            }

        while (possibilities.Any(kvp => kvp.Value.Count != 1))
            for (int i = 0; i < possibilities.Count; i++)
            {
                if (possibilities[i].Count != 1) continue;
                for (int j = 0; j < possibilities.Count; j++)
                {
                    if (j == i)
                        continue;
                    possibilities[j].Remove(possibilities[i][0]);
                }
            }

        return dataGroups[1][1].Split(',').Select(int.Parse)
            .Where((_, i) => possibilities[i][0].StartsWith("departure"))
            .Aggregate(1L, (total, val) => total * val).ToString();
    }
}