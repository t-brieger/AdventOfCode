using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2019Day04 : Solution
    {
        public override string Part1(string input)
        {
            int lowerBound, upperBound;
            (lowerBound, upperBound) = input.Split('-').Select(Int32.Parse).ToArray();

            int count = 0;

            for (int i = lowerBound; i <= upperBound; i++)
            {
                string str = i.ToString();
                bool hasDouble = false;
                bool isBreakExit = false;
                for (int j = 0; j < str.Length - 1; j++)
                {
                    if (str[j] == str[j + 1])
                        hasDouble = true;
                    if (str[j] <= str[j + 1]) continue;
                    isBreakExit = true;
                    break;
                }

                if (!isBreakExit && hasDouble)
                    count++;
            }

            return count.ToString();
        }

        public override string Part2(string input)
        {
            int lowerBound, upperBound;
            (lowerBound, upperBound) = input.Split('-').Select(Int32.Parse).ToArray();

            int count = 0;

            for (int i = lowerBound; i <= upperBound; i++)
            {
                string str = i.ToString();
                bool hasDouble = false;

                char last = '\0';
                int len = 0;

                foreach (char c in str)
                {
                    if (c == last)
                        len++;
                    else
                    {
                        if (len == 2)
                        {
                            hasDouble = true;
                            break;
                        }
                        len = 1;
                        last = c;
                    }
                }

                if (!hasDouble && len != 2)
                    continue;

                bool isBreakExit = false;
                for (int j = 0; j < str.Length - 1; j++)
                {
                    if (str[j] <= str[j + 1]) continue;
                    isBreakExit = true;
                    break;
                }

                if (!isBreakExit)
                    count++;
            }

            return count.ToString();
        }
    }
}