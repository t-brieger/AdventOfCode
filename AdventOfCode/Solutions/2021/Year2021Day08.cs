using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2021Day08 : Solution
    {
        public override string Part1(string input)
        {
            return input.Split('\n').Select(l => l.Split(" | ")[1])
                .Select(output => output.Split(' ').Count(x => x.Length is 2 or 4 or 3 or 7)).Sum().ToString();
        }

        public override string Part2(string input)
        {
            string[] displays = input.Split('\n');

            int outputSum = 0;

            foreach (string display in displays)
            {
                (string[] test, string[] data) = display.Split(" | ").Select(x => x.Split(' ')).ToArray();

                Dictionary<int, string> numbermap = new();
                for (int i = 0; i <= 9; i++)
                    numbermap[i] = "";

                // might have to iterate multiple times
                bool changes = true;
                while (changes)
                {
                    changes = false;
                    Dictionary<int, string> oldNumMap = new(numbermap);
                    foreach (string code in test.Select(rawcode => new string(rawcode.ToCharArray().OrderByDescending(c => (int) c).ToArray())))
                    {
                        switch (code.Length)
                        {
                            case 2:
                                numbermap[1] = code;
                                break;
                            case 3:
                                numbermap[7] = code;
                                break;
                            case 4:
                                numbermap[4] = code;
                                break;
                            case 7:
                                numbermap[8] = code;
                                break;
                            case 5:
                                if (code.Count(c => numbermap[1].Contains(c)) == 2)
                                    numbermap[3] = code;
                                else if (code.Count(c => numbermap[4].Contains(c)) == 2)
                                    numbermap[2] = code;
                                else if (code.Count(c => numbermap[4].Contains(c)) == 3)
                                    numbermap[5] = code;
                                break;
                            case 6:
                                if (code.Count(c => numbermap[4].Contains(c)) == 4)
                                    numbermap[9] = code;
                                else if (code.Count(c => numbermap[5].Contains(c)) == 5)
                                    numbermap[6] = code;
                                else if (code.Count(c => numbermap[5].Contains(c)) == 4)
                                    numbermap[0] = code;
                                break;
                        }
                    }

                    for (int i = 0; i <= 9; i++)
                    {
                        if (numbermap[i] != oldNumMap[i])
                            changes = true;
                    }
                }

                int tmpSum = 0;
                
                foreach (string code in data.Select(rawcode => new string(rawcode.ToCharArray().OrderByDescending(c => (int) c).ToArray())))
                {
                    tmpSum *= 10;
                    tmpSum += numbermap.First(kvp => kvp.Value == code).Key;
                }

                outputSum += tmpSum;
            }

            return outputSum.ToString();
        }
    }
}