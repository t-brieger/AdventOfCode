using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2023Day12 : Solution
{
    private static void recursiveResolveQuestionmarks(string row, HashSet<string> found = null, string curr = "")
    {
        if (row.Length == 0)
        {
            found.Add(curr);
            return;
        }

        if (row[0] == '?')
        {
            recursiveResolveQuestionmarks(row[1..], found, curr + ".");
            recursiveResolveQuestionmarks(row[1..], found, curr + "#");
        }
        else
            recursiveResolveQuestionmarks(row[1..], found, curr + row[0]);
    }
    
    public override string Part1(string input)
    {
        long possibilityCount = 0;
        string[] lines = input.Split('\n');
        RescaleBar(lines.Length);
        
        foreach (string line in lines)
        {
            IncreaseBar();
            HashSet<string> hs = new();
            string[] split = line.Split(" ");
            string row = split[0];
            int[] groups = split[1].Split(',').Select(int.Parse).ToArray();
            
            recursiveResolveQuestionmarks(row, hs);

            foreach (string candidate in hs)
            {
                int rowIx = 0;
                bool problem = false;
                for (int i = 0; i < groups.Length; i++)
                {
                    if (rowIx == candidate.Length)
                    {
                        problem = true;
                        break;
                    }

                    int groupLength = 0;
                    while (rowIx < candidate.Length && candidate[rowIx] == '.')
                        rowIx++;
                    while (rowIx < candidate.Length && candidate[rowIx] == '#')
                    {
                        groupLength++;
                        rowIx++;
                    }
                    while (rowIx < candidate.Length && candidate[rowIx] == '.')
                        rowIx++;

                    if (groupLength != groups[i])
                        problem = true;
                }

                if (!problem && rowIx == candidate.Length)
                {
                    possibilityCount++;
                }
            }
        }

        return possibilityCount.ToString();
    }

    public override string Part2(string input)
    {
        long possibilityCount = 0;
        string[] lines = input.Split('\n');
        RescaleBar(lines.Length);
        
        foreach (string line in lines)
        {
            IncreaseBar();

            HashSet<string> hs = new();
            string[] split = line.Split(" ");
            string rowRaw = split[0];
            string row = rowRaw;
            for (int i = 0; i < 4; i++)
                row += "?" + rowRaw;
            int[] groupsRaw = split[1].Split(',').Select(int.Parse).ToArray();
            int[] groups = new int[groupsRaw.Length * 5];
            for (int i = 0; i < groups.Length; i++)
                groups[i] = groupsRaw[i % groupsRaw.Length];
            
            recursiveResolveQuestionmarks(row, hs);

            foreach (string candidate in hs)
            {
                int rowIx = 0;
                bool problem = false;
                for (int i = 0; i < groups.Length; i++)
                {
                    if (rowIx == candidate.Length)
                    {
                        problem = true;
                        break;
                    }

                    int groupLength = 0;
                    while (rowIx < candidate.Length && candidate[rowIx] == '.')
                        rowIx++;
                    while (rowIx < candidate.Length && candidate[rowIx] == '#')
                    {
                        groupLength++;
                        rowIx++;
                    }
                    while (rowIx < candidate.Length && candidate[rowIx] == '.')
                        rowIx++;

                    if (groupLength != groups[i])
                        problem = true;
                }

                if (!problem && rowIx == candidate.Length)
                {
                    possibilityCount++;
                }
            }
        }

        return possibilityCount.ToString();
    }
}
