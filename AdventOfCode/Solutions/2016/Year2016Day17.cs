using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Solutions;

public class Year2016Day17 : Solution
{
    private static string GetStartOfMd5Hash(HashAlgorithm md5Hash, string input)
    {
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder sBuilder = new();

        sBuilder.Append(data[0].ToString("x2"));
        sBuilder.Append(data[1].ToString("x2"));

        return sBuilder.ToString();
    }

    public override string Part1(string input)
    {
        PriorityQueue<(string, int, int), int> paths = new PriorityQueue<(string, int, int), int>();
        paths.Enqueue(("", 0, 0), 0);

        MD5 md5 = MD5.Create();

        while (true)
        {
            (string path, int x, int y) = paths.Dequeue();

            if (x == 3 && y == 3)
                return path;

            string hashDigits = GetStartOfMd5Hash(md5, input + path);
            
            bool up = hashDigits[0] is >= 'b' and <= 'f';
            bool down = hashDigits[1] is >= 'b' and <= 'f';
            bool left = hashDigits[2] is >= 'b' and <= 'f';
            bool right = hashDigits[3] is >= 'b' and <= 'f';
            
            if (up && y != 0)
                paths.Enqueue((path + 'U', x, y - 1), path.Length + 1);
            
            if (down && y != 3)
                paths.Enqueue((path + 'D', x, y + 1), path.Length + 1);
            
            if (left && x != 0)
                paths.Enqueue((path + 'L', x - 1, y), path.Length + 1);
            
            if (right && x != 3)
                paths.Enqueue((path + 'R', x + 1, y), path.Length + 1);
        }
    }

    public override string Part2(string input)
    {
        List<(string, int, int)> paths = new List<(string, int, int)>();
        paths.Add(("", 0, 0));

        MD5 md5 = MD5.Create();

        int longest = -1;
        
        while (paths.Count > 0)
        {
            (string path, int x, int y) = paths[0];
            paths.RemoveAt(0);

            if (x == 3 && y == 3)
            {
                if (path.Length > longest)
                    longest = path.Length;
                continue;
            }

            string hashDigits = GetStartOfMd5Hash(md5, input + path);
            
            bool up = hashDigits[0] is >= 'b' and <= 'f';
            bool down = hashDigits[1] is >= 'b' and <= 'f';
            bool left = hashDigits[2] is >= 'b' and <= 'f';
            bool right = hashDigits[3] is >= 'b' and <= 'f';
            
            if (up && y != 0)
                paths.Add((path + 'U', x, y - 1));
            
            if (down && y != 3)
                paths.Add((path + 'D', x, y + 1));
            
            if (left && x != 0)
                paths.Add((path + 'L', x - 1, y));
            
            if (right && x != 3)
                paths.Add((path + 'R', x + 1, y));
        }

        return longest.ToString();
    }
}
