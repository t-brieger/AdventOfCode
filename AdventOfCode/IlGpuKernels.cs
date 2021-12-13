﻿using System.Collections.Immutable;
// ReSharper disable InconsistentNaming

namespace AdventOfCode;

public static class IlGpuKernels
{
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
    
    public static uint LeftRotate(uint x, int c)
    {
        return (x << c) | (x >> (32 - c));
    }
    
    public static uint KernelFirstMd5Int(byte[] message)
    {
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
            else if (k <= 31)
            {
                F = (D & B) | (~D & C);
                g = ((5 * k) + 1) % 16;
            }
            else if (k <= 47)
            {
                F = B ^ C ^ D;
                g = ((3 * k) + 5) % 16;
            }
            else
            {
                F = C ^ (B | ~D);
                g = (7 * k) % 16;
            }

            uint dtemp = D;
            D = C;
            C = B;
            B += LeftRotate((A + F + K[(int)k] + M[g]), s[(int)k]);
            A = dtemp;
        }

        a0 += A;
        return a0;
    }
}