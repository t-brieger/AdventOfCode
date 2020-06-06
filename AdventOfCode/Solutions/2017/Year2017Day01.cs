using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2017Day01 : Solution
    {
        public override string Part1(string input)
        {
            LinkedList<char> numbers = new LinkedList<char>(input);

            LinkedListNode<char> currentNumber = numbers.First;

            int sum = 0;
            foreach (char unused in numbers)
            {
                if (currentNumber != null && currentNumber.Value == (currentNumber.Next ?? numbers.First).Value)
                    sum += currentNumber.Value - '0';

                currentNumber = currentNumber?.Next;
            }

            return sum.ToString();
        }

        public override string Part2(string input)
        {
            char[] numbers = input.ToCharArray();

            int sum = numbers.Where((t, i) => t == numbers[(i + numbers.Length / 2) % numbers.Length]).Sum(t => t - '0');

            return sum.ToString();
        }
    }
}