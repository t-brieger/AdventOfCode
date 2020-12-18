using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2015Day11 : Solution
    {
        private void increaseString(char[] str)
        {
            bool carry = false;
            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (i == str.Length - 1 || carry)
                {
                    str[i]++;
                    str[i] = str[i] == ('z' + 1) ? 'a' : str[i];
                    carry = str[i] == 'a';
                }
            }
        }

        private bool validatePass(char[] s)
        {
            bool meetsCond1 = false;
            for (int i = 0; i < s.Length - 2; i++)
            {
                if (s[i] == s[i + 1] - 1 && s[i] == s[i + 2] - 2)
                    meetsCond1 = true;
            }

            if (!meetsCond1)
                return false;

            if (s.Any(c => c == 'i' || c == 'o' || c == 'l'))
                return false;

            int pairs = 0;
            for (int i = 0; i < s.Length - 1; i++)
            {
                if (s[i] == s[i + 1])
                {
                    i++;
                    pairs++;
                }
            }

            if (pairs < 2)
                return false;

            return true;
        }
        
        public override string Part1(string input)
        {
            char[] pass = input.Split("\n")[0].ToCharArray();
            while (!validatePass(pass))
                increaseString(pass);
            return new string(pass);
        }

        public override string Part2(string input)
        {
            char[] pass = Part1(input).ToCharArray();
            increaseString(pass);
            while (!validatePass(pass))
                increaseString(pass);
            return new string(pass);
        }
    }
}
