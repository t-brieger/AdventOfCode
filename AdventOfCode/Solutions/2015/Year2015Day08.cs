using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2015Day08 : Solution
    {
        public override string Part1(string input)
        {
            string[] strings = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Substring(1, s.Length - 2)).ToArray();
            int actualTextLength = 0;

            foreach (string s in strings)
                for (int i = 0; i < s.Length; i++)
                {
                    actualTextLength++;
                    if (s[i] != '\\') continue;
                    if (s[i + 1] == '\\' || s[i + 1] == '"')
                        i++;
                    else
                        i += 3;
                }

            return (strings.Sum(s => s.Length + 2) - actualTextLength).ToString();
        }

        public override string Part2(string input)
        {
            string[] strings = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string[] encodedStrings = strings.Select(s =>
                '"' + s.Replace("\"", "DOUBLEQUOTE").Replace("\\", "BACKSLASH").Replace("DOUBLEQUOTE", "\\\"")
                    .Replace("BACKSLASH", "\\\\") + '"').ToArray();

            return (encodedStrings.Sum(s => s.Length) - strings.Sum(s => s.Length)).ToString();
        }
    }
}