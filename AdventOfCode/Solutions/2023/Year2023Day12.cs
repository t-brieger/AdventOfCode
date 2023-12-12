using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2023Day12 : Solution
{
    private static long recursiveCount(string row, int[] groups, Dictionary<(string, string), long> memorised)
    {
        string joinedGroups = string.Join(',', groups);
        if (memorised.ContainsKey((row, joinedGroups)))
            return memorised[(row, joinedGroups)];


        string[] rowParts = row.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (row.Length == 0 || row.All(c => c == '.'))
            return groups.Length == 0 ? 1 : 0;

        long ret = 0;
        if (rowParts[0].Contains('?'))
        {
            int firstQuestionMark = rowParts[0].IndexOf('?');
            rowParts[0] = rowParts[0][..firstQuestionMark] + '.' + rowParts[0][(firstQuestionMark + 1)..];
            ret += recursiveCount(string.Join('.', rowParts), groups, memorised);
            rowParts[0] = rowParts[0][..firstQuestionMark] + '#' + rowParts[0][(firstQuestionMark + 1)..];
            ret += recursiveCount(string.Join('.', rowParts), groups, memorised);
        }
        else if (groups.Length != 0 && rowParts[0].Length == groups[0])
            ret += recursiveCount(string.Join('.', rowParts.Skip(1)), groups.Skip(1).ToArray(), memorised);

        memorised[(row, joinedGroups)] = ret;
        return ret;
    }

    public override string Part1(string input)
    {
        long possibilityCount = 0;
        string[] lines = input.Split('\n');
        RescaleBar(lines.Length);

        Dictionary<(string, string), long> memoization = new();
        foreach (string line in lines)
        {
            IncreaseBar();

            string[] split = line.Split(" ");
            string row = split[0];
            int[] groups = split[1].Split(',').Select(int.Parse).ToArray();

            possibilityCount += recursiveCount(row, groups, memoization);
        }

        return possibilityCount.ToString();
    }

    public override string Part2(string input)
    {
        long possibilityCount = 0;
        string[] lines = input.Split('\n');
        RescaleBar(lines.Length);

        Dictionary<(string, string), long> memoization = new();
        foreach (string line in lines)
        {
            IncreaseBar();

            string[] split = line.Split(" ");
            string rowRaw = split[0];
            string row = rowRaw;
            for (int i = 0; i < 4; i++)
                row += "?" + rowRaw;
            int[] groupsRaw = split[1].Split(',').Select(int.Parse).ToArray();
            int[] groups = new int[groupsRaw.Length * 5];
            for (int i = 0; i < groups.Length; i++)
                groups[i] = groupsRaw[i % groupsRaw.Length];

            possibilityCount += recursiveCount(row, groups, memoization);
        }

        return possibilityCount.ToString();
    }
}
