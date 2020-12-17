using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AdventOfCode.Solutions
{
    public class Year2020Day16 : Solution
    {
        public override string Part1(string input)
        {
            HashSet<int> validValues = new HashSet<int>();

            string[][] dataGroups = input.Split("\n\n")
                .Select(x => x.Split('\n', StringSplitOptions.RemoveEmptyEntries)).ToArray();
            foreach (string constraint in dataGroups[0])
            {
                foreach (int[] vals in constraint.Split(": ")[1].Split(" or ")
                    .Select(x => x.Split('-').Select(int.Parse).ToArray()))
                {
                    for (int i = vals[0]; i <= vals[1]; i++)
                    {
                        validValues.Add(i);
                    }
                }
            }

            int error = 0;
            int validCount = 0;
            int totalCount = 0;

            foreach (int[] ticket in dataGroups[2].Skip(1).Select(line => line.Split(',').Select(int.Parse).ToArray()))
            {
                bool isTicketValid = true;
                totalCount++;
#if VIS
                Console.Write(new string(' ', Console.BufferWidth - 1));
                Console.CursorLeft = 0;
#endif
                foreach (int field in ticket)
                {
                    bool isValid = validValues.Contains(field);

                    if (!isValid)
                    {
                        error += field;
                        isTicketValid = false;
                    }
#if VIS
                    Console.ForegroundColor = isValid ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.Write(field + " ");
#endif
                }

                if (isTicketValid)
                    validCount++;
#if VIS
                Console.ResetColor();
                Console.Write("\nCorrect: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(validCount);
                Console.ResetColor();
                Console.Write(
                    $"/{totalCount}, {(int) ((float) validCount / totalCount * 100)}% - total error: {error}");
                Thread.Sleep(70);
                Console.CursorLeft = 0;
                Console.CursorTop -= 1;
#endif
            }
#if VIS
            Console.WriteLine("\n\n\n");
#endif
            return error.ToString();
        }

        public override string Part2(string input)
        {
            string[][] dataGroups = input.Split("\n\n")
                .Select(x => x.Split('\n', StringSplitOptions.RemoveEmptyEntries)).ToArray();
            Dictionary<string, HashSet<int>> rules = new Dictionary<string, HashSet<int>>(dataGroups[0].Length);


            foreach (string rule in dataGroups[0])
            {
                string name = rule.Split(": ")[0];
                HashSet<int> validValues = new HashSet<int>();
                foreach (int[] vals in rule.Split(": ")[1].Split(" or ")
                    .Select(x => x.Split('-').Select(int.Parse).ToArray()))
                {
                    for (int i = vals[0]; i <= vals[1]; i++)
                    {
                        validValues.Add(i);
                    }
                }

                rules.Add(name, validValues);
            }

            int[][] tickets = dataGroups[2].Skip(1).Select(line => line.Split(',').Select(int.Parse).ToArray())
                .Where(ints => ints.All(i => rules.Any(r => r.Value.Contains(i)))).ToArray();

            Dictionary<int, List<string>> possibilities = new Dictionary<int, List<string>>(rules.Count);
            foreach (KeyValuePair<string, HashSet<int>> kvp in rules)
                for (int i = 0; i < rules.Count; i++)
                {
                    if (!possibilities.ContainsKey(i))
                        possibilities.Add(i, new List<string>());
                    if (tickets.All(ticket => rules[kvp.Key].Contains(ticket[i])))
                        possibilities[i].Add(kvp.Key);
                }

            while (possibilities.Any(kvp => kvp.Value.Count != 1))
            {
                for (int i = 0; i < possibilities.Count; i++)
                {
                    if (possibilities[i].Count == 1)
                    {
                        for (int j = 0; j < possibilities.Count; j++)
                        {
                            if (j == i)
                                continue;
                            possibilities[j].Remove(possibilities[i][0]);
                        }
                    }
                }
            }

            return dataGroups[1][1].Split(',').Select(int.Parse)
                .Where((val, i) => possibilities[i][0].StartsWith("departure"))
                .Aggregate(1L, (total, val) => total * val).ToString();
        }
    }
}
