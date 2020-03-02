using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions
{
    public class Year2018Day07 : Solution
    {
        public override string Part1(string input)
        {
            Dictionary<char, List<char>> requirements = new Dictionary<char, List<char>>();
            
            foreach (string line in input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!requirements.ContainsKey(line[36]))
                    requirements.Add(line[36], new List<char>(5));
                requirements[line[36]].Add(line[5]);
                if (!requirements.ContainsKey(line[5]))
                    requirements.Add(line[5], new List<char>(5));
            }

            HashSet<char> done = new HashSet<char>();
            StringBuilder output = new StringBuilder(requirements.Count);
            while (requirements.Count > 0)
            {
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    if (!requirements.TryGetValue(c, out List<char> val))
                        continue;
                    if (val.Any(_ => !done.Contains(_)))
                        continue;
                    done.Add(c);
                    output.Append(c);
                    requirements.Remove(c);
                    break;
                }
            }

            return output.ToString();
        }

        public override string Part2(string input)
        {
            Dictionary<char, List<char>> requirements = new Dictionary<char, List<char>>();

            foreach (string line in input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!requirements.ContainsKey(line[36]))
                    requirements.Add(line[36], new List<char>(5));
                requirements[line[36]].Add(line[5]);
                if (!requirements.ContainsKey(line[5]))
                    requirements.Add(line[5], new List<char>(5));
            }

            char[] working = new char[5];


            byte[] timeLeft = new byte[working.Length];
            for (int i = 0; i < working.Length; i++)
                working[i] = ' ';

            HashSet<char> done = new HashSet<char>();
            int time = 0;
            while (requirements.Count > 0)
            {
                time++;
                for (int i = 0; i < working.Length; i++)
                {
                    if (working[i] == ' ')
                        continue;
                    if (timeLeft[i] == 1)
                    {
                        done.Add(working[i]);
                        requirements.Remove(working[i]);
                        working[i] = ' ';
                    }
                    if (working[i] != ' ')
                    {
                        timeLeft[i]--;
                    }
                }

                for (char c = 'A'; c <= 'Z'; c++)
                {
                    if (!requirements.TryGetValue(c, out List<char> val))
                        continue;
                    if (val.Any(_ => !done.Contains(_)))
                        continue;
                    if (working.Any(_ => _ == c))
                        continue;
                    for (int i = 0; i < working.Length; i++)
                    {
                        if (working[i] == ' ')
                        {
                            working[i] = c;
                            timeLeft[i] = (byte)((byte)c - 4);
                            break;
                        }
                    }
                }
            }

            return (time - 1).ToString();
        }
    }
}