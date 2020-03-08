using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Solutions._2015
{
    //I honestly think this is one of the worst puzzles in all of adventofcode, because you can only bruteforce it - though that may be related to it being the 3rd ever, so I'll forgive it
    class Year2015Day04 : Solution
    {
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            
            return sBuilder.ToString();
        }

        public override string Part1(string input)
        {
            input = input.Trim();
            using (MD5 md5Hash = MD5.Create())
            {
                long i = 0;
                while (GetMd5Hash(md5Hash, input + i).Substring(0, 5) != "00000")
                    i++;
                return i.ToString();
            }
        }

        public override string Part2(string input)
        {
            input = input.Trim();
            using (MD5 md5Hash = MD5.Create())
            {
                long i = 0;
                while (GetMd5Hash(md5Hash, input + i).Substring(0, 6) != "000000")
                    i++;
                return i.ToString();
            }
        }
    }
}
