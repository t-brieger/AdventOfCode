using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions
{
    public class Year2018Day02 : Solution
    {
        public override string Part1(string s)
        {
            string[] inputs = s.Split('\n');
            int NumOfTwoElements = 0;
            int NumOfThreeElements = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                string line = inputs[i];
                Dictionary<char, int> letters = new Dictionary<char, int>();
                foreach (char c in line)
                {
                    if (letters.ContainsKey(c))
                    {
                        letters[c]++;
                    }
                    else
                    {
                        letters.Add(c, 1);
                    }
                }

                NumOfThreeElements = letters.ContainsValue(3) ? NumOfThreeElements + 1 : NumOfThreeElements;

                NumOfTwoElements = letters.ContainsValue(2) ? NumOfTwoElements + 1 : NumOfTwoElements;
            }

            return (NumOfThreeElements * NumOfTwoElements).ToString();
        }
        
        public override string Part2(string s)
        {
            string[] inputs = s.Split(new [] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s1 in inputs)
            {
                foreach (var s2 in inputs)
                {
                    if (s1 == s2)
                    {
                        continue;
                    }

                    bool onlyonediff = true;
                    bool canExit = false;
                    for (int i = 0; i < s1.Length; i++)
                    {
                        if (s1[i] != s2[i] && onlyonediff)
                            onlyonediff = false;
                        else if (s1[i] != s2[i])
                        {
                            canExit = true;
                            break;
                        }
                    }

                    if (onlyonediff) //shouldn't happen, but just in case it was the same string
                        continue;
                    if (canExit)
                        continue;
                    StringBuilder res = new StringBuilder();
                    for (int i = 0; i < s1.Length; i++)
                        if (s1[i] == s2[i])
                            res.Append(s1[i]);
                    return res.ToString();
                }
            }

            return "";
        }
    }
}