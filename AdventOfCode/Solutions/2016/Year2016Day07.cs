using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2016Day07 : Solution
{
    private static bool IsValidIpPart1(string[] ip)
    {
        bool symmetricOutsideOfBrackets = false;
        for (int i = 0; i < ip.Length; i++)
        {
            bool foundSymmetric = false;
            for (int j = 0; j <= ip[i].Length - 4; j++)
            {
                if (ip[i][j] == ip[i][j + 3] && ip[i][j + 1] == ip[i][j + 2] && ip[i][j] != ip[i][j + 1])
                {
                    foundSymmetric = true;
                    break;
                }
            }

            if (foundSymmetric && i % 2 == 1)
                return false;
            if (foundSymmetric && i % 2 == 0)
                symmetricOutsideOfBrackets = true;
        }

        return symmetricOutsideOfBrackets;
    }

    private static bool IsValidIpPart2(string[] ip)
    {
        HashSet<(char, char)> aba = new HashSet<(char, char)>();
        for (int i = 0; i < ip.Length; i += 2)
        {
            for (int j = 0; j <= ip[i].Length - 3; j++)
            {
                if (ip[i][j] == ip[i][j + 2] && ip[i][j + 1] != ip[i][j])
                    aba.Add((ip[i][j], ip[i][j + 1]));
            }
        }
        
        for (int i = 1; i < ip.Length; i += 2)
        {
            foreach ((char a, char b) in aba)
                if (ip[i].Contains("" + b + a + b))
                    return true;
        }

        return false;
    }

    public override string Part1(string input)
    {
        string[][] ips = input.Split('\n').Select(line => line.Split(new[] {']', '['})).ToArray();

        return ips.Count(IsValidIpPart1).ToString();
    }

    public override string Part2(string input)
    {
        string[][] ips = input.Split('\n').Select(line => line.Split(new[] {']', '['})).ToArray();

        return ips.Count(IsValidIpPart2).ToString();
    }
}