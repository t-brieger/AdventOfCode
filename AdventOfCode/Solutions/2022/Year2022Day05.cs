using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day05 : Solution
{
    public override string Part1(string input)
    {
        input = this.rawInput;
        
        string[] lines = input.Split('\n');

        int i = 0;

        Stack<char>[] stacks = new Stack<char>[lines[0].Length / 4 + 1];
        for (int k = 0; k < stacks.Length; k++)
            stacks[k] = new Stack<char>();
        while (true)
        {
            string line = lines[i++];
            if (line.Trim() == "" || !line.Contains('['))
                break;

            for (int j = 0; j < line.Length - 1; j += 4)
            {
                if (line[j] == '[')
                    stacks[j / 4].Push(line[j + 1]);
            }
        }

        for (int k = 0; k < stacks.Length; k++)
        {
            Stack<char> s = new Stack<char>();
            while (stacks[k].Count != 0)
                s.Push(stacks[k].Pop());
            stacks[k] = s;
        }
        
        foreach (string line in lines.Skip(i))
        {
            if (line.Trim() == "")
                continue;

            string[] parts = line.Split(' ');
            int amount = int.Parse(parts[1]);
            int from = int.Parse(parts[3]);
            int to = int.Parse(parts[5]);

            for (int j = 0; j < amount; j++)
            {
                stacks[to - 1].Push(stacks[from - 1].Pop());
            }
        }
        
        return new string(stacks.Select(s => s.Count == 0 ? '\0' : s.Peek()).ToArray()).Replace("\0", "");
    }

    public override string Part2(string input)
    {
        input = this.rawInput;
        
        string[] lines = input.Split('\n');

        int i = 0;

        Stack<char>[] stacks = new Stack<char>[lines[0].Length / 4 + 1];
        for (int k = 0; k < stacks.Length; k++)
            stacks[k] = new Stack<char>();
        while (true)
        {
            string line = lines[i++];
            if (line.Trim() == "" || !line.Contains('['))
                break;

            for (int j = 0; j < line.Length - 1; j += 4)
            {
                if (line[j] == '[')
                    stacks[j / 4].Push(line[j + 1]);
            }
        }

        for (int k = 0; k < stacks.Length; k++)
        {
            Stack<char> s = new Stack<char>();
            while (stacks[k].Count != 0)
                s.Push(stacks[k].Pop());
            stacks[k] = s;
        }
        
        foreach (string line in lines.Skip(i))
        {
            if (line.Trim() == "")
                continue;

            string[] parts = line.Split(' ');
            int amount = int.Parse(parts[1]);
            int from = int.Parse(parts[3]);
            int to = int.Parse(parts[5]);

            Stack<char> tmp = new Stack<char>();
            for (int j = 0; j < amount; j++)
            {
                tmp.Push(stacks[from - 1].Pop());
            }
            while (tmp.Count != 0)
                stacks[to - 1].Push(tmp.Pop());
        }
        
        return new string(stacks.Select(s => s.Count == 0 ? '\0' : s.Peek()).ToArray()).Replace("\0", "");
    }
}