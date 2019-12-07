using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions._2017
{
    public class Year2017Day16 : Solution
    {
        private class Node<T>
        {
            public T value;
            public Node<T> next;

            public Node(T v)
            {
                this.value = v;
            }
        }

        public override string Part1(string input)
        {
            /*
            int length = 5;
            input = "s1,x3/4,pe/b";
            /*/
            int length = 16;
            //*/

            var head = new Node<byte>(0);
            var originalHead = head;
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
                if (instruction[0] == 's')
                {
                    int spinSize = int.Parse(instruction.Substring(1));
                    for (int i = 0; i < length - spinSize; i++)
                        head = head.next;
                }else if (instruction[0] == 'x')
                {
                    byte register0 = byte.Parse(instruction.Substring(1).Split('/', 2)[0]);
                    byte register1 = byte.Parse(instruction.Substring(1).Split('/', 2)[1]);

                    for (int i = 0; i < register0; i++)
                        head = head.next;
                    var register0Node = head;
                    head = originalHead;
                    for (int i = 0; i < register1; i++)
                        head = head.next;
                    byte tmp = register0Node.value;
                    register0Node.value = head.value;
                    head.value = tmp;
                    head = originalHead;
                }
                else if (instruction[0] == 'p')
                {
                    byte swap0 = (byte) (instruction.Substring(1).Split('/', 2)[0][0] - 'a');
                    byte swap1 = (byte) (instruction.Substring(1).Split('/', 2)[1][0] - 'a');

                    for (int i = 0; i < length; i++)
                    {
                        if (head.value == swap0)
                            head.value = swap1;
                        else if (head.value == swap1)
                            head.value = swap0;

                        head = head.next;
                    }
                }
                else
                {
                    return "unrecognized instruction: " + instruction;
                }
            }

            return stringifyLinkedList(head, length);
        }

        private static string stringifyLinkedList(Node<byte> head, int l)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < l; i++)
            {
                sb.Append((char)(head.value + 'a'));
                head = head.next;
            }

            return sb.ToString();
        }

        public override string Part2(string input)
        {
            var head = new Node<byte>(0);
            var originalHead = head;
            for (byte i = 1; i < 16; i++)
            {
                head.next = new Node<byte>(i);
                head = head.next;
            }

            head.next = originalHead;
            head = originalHead;

            List<string> seenProgs = new List<string>(50);

            for (int j = 0;; j++)
            {
                var s = stringifyLinkedList(head, 16);

                if (seenProgs.Contains(s))
                    return seenProgs[1_000_000_000 % j];

                seenProgs.Add(s);

                foreach (string instruction in input.Split(','))
                {
                    originalHead = head;
                    if (instruction[0] == 's')
                    {
                        int spinSize = int.Parse(instruction.Substring(1));
                        for (int i = 0; i < 16 - spinSize; i++)
                            head = head.next;
                    }
                    else if (instruction[0] == 'x')
                    {
                        byte register0 = byte.Parse(instruction.Substring(1).Split('/', 2)[0]);
                        byte register1 = byte.Parse(instruction.Substring(1).Split('/', 2)[1]);

                        for (int i = 0; i < register0; i++)
                            head = head.next;
                        var register0Node = head;
                        head = originalHead;
                        for (int i = 0; i < register1; i++)
                            head = head.next;
                        byte tmp = register0Node.value;
                        register0Node.value = head.value;
                        head.value = tmp;
                        head = originalHead;
                    }
                    else if (instruction[0] == 'p')
                    {
                        byte swap0 = (byte) (instruction.Substring(1).Split('/', 2)[0][0] - 'a');
                        byte swap1 = (byte) (instruction.Substring(1).Split('/', 2)[1][0] - 'a');

                        for (int i = 0; i < 16; i++)
                        {
                            if (head.value == swap0)
                                head.value = swap1;
                            else if (head.value == swap1)
                                head.value = swap0;

                            head = head.next;
                        }
                    }
                    else
                    {
                        return "unrecognized instruction: " + instruction;
                    }
                }
            }
        }
    }
}