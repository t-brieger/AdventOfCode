using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions;

public class Year2016Day13 : Solution
{
    private bool isWall(int x, int y, int favNumber)
    {
        if (x < 0 || y < 0)
            return true;
        
        int tmp = (x + 3 + 2 * y) * x + (y + 1) * y;
        tmp += favNumber;

        bool parity = false;
        for (int i = 0; i < 32; i++)
        {
            if ((tmp & (1 << i)) > 0)
                parity = !parity;
        }

        return parity;
    }
    
    public override string Part1(string input)
    {
        int favNumber = int.Parse(input);

        PriorityQueue<(int, int y, int), int> moves = new();
        HashSet<int> alreadyVisited = new HashSet<int>();
        
        moves.Enqueue((1, 1, 0), 0);

        if (isWall(31, 39, favNumber))
            throw new ArgumentException();

        while (true)
        {
            (int x, int y, int weight) = moves.Dequeue();

            if (x == 31 && y == 39)
                return weight.ToString();

            if (alreadyVisited.Contains((x << 16) | y))
                continue;

            alreadyVisited.Add((x << 16) | y);
            
            if (!isWall(x + 1, y, favNumber))
                moves.Enqueue((x + 1, y, weight+1), weight+1);
            if (!isWall(x - 1, y, favNumber))
                moves.Enqueue((x - 1, y, weight+1), weight+1);
            if (!isWall(x, y + 1, favNumber))
                moves.Enqueue((x, y + 1, weight+1), weight+1);
            if (!isWall(x, y - 1, favNumber))
                moves.Enqueue((x, y - 1, weight+1), weight+1);
        }
    }

    public override string Part2(string input)
    {
        int favNumber = int.Parse(input);

        PriorityQueue<(int, int y, int), int> moves = new();
        HashSet<int> alreadyVisited = new HashSet<int>();
        
        moves.Enqueue((1, 1, 0), 0);

        while (true)
        {
            (int x, int y, int weight) = moves.Dequeue();

            if (weight > 50)
                // break, not continue, because by definition of a prioqueue, all the others are over 50 too
                break;
            
            if (alreadyVisited.Contains((x << 16) | y))
                continue;

            alreadyVisited.Add((x << 16) | y);
            
            if (!isWall(x + 1, y, favNumber))
                moves.Enqueue((x + 1, y, weight+1), weight+1);
            if (!isWall(x - 1, y, favNumber))
                moves.Enqueue((x - 1, y, weight+1), weight+1);
            if (!isWall(x, y + 1, favNumber))
                moves.Enqueue((x, y + 1, weight+1), weight+1);
            if (!isWall(x, y - 1, favNumber))
                moves.Enqueue((x, y - 1, weight+1), weight+1);
        }

        return alreadyVisited.Count.ToString();
    }
}
