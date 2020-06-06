using System;

namespace AdventOfCode.Solutions
{
    class Year2015Day05 : Solution
    {
        public override string Part1(string input)
        {
            string[] strings = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            int count = 0;

            foreach (string s in strings)
            {
                if (s.IndexOf("ab", StringComparison.Ordinal) >= 0 ||
                    s.IndexOf("cd", StringComparison.Ordinal) >= 0 ||
                    s.IndexOf("pq", StringComparison.Ordinal) >= 0 ||
                    s.IndexOf("xy", StringComparison.Ordinal) >= 0)
                    continue;
                int vowelCount = s[^1] == 'a' || s[^1] == 'e' || s[^1] == 'i' || s[^1] == 'o' || s[^1] == 'u' ? 1 : 0;
                bool hasDoubleLetter = false;
                for (int i = 0; i < s.Length - 1; i++)
                {
                    if (s[i] == s[i + 1])
                        hasDoubleLetter = true;
                    if (s[i] == 'a' || s[i] == 'e' || s[i] == 'i' || s[i] == 'o' || s[i] == 'u')
                        vowelCount++;
                }
                if (vowelCount < 3 || !hasDoubleLetter)
                    continue;

                count++;
            }

            return count.ToString();
        }

        public override string Part2(string input)
        {
            string[] strings = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            int count = 0;

            foreach (string s in strings)
            {
                bool hasDoubleLetter = false;
                for (int i = 0; i < s.Length - 2; i++)
                {
                    if (s[i] == s[i + 2])
                        hasDoubleLetter = true;
                }

                bool canBreak = false;
                //not efficient, but meh, there arent that many strings
                for (int i = 0; i < s.Length - 1; i++)
                {
                    string twoLetters = s.Substring(i, 2);

                    for (int j = i + 2; j < s.Length - 1; j++)
                    {
                        if (j == i || j == i - 1 || j == i + 1)
                            continue;
                        if (s.Substring(j, 2) != twoLetters) continue;
                        canBreak = true;
                        break;
                    }
                    if (canBreak)
                        break;
                }

                if (!hasDoubleLetter || !canBreak)
                    continue;

                count++;
            }

            return count.ToString();
        }
    }
}
