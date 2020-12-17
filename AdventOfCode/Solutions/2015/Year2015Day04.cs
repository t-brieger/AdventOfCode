using System;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Solutions
{
    //I honestly think this is one of the worst puzzles in all of adventofcode, because you can only bruteforce it - though that may be related to it being the 3rd ever, so I'll forgive it
    class Year2015Day04 : Solution
    {
        private static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            foreach (byte b in data)
            {
                sBuilder.Append(b.ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public override string Part1(string input)
        {
            input = input.Trim();
            using MD5 md5Hash = MD5.Create();
            long i = 0;
            string currentHash;
            while ((currentHash = GetMd5Hash(md5Hash, input + i++)).Substring(0, 5) != "00000")
            {
#if VIS
                if (i % 13 != 0) //13 so that the last digit changes
                    continue;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{i:00000000}: {currentHash}");
                Console.CursorLeft = 0;
#endif
            }
#if VIS
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{i:00000000}: {currentHash}");
            Console.ResetColor();
#endif
            return i.ToString();
        }

        public override string Part2(string input)
        {
            input = input.Trim();
            using MD5 md5Hash = MD5.Create();
            long i = 0;
            string currentHash;
            while ((currentHash = GetMd5Hash(md5Hash, input + i++)).Substring(0, 6) != "000000")
            {
#if VIS
                if (i % 133 != 0)
                    continue;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{i:00000000}: {currentHash}");
                Console.CursorLeft = 0;
#endif
            }
#if VIS
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{i:00000000}: {currentHash}\n\n");
            Console.ResetColor();
#endif
            return i.ToString();
        }
    }
}
