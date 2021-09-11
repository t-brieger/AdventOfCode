using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2017Day04 : Solution
    {
        public override string Part1(string input)
        {
            string[] passPhrases = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            int valid = passPhrases.Length;
            foreach (string s in passPhrases)
            {
                HashSet<string> words = new();
                foreach (string word in s.Split(' '))
                {
                    if (words.Contains(word))
                    {
                        valid--;
                        break;
                    }

                    words.Add(word);
                }
            }

            return valid.ToString();
        }

        public override string Part2(string input)
        {
            string[] passPhrases = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            int valid = passPhrases.Length;
            foreach (string s in passPhrases)
            {
                HashSet<string> words = new();
                foreach (string ordered in s.Split(' ').Select(word => new string(word.OrderBy(c => c).ToArray())))
                {
                    if (words.Contains(ordered))
                    {
                        valid--;
                        break;
                    }

                    words.Add(ordered);
                }
            }

            return valid.ToString();
        }
    }
}