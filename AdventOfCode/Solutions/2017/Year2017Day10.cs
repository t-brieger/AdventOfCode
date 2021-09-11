using System;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions
{
    class Year2017Day10 : Solution
    {
        public static byte[] GetKnotHash(string s)
        {
            //*
            byte[] lengths = new byte[Encoding.ASCII.GetByteCount(s) + 5];
            Array.Copy(Encoding.ASCII.GetBytes(s), 0, lengths, 0, lengths.Length - 5);
            lengths[^5] = 17;
            lengths[^4] = 31;
            lengths[^3] = 73;
            lengths[^2] = 47;
            lengths[^1] = 23;
            //*/
            /*
            byte[] lengths = new byte[Encoding.ASCII.GetByteCount(s)];
            Array.Copy(Encoding.ASCII.GetBytes(s), 0, lengths, 0, lengths.Length);
            //*/

            byte[] numbers = new byte[511];
            for (short i = 0; i < numbers.Length; i++)
                numbers[i] = (byte)(i % 256);

            // pos + len - 256
            int pos = 0;
            int skip = 0;

            for (int i = 0; i < 64; i++)
                foreach (int length in lengths)
                {
                    Array.Copy(numbers, 0, numbers, 256, 255);

                    Array.Reverse(numbers, pos, length);

                    int affectedInSecondPart = pos + length - 256;
                    if (affectedInSecondPart > 0)
                        Array.Copy(numbers, 256, numbers, 0, affectedInSecondPart);

                    pos += length + skip;
                    skip++;
                    pos %= 256;
                }

            byte[] denseHash = new byte[16];

            for (int i = 0; i < denseHash.Length; i++)
            {
                denseHash[i] = numbers[i * 16];
                for (int j = 1; j < 16; j++) denseHash[i] ^= numbers[i * 16 + j];
            }

            return denseHash;
        }

        public override string Part1(string input)
        {
            int[] lengths = input.Split(',').Select(Int32.Parse).ToArray();
            byte[] numbers = new byte[511];
            for (short i = 0; i < numbers.Length; i++)
                numbers[i] = (byte)(i % 256);

            // pos + len - 256
            short pos = 0;
            short skip = 0;

            foreach (int length in lengths)
            {
                Array.Copy(numbers, 0, numbers, 256, 255);

                Array.Reverse(numbers, pos, length);

                int affectedInSecondPart = pos + length - 256;
                if (affectedInSecondPart > 0)
                    Array.Copy(numbers, 256, numbers, 0, affectedInSecondPart);

                pos += (short)(length + skip);
                skip++;
                pos %= 256;
            }

            return (numbers[0] * numbers[1]).ToString();
        }

        public override string Part2(string input)
        {
            byte[] denseHash = GetKnotHash(input.Trim());

            return BitConverter.ToString(denseHash).Replace("-", "").ToLowerInvariant();
        }
    }
}