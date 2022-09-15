using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions;

public class Year2016Day19 : Solution
{
    private class Node
    {
        public Node Next;
        public int value;

        public Node(int val)
        {
            value = val;
        }
    }

    public override string Part1(string input)
    {
        Node head = new Node(1);
        Node first = head;

        int elfCount = int.Parse(input);

        for (int i = 2; i <= elfCount; i++)
        {
            Node current = new Node(i);
            head.Next = current;
            head = current;
        }

        head.Next = first;
        head = head.Next;

        while (head.Next != head)
        {
            head.Next = head.Next.Next;
            head = head.Next;
        }

        return head.value.ToString();
    }

    public override string Part2(string input)
    {
        Node head = new Node(1);
        Node first = head;

        int elfCount = int.Parse(input);

        for (int i = 2; i <= elfCount; i++)
        {
            Node current = new Node(i);
            head.Next = current;
            head = current;
        }

        head.Next = first;
        head = head.Next;

        Node beforeHalfway = head;

        int count = elfCount;
        int headIndex = 0;
        int beforeHwIndex = 0;

        while (count > 1)
        {
            int toDelete = (headIndex + count / 2) % count;
            int offset = toDelete - beforeHwIndex;
            if (offset < 0)
                offset += count;

            for (int i = 0; i < (offset - 1 + count) % count; i++)
                beforeHalfway = beforeHalfway.Next;

            beforeHalfway.Next = beforeHalfway.Next.Next;
            count--;

            if (headIndex > toDelete)
                headIndex--;
            
            beforeHwIndex = toDelete - 1;
            if (beforeHwIndex < 0)
                beforeHwIndex += count;

            head = head.Next;
            headIndex++;
            headIndex %= count;
        }

        return head.value.ToString();
    }
}
