using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2018
{
    public class Year2018Day13 : Solution
    {
        private enum Directions : byte
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
                            cartList.Add(new byte[] { 0, (byte)Directions.UP, x, y, (byte)cartList.Count });
                            map[y][x] = '|';
                        }
                        else if (map[y][x] == 'v')
                        {
                            cartList.Add(new byte[] { 0, (byte)Directions.DOWN, x, y, (byte)cartList.Count });
                            map[y][x] = '|';
                        }
                        else if (map[y][x] == '<')
                        {
                            cartList.Add(new byte[] { 0, (byte)Directions.LEFT, x, y, (byte)cartList.Count });
                            map[y][x] = '-';
                        }
                        else if (map[y][x] == '>')
                        {
                            cartList.Add(new byte[] { 0, (byte)Directions.RIGHT, x, y, (byte)cartList.Count });
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
                foreach (byte[] cart in carts)
                {
                    switch (map[cart[3]][cart[2]])
                    {
                        case '|':
                        case '-':
                            break;
                        case '\\':
                            if (cart[1] == (byte)Directions.LEFT)
                                cart[1] = (byte)Directions.UP;
                            else if (cart[1] == (byte)Directions.UP)
                                cart[1] = (byte)Directions.LEFT;
                            else if (cart[1] == (byte)Directions.RIGHT)
                                cart[1] = (byte)Directions.DOWN;
                            else if (cart[1] == (byte)Directions.DOWN)
                                cart[1] = (byte)Directions.RIGHT;
                            break;
                        case '/':
                            if (cart[1] == (byte)Directions.LEFT)
                                cart[1] = (byte)Directions.DOWN;
                            else if (cart[1] == (byte)Directions.UP)
                                cart[1] = (byte)Directions.RIGHT;
                            else if (cart[1] == (byte)Directions.RIGHT)
                                cart[1] = (byte)Directions.UP;
                            else if (cart[1] == (byte)Directions.DOWN)
                                cart[1] = (byte)Directions.LEFT;
                            break;
                        case '+':
                            if (cart[0] == 0)
                            {
                                if (cart[1] == (byte) Directions.DOWN)
                                    cart[1] = (byte) Directions.RIGHT;
                                else
                                    cart[1]--;
                            }else if (cart[0] == 2)
                            {
                                if (cart[1] == (byte)Directions.RIGHT)
                                    cart[1] = (byte)Directions.DOWN;
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

                    if (cart[1] == (byte)Directions.UP)
                        cart[3]--;
                    else if (cart[1] == (byte)Directions.DOWN)
                        cart[3]++;
                    else if (cart[1] == (byte)Directions.LEFT)
                        cart[2]--;
                    else if (cart[1] == (byte)Directions.RIGHT)
                        cart[2]++;


                    if (carts.Any(otherCart => otherCart[2] == cart[2] && otherCart[3] == cart[3] && otherCart[4] != cart[4]))
                    {
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
                            cartList.Add(new byte[] { 0, (byte)Directions.UP, x, y, (byte)cartList.Count });
                            map[y][x] = '|';
                        }
                        else if (map[y][x] == 'v')
                        {
                            cartList.Add(new byte[] { 0, (byte)Directions.DOWN, x, y, (byte)cartList.Count });
                            map[y][x] = '|';
                        }
                        else if (map[y][x] == '<')
                        {
                            cartList.Add(new byte[] { 0, (byte)Directions.LEFT, x, y, (byte)cartList.Count });
                            map[y][x] = '-';
                        }
                        else if (map[y][x] == '>')
                        {
                            cartList.Add(new byte[] { 0, (byte)Directions.RIGHT, x, y, (byte)cartList.Count });
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
                foreach (byte[] cart in carts.Where(cart => cart[4] != 255))
                {
                    switch (map[cart[3]][cart[2]])
                    {
                        case '|':
                        case '-':
                            break;
                        case '\\':
                            if (cart[1] == (byte)Directions.LEFT)
                                cart[1] = (byte)Directions.UP;
                            else if (cart[1] == (byte)Directions.UP)
                                cart[1] = (byte)Directions.LEFT;
                            else if (cart[1] == (byte)Directions.RIGHT)
                                cart[1] = (byte)Directions.DOWN;
                            else if (cart[1] == (byte)Directions.DOWN)
                                cart[1] = (byte)Directions.RIGHT;
                            break;
                        case '/':
                            if (cart[1] == (byte)Directions.LEFT)
                                cart[1] = (byte)Directions.DOWN;
                            else if (cart[1] == (byte)Directions.UP)
                                cart[1] = (byte)Directions.RIGHT;
                            else if (cart[1] == (byte)Directions.RIGHT)
                                cart[1] = (byte)Directions.UP;
                            else if (cart[1] == (byte)Directions.DOWN)
                                cart[1] = (byte)Directions.LEFT;
                            break;
                        case '+':
                            if (cart[0] == 0)
                            {
                                if (cart[1] == (byte)Directions.DOWN)
                                    cart[1] = (byte)Directions.RIGHT;
                                else
                                    cart[1]--;
                            }
                            else if (cart[0] == 2)
                            {
                                if (cart[1] == (byte)Directions.RIGHT)
                                    cart[1] = (byte)Directions.DOWN;
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

                    if (cart[1] == (byte)Directions.UP)
                        cart[3]--;
                    else if (cart[1] == (byte)Directions.DOWN)
                        cart[3]++;
                    else if (cart[1] == (byte)Directions.LEFT)
                        cart[2]--;
                    else if (cart[1] == (byte)Directions.RIGHT)
                        cart[2]++;


                    foreach (byte[] otherCart in carts.Where(otherCart => otherCart[2] == cart[2] && otherCart[3] == cart[3] && otherCart[4] != cart[4] && otherCart[4] != 255))
                    {
                        otherCart[4] = 255;
                        cart[4] = 255;
                        break;
                    }
                }

                if (carts.Count(c => c[4] == 255) != carts.Length - 1) continue;
                {
                    byte[] winningCart = carts.Where(c => c[4] != 255).ToArray()[0];
                    return $"{winningCart[2]},{winningCart[3]}";
                }
            }
        }
    }
}