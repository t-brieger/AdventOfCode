using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2016Day21 : Solution
{
    private static void Rotate(IList<char> s, int count, bool right)
    {
        count %= s.Count;
        char[] n = new char[s.Count];

        for (int i = 0; i < s.Count; i++)
        {
            // add s.Length before "% s.Count" to avoid negative indices
            n[i] = s[(i + (right ? -1 : 1) * count + s.Count) % s.Count];
        }

        for (int i = 0; i < s.Count; i++)
        {
            s[i] = n[i];
        }
    }

    private static void Reverse(List<char> s, int from, int to)
    {
        for (int i = 0; i < (to - from + 1) / 2; i++)
        {
            (s[from + i], s[to - i]) = (s[to - i], s[from + i]);
        }
    }

    private static string Scramble(string[][] instructions, string input)
    {
        List<char> pass = input.ToCharArray().ToList();

        foreach (string[] instruction in instructions)
        {
            if (instruction[0] == "swap" && instruction[1] == "position")
            {
                int x = int.Parse(instruction[2]);
                int y = int.Parse(instruction[5]);

                (pass[x], pass[y]) = (pass[y], pass[x]);
            }
            else if (instruction[0] == "swap" && instruction[1] == "letter")
            {
                char x = instruction[2][0];
                char y = instruction[5][0];
                for (int i = 0; i < pass.Count; i++)
                {
                    if (pass[i] == x)
                        pass[i] = y;
                    else if (pass[i] == y)
                        pass[i] = x;
                }
            }
            else if (instruction[0] == "rotate")
            {
                int count;
                bool right = instruction[1] != "left";

                if (instruction[1] == "based")
                {
                    int index = pass.FirstIndexOf(instruction[6][0]);
                    count = index + 1;
                    if (index >= 4)
                        count++;
                }
                else
                {
                    count = int.Parse(instruction[2]);
                }

                Rotate(pass, count, right);
            }
            else if (instruction[0] == "reverse")
            {
                int x = int.Parse(instruction[2]);
                int y = int.Parse(instruction[4]);

                Reverse(pass, x, y);
            }
            else
            {
                int x = int.Parse(instruction[2]);
                char xc = pass[x];
                int y = int.Parse(instruction[5]);

                pass.RemoveAt(x);
                pass.Insert(y, xc);
            }
        }

        return new string(pass.ToArray());
    }
    
    public override string Part1(string input)
    {
        string[][] instructions = input.Split('\n').Select(l => l.Split(' ')).ToArray();

        return Scramble(instructions, "abcdefgh");
    }

    public override string Part2(string input)
    {
        string[][] instructions = input.Split('\n').Select(l => l.Split(' ')).ToArray();

        foreach (char[] candidate in Util.GetPermutations("abcdefgh", 8))
        {
            if (Scramble(instructions, new string(candidate)) == "fbgdceah")
                return new string(candidate);
        }

        return null;
    }
}
