using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2021Day10 : Solution
    {
        private readonly HashSet<string> corrupted = new();
        
        public override string Part1(string input)
        {
            int score = 0;
            
            foreach (string s in input.Split('\n'))
            {
                Stack<char> opened = new();

                foreach (char c in s)
                {
                    if (c is '{' or '[' or '(' or '<')
                        opened.Push(c);

                    if (c is not ('}' or ']' or '>' or ')')) continue;
                    char c2 = opened.Pop();
                    if (c2 == c - (c == ')' ? 1 : 2)) continue;
                        
                    score += c switch
                    {
                        ')' => 3,
                        ']' => 57,
                        '}' => 1197,
                        '>' => 25137,
                        _ => throw new ArgumentOutOfRangeException(nameof(input),"Non-Bracket Character Found.")
                    };
                    corrupted.Add(s);
                    break;
                }
            }

            return score.ToString();
        }

        public override string Part2(string input)
        {
            List<long> scores = new();

            foreach (string s in input.Split('\n'))
            {
                if (corrupted.Contains(s)) continue;
                
                Stack<char> opened = new();

                foreach (char c in s)
                {
                    if (c is '{' or '[' or '(' or '<')
                        opened.Push(c);

                    if (c is not ('}' or ']' or '>' or ')')) continue;
                    if (opened.Pop() != c - (c == ')' ? 1 : 2))
                        throw new Exception();
                }

                long score = 0;
                while (opened.Count != 0)
                {
                    char c = opened.Pop();
                    score *= 5;
                    score += c switch
                    {
                        '(' => 1,
                        '[' => 2,
                        '{' => 3,
                        _ => 4
                    };
                }

                scores.Add(score);
            }

            scores = scores.OrderByDescending(e => e).ToList();
            // truncation is helpful:
            // length 3 -> ix 1
            // length 5 -> ix 2
            // length 7 -> ix 3
            // ...
            return scores[scores.Count / 2].ToString();
        }
    }
}