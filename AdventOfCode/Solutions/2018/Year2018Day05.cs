using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
    public class Year2018Day05 : Solution
    {
        private static bool opposite_polarity(char c, char c2)
        {
            return (c - 32 == c2 || c2 - 32 == c);
        }

        private static string FullyReact(string input)
        {
            /*
            while (true)
            {
                Boolean hasChange = false;
                for (char c = 'a'; c <= 'z'; c++)
                {
                    int index = input.IndexOf("" + c + char.ToUpper(c));
                    index = index >= 0 ? index : input.IndexOf("" + char.ToUpper(c) + c);
                    if (index < 0)
                        continue;
                    hasChange = true;
                    input = input.Substring(0, index) + input.Substring(index+2);
                }

                if (!hasChange)
                    break;
            }

            return input; //takes a few minutes */

            Stack<char> s = new Stack<char>();

            foreach (char p in input)
            {
                if (s.Count > 0 && opposite_polarity(p, s.Peek()))
                    s.Pop();
                else
                    s.Push(p);
            }

            return new string(s.ToArray());
        }

        public override string Part1(string input)
        {
            return FullyReact(input).Length.ToString();
        }

        public override string Part2(string input)
        {
            int shortest = input.Length - 1;
            for (char c = 'a'; c <= 'z'; c++)
            {
                int length = FullyReact(input.Replace(c.ToString(), "").Replace(char.ToUpper(c).ToString(), "")).Length;
                if (length < shortest)
                    shortest = length;
            }

            return shortest.ToString();
        }
    }
}