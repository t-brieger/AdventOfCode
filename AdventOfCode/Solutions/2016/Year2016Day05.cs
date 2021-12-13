using System;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime.Cuda;
using ILGPU.Runtime.OpenCL;

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

        private static readonly ImmutableArray<int> s = ImmutableArray.Create(7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17,
            22, 7, 12, 17, 22, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 4, 11, 16, 23, 4, 11, 16, 23, 4,
            11, 16, 23, 4, 11, 16, 23, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21);

        private static readonly ImmutableArray<uint> K = ImmutableArray.Create<uint>(
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
        );

        private static uint leftRotate(uint x, int c)
        {
            return (x << c) | (x >> (32 - c));
        }

        private static void Part2Kernel(Index1D ix, ArrayView<byte> data, ulong constant)
        {
            // basically transpiled verbatim from https://en.wikipedia.org/wiki/MD5#Pseudocode
            
            byte[] message = new byte[64];
            message[0] = (byte) ((constant & 0xFF00000000000000L) >> 56);
            message[1] = (byte) ((constant & 0x00FF000000000000L) >> 48);
            message[2] = (byte) ((constant & 0x0000FF0000000000L) >> 40);
            message[3] = (byte) ((constant & 0x000000FF00000000L) >> 32);
            message[4] = (byte) ((constant & 0x00000000FF000000L) >> 24);
            message[5] = (byte) ((constant & 0x0000000000FF0000L) >> 16);
            message[6] = (byte) ((constant & 0x000000000000FF00L) >> 8);
            message[7] = (byte) ((constant & 0x00000000000000FFL) >> 0);
            byte j = 8;
            Index1D oldIx = ix;

            if (ix == 0)
            {
                j = 9;
                message[8] = (byte) '0';
            }

            while (ix > 0)
            {
                message[j++] = (byte) ((ix % 10) + '0');
                ix /= 10;
            }

            ix = oldIx;

            byte numLength = (byte) (j - 8);

            j--;
            // reverse the number
            for (int l = 0; l < numLength / 2 + (numLength % 2 == 0 ? 0 : 1); l++)
                (message[8 + l], message[j - l]) = (message[j - l], message[8 + l]);

            // message is now 8 bytes of seed, then numLength bytes of the string-ified int "ix".
            // fill it up to 64 bytes (or rather, 56, and then 8 for original message length)
            for (int l = 8 + numLength; l < 63; l++)
                message[l] = 0;
            message[8 + numLength] = 0x80;
            message[56] = (byte) (64 + numLength * 8);

            uint a0 = 0x67452301;
            uint b0 = 0xefcdab89;
            uint c0 = 0x98badcfe;
            uint d0 = 0x10325476;

            uint[] M = new uint[16];
            for (int k = 0; k < 16; ++k)
            {
                int baseIx = k * 4;
                M[k] = (uint) (message[baseIx] | (message[baseIx + 1] << 8) |
                               (message[baseIx + 2] << 16) | (message[baseIx + 3] << 24));
            }

            uint A = a0, B = b0, C = c0, D = d0, F = 0, g = 0;

            for (uint k = 0; k < 64; ++k)
            {
                if (k <= 15)
                {
                    F = (B & C) | (~B & D);
                    g = k;
                }
                else if (k >= 16 && k <= 31)
                {
                    F = (D & B) | (~D & C);
                    g = ((5 * k) + 1) % 16;
                }
                else if (k >= 32 && k <= 47)
                {
                    F = B ^ C ^ D;
                    g = ((3 * k) + 5) % 16;
                }
                else if (k >= 48)
                {
                    F = C ^ (B | ~D);
                    g = (7 * k) % 16;
                }

                uint dtemp = D;
                D = C;
                C = B;
                B += leftRotate((A + F + K[(int)k] + M[g]), s[(int)k]);
                A = dtemp;
            }

            a0 += A;
            if ((0xF0FFFF & a0) == 0)
            {
                data[ix] = (byte) (((0xF0000 & a0) >> 12) | ((0xF0000000 & a0) >> 28));
            }
            else
                data[ix] = 0xff;
        }

        public override string Part2(string input)
        {
            // never needs above 26m iterations in all the inputs ive checked - lets be very safe and say 30.
            int numberOfHashes = 30_000_000;

            using Context context = Context.CreateDefault();
            Accelerator a = context.CreateCPUAccelerator(0);
            try
            {
                a = context.CreateCudaAccelerator(0);
            }
            catch (Exception e)
            {
                try
                {
                    a = context.CreateCLAccelerator(0);
                }
                catch (Exception e2)
                {
                    // if this doesn't work it just stays a CPUAccelerator
                }
            }

            Action<Index1D, ArrayView<byte>, ulong> kern =
                a.LoadAutoGroupedStreamKernel<Index1D, ArrayView<byte>, ulong>(Part2Kernel);

            using MemoryBuffer1D<byte, Stride1D.Dense> buff = a.Allocate1D<byte>(numberOfHashes);

            ulong ul =
                ((ulong) input[0] << 56) |
                ((ulong) input[1] << 48) |
                ((ulong) input[2] << 40) |
                ((ulong) input[3] << 32) |
                ((ulong) input[4] << 24) |
                ((ulong) input[5] << 16) |
                ((ulong) input[6] <<  8) |
                ((ulong) input[7] <<  0);
            
            kern((Index1D) buff.Length, buff.View, ul);

            a.Synchronize();
            Thread.Sleep(1000);

            byte[] data = buff.GetAsArray1D();

            char[] output = {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0'};

            for (int i = 0; i < data.Length; i++)
            {
                if ((data[i] & 0xF0) > 0x70)
                    continue;
                byte ix = (byte) ((data[i] & 0xF0) >> 4);
                byte val = (byte) (data[i] & 0x0F);
                if (output[ix] == '\0')
                    output[ix] = val.ToString("x")[0];
            }

            a.Dispose();

            return new string(output);

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