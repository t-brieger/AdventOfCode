using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day13 : Solution
{
    private class Value
    {
        public bool isList;
        public int val;
        public List<Value> list;

        public override string ToString()
        {
            if (isList)
                return '[' + string.Join(", ", list) + ']';
            
            return val.ToString();
        }
    }

    private static Value ParseValue(string s, out int consumed)
    {
        Value v = new Value();
        if (s[0] == '[')
        {
            consumed = 1;
            v.isList = true;
            v.list = new List<Value>();
            s = s.Substring(1);
            if (s[0] == ']')
            {
                consumed++;
                return v;
            }

            while (true)
            {
                v.list.Add(ParseValue(s, out int cons));
                consumed += cons;
                s = s.Substring(cons);
                consumed++; //trailing comma or ]
                if (s[0] == ']')
                    break;
                else
                    s = s.Substring(1);
            }

            return v;
        }

        v.isList = false;
        string integer = "";
        consumed = 0;
        while (s[0] is >= '0' and <= '9')
        {
            consumed++;
            integer += s[0];
            s = s.Substring(1);
        }

        v.val = int.Parse(integer);
        return v;
    }

    private static bool? ValidPair(Value v1, Value v2)
    {
        if (!v1.isList && !v2.isList)
            return v1.val < v2.val ? true : v1.val > v2.val ? false : null;
        if (v1.isList && v2.isList)
        {
            for (int i = 0; i <= Math.Max(v1.list.Count, Math.Max(v2.list.Count, 1)); i++)
            {
                if (i == v1.list.Count && i == v2.list.Count)
                    return null;
                if (i != v1.list.Count && i == v2.list.Count)
                    return false;
                if (i == v1.list.Count && i != v2.list.Count)
                    return true;

                bool? itemComp = ValidPair(v1.list[i], v2.list[i]);
                if (itemComp == null)
                    continue;
                return itemComp;
            }
        }

        if (v1.isList && !v2.isList)
        {
            Value newv2 = new Value();
            newv2.isList = true;
            newv2.list = new List<Value>();
            Value tmp = new Value();
            tmp.isList = false;
            tmp.val = v2.val;
            newv2.list.Add(tmp);
            v2 = newv2;
        }

        if (!v1.isList && v2.isList)
        {
            Value newv1 = new Value();
            newv1.isList = true;
            newv1.list = new List<Value>();
            Value tmp = new Value();
            tmp.isList = false;
            tmp.val = v1.val;
            newv1.list.Add(tmp);
            v1 = newv1;
        }

        return ValidPair(v1, v2);
    }

    public override string Part1(string input)
    {
        string[] pairs = input.Split("\n\n");

        int ixSum = 0;

        for (int i = 0; i < pairs.Length; i++)
        {
            string[] nodes = pairs[i].Split('\n');
            if ((bool) ValidPair(ParseValue(nodes[0], out int _), ParseValue(nodes[1], out int _)))
                ixSum += i + 1;
        }

        return ixSum.ToString();
    }

    public override string Part2(string input)
    {
        input += "\n\n[[2]]\n[[6]]";
        
        string[] pairs = input.Split("\n\n");

        List<Value> sorted = new List<Value>();

        for (int i = 0; i < pairs.Length; i++)
        {
            Value[] nodes = pairs[i].Split('\n').Select(s => ParseValue(s, out int _)).ToArray();

            foreach (Value v in nodes)
            {
                int guess = 0;
                while (guess < sorted.Count && ValidPair(sorted[guess], v) == true)
                    guess++;
                sorted.Insert(guess, v);
            }
        }

        return ((sorted.FindIndex(v =>
                    v.isList && v.list.Count == 1 && v.list[0].isList && v.list[0].list.Count == 1 &&
                    v.list[0].list[0].val == 2) + 1) *
                (sorted.FindIndex(v =>
                    v.isList && v.list.Count == 1 && v.list[0].isList && v.list[0].list.Count == 1 &&
                    v.list[0].list[0].val == 6) + 1)).ToString();
    }
}
