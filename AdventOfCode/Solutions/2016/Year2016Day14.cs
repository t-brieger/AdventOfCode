using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Solutions;

public class Year2016Day14 : Solution
{
    private static string GetMd5Hash(HashAlgorithm md5Hash, string input)
    {
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder sBuilder = new();

        foreach (byte b in data) sBuilder.Append(b.ToString("x2"));

        return sBuilder.ToString();
    }

    private int DoTheThing(Func<int, string> getHash, string salt)
    {
        int keysGenerated = 0;

        for (int i = 0;; i++)
        {
            string currentHash = getHash(i);
            char triplet = '\0';
            for (int j = 0; j < currentHash.Length - 2; j++)
            {
                if (currentHash[j] == currentHash[j + 1] && currentHash[j] == currentHash[j + 2])
                {
                    triplet = currentHash[j];
                    break;
                }
            }

            if (triplet == '\0')
                continue;

            for (int nextHash = i + 1; nextHash <= i + 1000; nextHash++)
            {
                if (getHash(nextHash).Contains("" + triplet + triplet + triplet + triplet + triplet))
                {
                    keysGenerated++;
                    if (keysGenerated == 64)
                        return i;
                    break;
                }
            }
        }
    }

    public override string Part1(string input)
    {
        using MD5 md5Hash = MD5.Create();

        string[] hashes = new string[1001];
        int[] lastWrites = new int[1001];
        for (int i = 0; i < 1001; i++)
        {
            hashes[i] = null;
            lastWrites[i] = -1;
        }

        string GetHash(int x)
        {
            if (lastWrites[x % 1001] == x)
                return hashes[x % 1001];
            
            string hash = GetMd5Hash(md5Hash, input + x);
            hashes[x % 1001] = hash;
            lastWrites[x % 1001] = x;
            return hash;
        }

        return DoTheThing(GetHash, input).ToString();
    }

    public override string Part2(string input)
    {
        using MD5 md5Hash = MD5.Create();

        string[] hashes = new string[1001];
        int[] lastWrites = new int[1001];
        for (int i = 0; i < 1001; i++)
        {
            hashes[i] = null;
            lastWrites[i] = -1;
        }

        string GetHash(int x)
        {
            if (lastWrites[x % 1001] == x)
                return hashes[x % 1001];
            
            string hash = GetMd5Hash(md5Hash, input + x);
            for (int i = 0; i < 2016; i++)
                hash = GetMd5Hash(md5Hash, hash);
            hashes[x % 1001] = hash;
            lastWrites[x % 1001] = x;
            return hash;
        }

        return DoTheThing(GetHash, input).ToString();
    }
}
