﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions;

public class Year2017Day16 : Solution
{
    public override string Part1(string input)
    {
        /*
        int length = 5;
        input = "s1,x3/4,pe/b";
        /*/
        const int length = 16;
        //*/

        Node<byte> head = new(0);
        Node<byte> originalHead = head;
        for (byte i = 1; i < length; i++)
        {
            head.next = new Node<byte>(i);
            head = head.next;
        }

        head.next = originalHead;
        head = originalHead;

        foreach (string instruction in input.Split(','))
        {
            originalHead = head;
            switch (instruction[0])
            {
                case 's':
                {
                    int spinSize = Int32.Parse(instruction[1..]);
                    for (int i = 0; i < length - spinSize; i++)
                        head = head.next;
                    break;
                }
                case 'x':
                {
                    byte register0 = Byte.Parse(instruction[1..].Split('/', 2)[0]);
                    byte register1 = Byte.Parse(instruction[1..].Split('/', 2)[1]);

                    for (int i = 0; i < register0; i++)
                        head = head.next;
                    Node<byte> register0Node = head;
                    head = originalHead;
                    for (int i = 0; i < register1; i++)
                        head = head.next;
                    (register0Node.value, head.value) = (head.value, register0Node.value);
                    head = originalHead;
                    break;
                }
                case 'p':
                {
                    byte swap0 = (byte)(instruction[1..].Split('/', 2)[0][0] - 'a');
                    byte swap1 = (byte)(instruction[1..].Split('/', 2)[1][0] - 'a');

                    for (int i = 0; i < length; i++)
                    {
                        if (head.value == swap0)
                            head.value = swap1;
                        else if (head.value == swap1)
                            head.value = swap0;

                        head = head.next;
                    }

                    break;
                }
                default:
                    return "unrecognized instruction: " + instruction;
            }
        }

        return StringifyLinkedList(head, length);
    }

    private static string StringifyLinkedList(Node<byte> head, int l)
    {
        StringBuilder sb = new();
        for (int i = 0; i < l; i++)
        {
            sb.Append((char)(head.value + 'a'));
            head = head.next;
        }

        return sb.ToString();
    }

    public override string Part2(string input)
    {
        Node<byte> head = new(0);
        Node<byte> originalHead = head;
        for (byte i = 1; i < 16; i++)
        {
            head.next = new Node<byte>(i);
            head = head.next;
        }

        head.next = originalHead;
        head = originalHead;

        List<string> seenProgs = new(50);

        for (int j = 0;; j++)
        {
            string s = StringifyLinkedList(head, 16);

            if (seenProgs.Contains(s))
                if (j != 0)
                    return seenProgs[1_000_000_000 % j];

            seenProgs.Add(s);

            foreach (string instruction in input.Split(','))
            {
                originalHead = head;
                switch (instruction[0])
                {
                    case 's':
                    {
                        int spinSize = Int32.Parse(instruction[1..]);
                        for (int i = 0; i < 16 - spinSize; i++)
                            head = head.next;
                        break;
                    }
                    case 'x':
                    {
                        byte register0 = Byte.Parse(instruction[1..].Split('/', 2)[0]);
                        byte register1 = Byte.Parse(instruction[1..].Split('/', 2)[1]);

                        for (int i = 0; i < register0; i++)
                            head = head.next;
                        Node<byte> register0Node = head;
                        head = originalHead;
                        for (int i = 0; i < register1; i++)
                            head = head.next;
                        (register0Node.value, head.value) = (head.value, register0Node.value);
                        head = originalHead;
                        break;
                    }
                    case 'p':
                    {
                        byte swap0 = (byte)(instruction[1..].Split('/', 2)[0][0] - 'a');
                        byte swap1 = (byte)(instruction[1..].Split('/', 2)[1][0] - 'a');

                        for (int i = 0; i < 16; i++)
                        {
                            if (head.value == swap0)
                                head.value = swap1;
                            else if (head.value == swap1)
                                head.value = swap0;

                            head = head.next;
                        }

                        break;
                    }
                    default:
                        return "unrecognized instruction: " + instruction;
                }
            }
        }
    }

    private class Node<T>
    {
        public Node<T> next;
        public T value;

        public Node(T v)
        {
            this.value = v;
        }
    }
}