using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2018Day13 : Solution
    {
        private enum DIRECTIONS : byte
        {
            DOWN, LEFT, UP, RIGHT
        }

        public override string Part1(string input)
        {
            //y, then x
            char[][] map = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.ToCharArray()).ToArray();

            //TURNNUMBER, DIRECTION, X, Y, ID
            byte[][] carts;

            {
                List<byte[]> cartList =
                    new List<byte[]>();

                for (byte x = 0; x < map.Length; x++)
                {
                    for (byte y = 0; y < map[x].Length; y++)
                    {
                        if (map[y][x] == '^')
                        {
                            cartList.Add(new byte[] { 0, (byte)DIRECTIONS.UP, x, y, (byte)cartList.Count });
                            map[y][x] = '|';
                        }
                        else if (map[y][x] == 'v')
                        {
                            cartList.Add(new byte[] { 0, (byte)DIRECTIONS.DOWN, x, y, (byte)cartList.Count });
                            map[y][x] = '|';
                        }
                        else if (map[y][x] == '<')
                        {
                            cartList.Add(new byte[] { 0, (byte)DIRECTIONS.LEFT, x, y, (byte)cartList.Count });
                            map[y][x] = '-';
                        }
                        else if (map[y][x] == '>')
                        {
                            cartList.Add(new byte[] { 0, (byte)DIRECTIONS.RIGHT, x, y, (byte)cartList.Count });
                            map[y][x] = '-';
                        }
                    }
                }

                carts = cartList.ToArray();
            }

            //carts = carts.OrderBy(tuple => (tuple.Item4 << 8) | tuple.Item3).ToArray();

            while (true)
            {
                carts = carts.OrderBy(tuple => ((int)tuple[3] << 8) | tuple[2]).ToArray();
                foreach (var cart in carts)
                {
                    switch (map[cart[3]][cart[2]])
                    {
                        case '|':
                        case '-':
                            break;
                        case '\\':
                            if (cart[1] == (byte)DIRECTIONS.LEFT)
                                cart[1] = (byte)DIRECTIONS.UP;
                            else if (cart[1] == (byte)DIRECTIONS.UP)
                                cart[1] = (byte)DIRECTIONS.LEFT;
                            else if (cart[1] == (byte)DIRECTIONS.RIGHT)
                                cart[1] = (byte)DIRECTIONS.DOWN;
                            else if (cart[1] == (byte)DIRECTIONS.DOWN)
                                cart[1] = (byte)DIRECTIONS.RIGHT;
                            break;
                        case '/':
                            if (cart[1] == (byte)DIRECTIONS.LEFT)
                                cart[1] = (byte)DIRECTIONS.DOWN;
                            else if (cart[1] == (byte)DIRECTIONS.UP)
                                cart[1] = (byte)DIRECTIONS.RIGHT;
                            else if (cart[1] == (byte)DIRECTIONS.RIGHT)
                                cart[1] = (byte)DIRECTIONS.UP;
                            else if (cart[1] == (byte)DIRECTIONS.DOWN)
                                cart[1] = (byte)DIRECTIONS.LEFT;
                            break;
                        case '+':
                            if (cart[0] == 0)
                            {
                                if (cart[1] == (byte) DIRECTIONS.DOWN)
                                    cart[1] = (byte) DIRECTIONS.RIGHT;
                                else
                                    cart[1]--;
                            }else if (cart[0] == 2)
                            {
                                if (cart[1] == (byte)DIRECTIONS.RIGHT)
                                    cart[1] = (byte)DIRECTIONS.DOWN;
                                else
                                    cart[1]++;
                            }

                            cart[0]++;
                            cart[0] = (byte) (cart[0] % 3);

                            break;

                        default:
                            Console.WriteLine("malformed input - " + map[cart[3]][cart[2]]);
                            break;
                    }

                    if (cart[1] == (byte)DIRECTIONS.UP)
                        cart[3]--;
                    else if (cart[1] == (byte)DIRECTIONS.DOWN)
                        cart[3]++;
                    else if (cart[1] == (byte)DIRECTIONS.LEFT)
                        cart[2]--;
                    else if (cart[1] == (byte)DIRECTIONS.RIGHT)
                        cart[2]++;


                    foreach (var otherCart in carts)
                    {
                        if ((otherCart[2] == cart[2] && otherCart[3] == cart[3]) && otherCart[4] != cart[4])
                            return $"{cart[2]},{cart[3]}";
                    }
                }
            }
        }

        public override string Part2(string input)
        {
            /*
            input =
                "/>-<\\  \r\n|   |  \r\n| /<+-\\\r\n| | | v\r\n\\>+</ |\r\n  |   ^\r\n  \\<->/";
            //*/

            //y, then x
            char[][] map = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.ToCharArray()).ToArray();

            //TURNNUMBER, DIRECTION, X, Y, ID
            byte[][] carts;

            {
                List<byte[]> cartList =
                    new List<byte[]>();

                for (byte x = 0; x < map.Length; x++)
                {
                    for (byte y = 0; y < map[x].Length; y++)
                    {
                        if (map[y][x] == '^')
                        {
                            cartList.Add(new byte[] { 0, (byte)DIRECTIONS.UP, x, y, (byte)cartList.Count });
                            map[y][x] = '|';
                        }
                        else if (map[y][x] == 'v')
                        {
                            cartList.Add(new byte[] { 0, (byte)DIRECTIONS.DOWN, x, y, (byte)cartList.Count });
                            map[y][x] = '|';
                        }
                        else if (map[y][x] == '<')
                        {
                            cartList.Add(new byte[] { 0, (byte)DIRECTIONS.LEFT, x, y, (byte)cartList.Count });
                            map[y][x] = '-';
                        }
                        else if (map[y][x] == '>')
                        {
                            cartList.Add(new byte[] { 0, (byte)DIRECTIONS.RIGHT, x, y, (byte)cartList.Count });
                            map[y][x] = '-';
                        }
                    }
                }

                carts = cartList.ToArray();
            }

            //carts = carts.OrderBy(tuple => (tuple.Item4 << 8) | tuple.Item3).ToArray();

            while (true)
            {
                carts = carts.OrderBy(tuple => ((int)tuple[3] << 8) | tuple[2]).ToArray();
                foreach (var cart in carts)
                {
                    if (cart[4] == 255)
                        continue;

                    switch (map[cart[3]][cart[2]])
                    {
                        case '|':
                        case '-':
                            break;
                        case '\\':
                            if (cart[1] == (byte)DIRECTIONS.LEFT)
                                cart[1] = (byte)DIRECTIONS.UP;
                            else if (cart[1] == (byte)DIRECTIONS.UP)
                                cart[1] = (byte)DIRECTIONS.LEFT;
                            else if (cart[1] == (byte)DIRECTIONS.RIGHT)
                                cart[1] = (byte)DIRECTIONS.DOWN;
                            else if (cart[1] == (byte)DIRECTIONS.DOWN)
                                cart[1] = (byte)DIRECTIONS.RIGHT;
                            break;
                        case '/':
                            if (cart[1] == (byte)DIRECTIONS.LEFT)
                                cart[1] = (byte)DIRECTIONS.DOWN;
                            else if (cart[1] == (byte)DIRECTIONS.UP)
                                cart[1] = (byte)DIRECTIONS.RIGHT;
                            else if (cart[1] == (byte)DIRECTIONS.RIGHT)
                                cart[1] = (byte)DIRECTIONS.UP;
                            else if (cart[1] == (byte)DIRECTIONS.DOWN)
                                cart[1] = (byte)DIRECTIONS.LEFT;
                            break;
                        case '+':
                            if (cart[0] == 0)
                            {
                                if (cart[1] == (byte)DIRECTIONS.DOWN)
                                    cart[1] = (byte)DIRECTIONS.RIGHT;
                                else
                                    cart[1]--;
                            }
                            else if (cart[0] == 2)
                            {
                                if (cart[1] == (byte)DIRECTIONS.RIGHT)
                                    cart[1] = (byte)DIRECTIONS.DOWN;
                                else
                                    cart[1]++;
                            }

                            cart[0]++;
                            cart[0] = (byte)(cart[0] % 3);

                            break;

                        default:
                            Console.WriteLine("malformed input - " + map[cart[3]][cart[2]]);
                            break;
                    }

                    if (cart[1] == (byte)DIRECTIONS.UP)
                        cart[3]--;
                    else if (cart[1] == (byte)DIRECTIONS.DOWN)
                        cart[3]++;
                    else if (cart[1] == (byte)DIRECTIONS.LEFT)
                        cart[2]--;
                    else if (cart[1] == (byte)DIRECTIONS.RIGHT)
                        cart[2]++;


                    foreach (var otherCart in carts)
                    {
                        if ((otherCart[2] == cart[2] && otherCart[3] == cart[3]) && otherCart[4] != cart[4] && otherCart[4] != 255)
                        {
                            otherCart[4] = 255;
                            cart[4] = 255;
                            break;
                        }
                    }
                }
                if (carts.Count(c => c[4] == 255) == carts.Length - 1)
                {
                    byte[] winningCart = carts.Where(c => c[4] != 255).ToArray()[0];
                    return $"{winningCart[2]},{winningCart[3]}";
                }
            }
        }
    }
}