using System;
using System.Collections.Generic;
using AngleSharp.Css;

namespace AdventOfCode.Solutions;

public class Year2022Day07 : Solution
{
    private static int getDirSize(string dir, Dictionary<string, List<string>> DirContents,
        Dictionary<string, int> FileSizes)
    {
        int size = 0;
        foreach (string content in DirContents[dir])
        {
            if (FileSizes.ContainsKey(dir + content))
                size += FileSizes[dir + content];
            else
                size += getDirSize(dir + content + '/', DirContents, FileSizes);
        }

        return size;
    }
    
    public override string Part1(string input)
    {
        Dictionary<string, List<string>> DirContents = new Dictionary<string, List<string>>();
        Dictionary<string, int> FileSizes = new Dictionary<string, int>();

        string[] lines = input.Split('\n');

        string currentDir = "";
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.StartsWith("$ cd "))
            {
                string cd = line.Substring("$ cd ".Length);
                currentDir = currentDir + cd;
                if (currentDir[^1] != '/')
                    currentDir += '/';
                if (currentDir.EndsWith("../"))
                {
                    currentDir = currentDir.Substring(0, currentDir.Length - 4);
                    while (currentDir[^1] != '/')
                        currentDir = currentDir.Substring(0, currentDir.Length - 1);
                }
                continue;
            }

            if (line == "$ ls")
            {
                DirContents[currentDir] = new List<string>();
                while (i < lines.Length - 1 && lines[i + 1][0] != '$')
                {
                    string[] parts = lines[++i].Split(' ');
                    if (int.TryParse(parts[0], out int res))
                        FileSizes.Add(currentDir + parts[1], res);

                    DirContents[currentDir].Add(parts[1]);
                }
            }
            
        }

        int s = 0;
        foreach (string x in DirContents.Keys)
        {
            int size = getDirSize(x, DirContents, FileSizes);
            if (size <= 100000)
                s += size;
        }
        
        return s.ToString();
    }

    public override string Part2(string input)
    {
        Dictionary<string, List<string>> DirContents = new Dictionary<string, List<string>>();
        Dictionary<string, int> FileSizes = new Dictionary<string, int>();

        string[] lines = input.Split('\n');

        string currentDir = "";
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.StartsWith("$ cd "))
            {
                string cd = line.Substring("$ cd ".Length);
                currentDir = currentDir + cd;
                if (currentDir[^1] != '/')
                    currentDir += '/';
                if (currentDir.EndsWith("../"))
                {
                    currentDir = currentDir.Substring(0, currentDir.Length - 4);
                    while (currentDir[^1] != '/')
                        currentDir = currentDir.Substring(0, currentDir.Length - 1);
                }
                continue;
            }

            if (line == "$ ls")
            {
                DirContents[currentDir] = new List<string>();
                while (i < lines.Length - 1 && lines[i + 1][0] != '$')
                {
                    string[] parts = lines[++i].Split(' ');
                    if (int.TryParse(parts[0], out int res))
                        FileSizes.Add(currentDir + parts[1], res);

                    DirContents[currentDir].Add(parts[1]);
                }
            }
            
        }

        int spaceFree = 70000000 - getDirSize("/", DirContents, FileSizes);
        int sizeNeeded = 30000000 - spaceFree;

        int smallest = int.MaxValue;
        foreach (string x in DirContents.Keys)
        {
            int size = getDirSize(x, DirContents, FileSizes);
            if (size < smallest && size >= sizeNeeded)
                smallest = size;
        }
        
        return smallest.ToString();
    }
}
