using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2020Day07 : Solution
    {
        private void getPossibleParents(Dictionary<string, List<string>> rules, HashSet<string> alreadyFound, string start)
        {
            foreach (string parent in rules[start])
            {
                if (!alreadyFound.Contains(parent))
                {
                    alreadyFound.Add(parent);
                    getPossibleParents(rules, alreadyFound, parent);
                }
            }
        }

        public override string Part1(string input)
        {
            string[][] rules = input
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => new string(line.Substring(0, line.Length - 1).Where(c => c < '0' || c > '9').ToArray())
                    .Replace(" bags", " bag").Replace(" bag", "")
                    .Split(" contain ")).ToArray();

            Dictionary<string, List<string>> bagRulesDict = new Dictionary<string, List<string>>();
            
            foreach (string[] rule in rules)
            {
                List<string> templist = new List<string>(new[] {rule[0]});
                templist.AddRange(rule[1].Substring(1).Split(", ").Select(x => x.Trim()));
                string[] actualRule = templist.ToArray();
                if (!bagRulesDict.ContainsKey(actualRule[0]))
                    bagRulesDict.Add(actualRule[0], new List<string>());
                for (int i = 1; i < actualRule.Length; i++)
                {
                    if (!bagRulesDict.ContainsKey(actualRule[i]))
                        bagRulesDict.Add(actualRule[i], new List<string>());
                    
                    bagRulesDict[actualRule[i]].Add(actualRule[0]);
                    //Console.WriteLine("added: " + actualRule[i] + " is contained by " + actualRule[0] + " (and " + bagRulesDict[actualRule[i]].Count + " in total)");
                }
                //Console.WriteLine("done adding things contained by " + actualRule[0]);
            }

            HashSet<string> parents = new HashSet<string>();
            getPossibleParents(bagRulesDict, parents, "shiny gold");

            return parents.Count.ToString();
        }

        private void getWeight(Dictionary<string, Dictionary<string, int>> weights, Dictionary<string, int> totalWeights, string start)
        {
            if (totalWeights.ContainsKey(start))
                return;
            int bagAmount = 1; //the one we're on right now
            foreach (KeyValuePair<string, int> kvp in weights[start])
            {
                if (!totalWeights.ContainsKey(kvp.Key))
                    getWeight(weights, totalWeights, kvp.Key);
                
                bagAmount += totalWeights[kvp.Key] * kvp.Value;
            }

            totalWeights.Add(start, bagAmount);
        }
        
        public override string Part2(string input)
        {
            string[][] rules = input
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => new string(line.Substring(0, line.Length - 1).ToArray())
                    .Replace(" bags", " bag").Replace(" bag", "")
                    .Split(" contain ")).ToArray();

            Dictionary<string, Dictionary<string, int>> bagContainsAmounts = new Dictionary<string, Dictionary<string, int>>();
            
            foreach (string[] rule in rules)
            {
                if (!bagContainsAmounts.ContainsKey(rule[0]))
                    bagContainsAmounts.Add(rule[0], new Dictionary<string, int>());
                if (rule[1] == "no other")
                    continue;
                
                List<(int, string)> templist = new List<(int, string)>();
                templist.AddRange(rule[1].Split(", ").Select(x => x.Trim()).Select(x => x.Split(' ', 2))
                    .Select(arr => (int.Parse(arr[0]), arr[1])));
                (int, string)[] actualRule = templist.ToArray();
                for (int i = 0; i < actualRule.Length; i++)
                {
                    bagContainsAmounts[rule[0]].Add(actualRule[i].Item2, actualRule[i].Item1);
                    //Console.WriteLine("added: " + actualRule[i] + " is contained by " + actualRule[0] + " (and " + bagRulesDict[actualRule[i]].Count + " in total)");
                }
                //Console.WriteLine("done adding things contained by " + actualRule[0]);
            }
            
            Dictionary<string, int> totalWeight = new Dictionary<string, int>();
            getWeight(bagContainsAmounts, totalWeight, "shiny gold");

            return (totalWeight["shiny gold"] - 1).ToString();
        }
    }
}
