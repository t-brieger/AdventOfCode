using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2021Day18 : Solution
{
    private static List<(int value, int depth)> Parse(string s)
    {
        List<(int val, int depth)> output = new List<(int val, int depth)>();

        int nestLevel = 0;
        foreach (char c in s)
        {
            if (c == '[')
                nestLevel++;
            else if (c == ']')
                nestLevel--;
            else if (c == ',')
                continue;
            else
                output.Add((c - '0', nestLevel));
        }

        return output;
    }

    private static List<(int, int)> Add(List<(int val, int depth)> a, List<(int, int)> b)
    {
        a = a.Select(t => (t.val, t.depth + 1)).ToList();
        b = b.Select(t => (t.Item1, t.Item2 + 1)).ToList();
        a.AddRange(b);

        while (true)
        {
            if (Explode(a))
                continue;

            if (Split(a))
                continue;
                
            break;
        }

        return a;
    }
    
    private static bool Explode(List<(int, int)> t)
    {
        for (int i = 0; i < t.Count - 1; i++)
        {
            // it's a pair of numbers, not nested
            if (t[i].Item2 != t[i + 1].Item2 || t[i].Item2 < 5) continue;
            if (i != 0)
                t[i - 1] = (t[i - 1].Item1 + t[i].Item1, t[i - 1].Item2);
            if (i != t.Count - 2)
                t[i + 2] = (t[i + 2].Item1 + t[i + 1].Item1, t[i + 2].Item2);
                
            t.RemoveAt(i);
            t[i] = (0, t[i].Item2 - 1);
            return true;
        }

        return false;
    }

    private static int Magnitude(List<(int, int)> t)
    {
        for (int i = 4; i > 0; i--)
        {
            bool @continue = true;
            while (@continue)
            {
                @continue = false;
                for (int j = 0; j < t.Count - 1; j++)
                {
                    (int, int) left = t[j];
                    (int, int) right = t[j + 1];
                    if (left.Item2 != i) continue;
                    t.RemoveAt(j);
                    t.RemoveAt(j);
                    t.Insert(j, (3 * left.Item1 + 2 * right.Item1, left.Item2 - 1));
                    @continue = true;
                    break;
                }
            }
        }

        return t.Single().Item1;
    }
    
    private static bool Split(List<(int, int)> t)
    {
        for (int i = 0; i < t.Count; i++)
        {
            if (t[i].Item1 <= 9) continue;
            (int val, int depth) = t[i];
            t.RemoveAt(i);
            t.Insert(i, (val / 2, depth + 1));
            t.Insert(i+1, ((int) Math.Ceiling(val / 2f), depth + 1));
            return true;
        }
        
        return false;
    }
    
    public override string Part1(string input)
    {
        string[] lines = input.Split('\n');

        List<(int val, int depth)> parsed = Parse(lines[0]);

        foreach (string line in lines[1..])
        {
            List<(int, int)> tempParsed = Parse(line);

            parsed = Add(parsed, tempParsed);
        }

        return Magnitude(parsed).ToString();
    }

    public override string Part2(string input)
    {
        string[] lines = input.Split('\n');

        List<(int val, int depth)>[] parsed = lines.Select(Parse).ToArray();

        int maxMagnitude = int.MinValue;

        for (int i = 0; i < parsed.Length; i++)
        {
            for (int j = 0; j < parsed.Length; j++)
            {
                if (i == j)
                    continue;

                maxMagnitude = Math.Max(maxMagnitude, Magnitude(Add(parsed[i], parsed[j])));
            }
        }

        return maxMagnitude.ToString();
    }
}