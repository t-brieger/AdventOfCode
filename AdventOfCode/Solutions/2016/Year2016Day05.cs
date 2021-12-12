using System.Security.Cryptography;
using System.Text;
using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.CPU;
using eightCharString = System.ValueTuple<char, char, char, char, char, char, char, System.ValueTuple<char>>;

namespace AdventOfCode.Solutions
{
    public class Year2016Day05 : Solution
    {
        private static string GetMd5Hash(HashAlgorithm md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new();

            foreach (byte b in data) sBuilder.Append(b.ToString("x2"));

            return sBuilder.ToString();
        }

        public override string Part1(string input)
        {
            return null;

            using MD5 md5Hash = MD5.Create();

            string output = "";


            for (int i = 0; output.Length < 8; i++)
            {
                string h = GetMd5Hash(md5Hash, input + i);
                if (h[..5] == "00000")
                    output += h[5];
            }

            return output;
        }

        private static eightCharString StrToTuple(string s)
        {
            return new eightCharString(s[0], s[1], s[2], s[3], s[4], s[5], s[6], new ValueTuple<char>(s[7]));
        }

        private static void Part2Kernel(Index1D ix, ArrayView<byte> data, eightCharString constant)
        {
            uint i;
            uint[] s =
            {
                7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5,
                9, 14, 20, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 6, 10, 15, 21, 6, 10, 15, 21, 6,
                10, 15, 21, 6, 10, 15, 21
            };
            uint[] K =
            {
                0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
                0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
                0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
                0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
                0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
                0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
                0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
                0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
                0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
                0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
                0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
                0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
                0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
                0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
                0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
                0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
            };

            uint a0 = 0x67452301;
            uint b0 = 0xefcdab89;
            uint c0 = 0x98badcfe;
            uint d0 = 0x10325476;

            byte[] message = new byte[64];
            message[0] = (byte)constant.Item1;
            message[1] = (byte)constant.Item2;
            message[2] = (byte)constant.Item3;
            message[3] = (byte)constant.Item4;
            message[4] = (byte)constant.Item5;
            message[5] = (byte)constant.Item6;
            message[6] = (byte)constant.Item7;
            message[7] = (byte)constant.Rest.Item1;
            byte j = 8;
            Index1D oldIx = ix;
            
            if (ix == 0)
            {
                j = 9;
                message[8] = (byte)'0';
            }

            while (ix > 0)
            {
                message[j++] = (byte)((ix % 10) + '0');
                ix /= 10;
            }

            ix = oldIx;
            
            byte numLength = (byte)(j - 8);

            j--;
            // reverse the number
            for (int l = 0; l < numLength / 2 + (numLength % 2 == 0 ? 0 : 1); l++)
                (message[8 + l], message[j - l]) = (message[j - l], message[8 + l]);

            // message is now 8 bytes of seed, then numLength bytes of the string-ified int "ix".
            // fill it up to 64 bytes (or rather, 56, and then 8 for original message length)
            for (int l = 8 + numLength; l < 63; l++)
                message[l] = 0;
            message[63] = (byte)(8 + numLength);

            data[ix] = (byte)((int)ix + 5); //(message[8]);
        }

        public override string Part2(string input)
        {
            // never needs above 26m iterations in all the inputs ive checked - lets be very safe and say 30.
            int numberOfHashes = 50; //30_000_000;

            using Context context = Context.CreateDefault();
            // TODO figure out when to use which kind of accelerator
            using Accelerator a = context.CreateCPUAccelerator(0);

            Action<Index1D, ArrayView<byte>, eightCharString> kern =
                a.LoadAutoGroupedStreamKernel<Index1D, ArrayView<byte>, eightCharString>(Part2Kernel);

            using MemoryBuffer1D<byte, Stride1D.Dense> buff = a.Allocate1D<byte>(numberOfHashes);

            kern((Index1D)buff.Length, buff.View, StrToTuple(input));

            a.Synchronize();
            Thread.Sleep(1000);

            byte[] data = buff.GetAsArray1D();

            char[] output = { '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' };

            for (int i = 0; i < data.Length; i++)
                if (data[i] != 0 /*&& output[data[i] >> 8] == '\0'*/)
                    continue; //output[data[i] >> 4] = ((byte)(data[i] & 0xf)).ToString("x2")[^1];

            return output[0].ToString();

            /*using MD5 md5Hash = MD5.Create();

            char[] output = { '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' };

            // just here so we don't do "output.ALl(c => c != '\0')" every loop - it gets expensive quick.
            int done = 0;
            
            for (int i = 0; done < 8; i++)
            {
                string h = GetMd5Hash(md5Hash, input + i);
                if (h[..5] == "00000" && h[5] is >= '0' and <= '7' &&output[h[5] - '0'] == '\0')
                {
                    done++;
                    output[h[5] - '0'] = h[6];
                }
            }

            return new string(output); */
        }
    }
}