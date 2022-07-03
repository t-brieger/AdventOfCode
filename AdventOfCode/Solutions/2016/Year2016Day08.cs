using System;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions;

public class Year2016Day08 : Solution
{
    private bool[,] GetScreenFromInstructions(string[][] instructions, int width, int height)
    {
        bool[,] display = new bool[width, height];

        foreach (string[] s in instructions)
        {
            if (s[0] == "rect")
            {
                string[] args = s[1].Split('x');
                int w = int.Parse(args[0]);
                int h = int.Parse(args[1]);

                for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    display[x, y] = true;
            }
            else
            {
                int amount = int.Parse(s[^1]);
                if (s[2][0] == 'x')
                {
                    int col = int.Parse(s[2][2..]);
                    bool[] newCol = new bool[display.GetLength(1)];
                    for (int i = 0; i < newCol.Length; i++)
                        newCol[i] = display[col, (newCol.Length + i - amount) % newCol.Length];
                    for (int i = 0; i < newCol.Length; i++)
                        display[col, i] = newCol[i];
                }
                else
                {
                    int row = int.Parse(s[2][2..]);
                    bool[] newRow = new bool[display.GetLength(0)];
                    for (int i = 0; i < newRow.Length; i++)
                        newRow[i] = display[(newRow.Length + i - amount) % newRow.Length, row];
                    for (int i = 0; i < newRow.Length; i++)
                        display[i, row] = newRow[i];
                }
            }
        }

        return display;
    }

    public override string Part1(string input)
    {
        string[][] instructions = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(' '))
            .ToArray();

        bool[,] display = GetScreenFromInstructions(instructions, 50, 6);

        int count = 0;
        for (int i = 0; i < display.GetLength(0); i++)
        for (int j = 0; j < display.GetLength(1); j++)
            if (display[i, j])
                count++;

        return count.ToString();
    }

    public override string Part2(string input)
    {
        string[][] instructions = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(' '))
            .ToArray();

        bool[,] display = GetScreenFromInstructions(instructions, 50, 6);

        StringBuilder sb = new StringBuilder("\n");
        
        for (int y = 0; y < display.GetLength(1); y++)
        {
            for (int x = 0; x < display.GetLength(0); x++)
            {
                sb.Append(display[x, y] ? '#' : ' ');
            }

            sb.Append('\n');
        }
        
        return sb.ToString();
    }
}