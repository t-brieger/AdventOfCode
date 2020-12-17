using System.Text;

namespace AdventOfCode.Solutions
{
    public class Year2015Day10 : Solution
    {
        public override string Part1(string input)
        {
            string currStr = input.Replace("\n", "");
            for (int i = 0; i < 40; i++)
            {
                StringBuilder newStr = new StringBuilder();
                
                for (int j = 0; j < currStr.Length; j++)
                {
                    int repetitions = 1;
                    while (j < currStr.Length - 1 && currStr[j + 1] == currStr[j])
                    {
                        repetitions++;
                        j++;
                    }

                    newStr.Append(repetitions);
                    newStr.Append(currStr[j]);
                }
                    
                currStr = newStr.ToString();
            }
            return currStr.Length.ToString();
        }

        public override string Part2(string input)
        {
            string currStr = input.Replace("\n", "");
            for (int i = 0; i < 50; i++)
            {
                StringBuilder newStr = new StringBuilder();
                
                for (int j = 0; j < currStr.Length; j++)
                {
                    int repetitions = 1;
                    while (j < currStr.Length - 1 && currStr[j + 1] == currStr[j])
                    {
                        repetitions++;
                        j++;
                    }

                    newStr.Append(repetitions);
                    newStr.Append(currStr[j]);
                }
                    
                currStr = newStr.ToString();
            }
            return currStr.Length.ToString();
        }
    }
}
