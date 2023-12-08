using System.Collections.Generic;
using System.Linq;
using System.Xml.Xsl;

namespace AdventOfCode.Solutions;

public class Year2023Day08 : Solution
{
    public override string Part1(string input)
    {
        string[] parts = input.Split("\n\n");
        string instruc = parts[0];
        string map = parts[1];
        Dictionary<string, (string, string)> dic = new();
        foreach (string line in map.Split('\n'))
        {
            string[] split = line.Replace("(", "").Replace(")", "").Split(" = ");
            string inp = split[0];
            (string l, string r) = split[1].Split(", ");
            dic[inp] = (l, r);
        }

        string curr = "AAA";
        int steps = 0;
        while (curr != "ZZZ")
        {
            curr = instruc[steps % instruc.Length] == 'L' ? dic[curr].Item1 : dic[curr].Item2;
            steps++;
        }
        
        return steps.ToString();
    }

    public override string Part2(string input)
    {
        string[] parts = input.Split("\n\n");
        string instruc = parts[0];
        string map = parts[1];
        Dictionary<string, (string, string)> dic = new();
        foreach (string line in map.Split('\n'))
        {
            string[] split = line.Replace("(", "").Replace(")", "").Split(" = ");
            string inp = split[0];
            (string l, string r) = split[1].Split(", ");
            dic[inp] = (l, r);
        }

        HashSet<int> stepCounts = new();
        foreach (string initial in dic.Keys.Where(k => k.EndsWith("A")))
        {
            string curr = initial;
            int steps = 0;
            while (!curr.EndsWith("Z")) 
            {
                curr = instruc[steps % instruc.Length] == 'L' ? dic[curr].Item1 : dic[curr].Item2;
                steps++;
            }

            stepCounts.Add(steps);
        }

        long lcm = 1;
        foreach (int i in stepCounts)
        {
            lcm = Util.Lcm(lcm, i);
        }

        return lcm.ToString();
    }
}
