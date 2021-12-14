using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2018Day13 : Solution
{
    public override string Part1(string input)
    {
        input = this.rawInput;
            
        //y, then x
        char[][] map = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(line => line.ToCharArray())
            .ToArray();

        //TURNNUMBER, DIRECTION, X, Y, ID
        byte[][] carts;

        {
            List<byte[]> cartList =
                new();

            for (byte x = 0; x < map.Length; x++)
            for (byte y = 0; y < map[x].Length; y++)
                switch (map[y][x])
                {
                    case '^':
                        cartList.Add(new byte[] { 0, (byte)Directions.UP, x, y, (byte)cartList.Count });
                        map[y][x] = '|';
                        break;
                    case 'v':
                        cartList.Add(new byte[] { 0, (byte)Directions.DOWN, x, y, (byte)cartList.Count });
                        map[y][x] = '|';
                        break;
                    case '<':
                        cartList.Add(new byte[] { 0, (byte)Directions.LEFT, x, y, (byte)cartList.Count });
                        map[y][x] = '-';
                        break;
                    case '>':
                        cartList.Add(new byte[] { 0, (byte)Directions.RIGHT, x, y, (byte)cartList.Count });
                        map[y][x] = '-';
                        break;
                }

            carts = cartList.ToArray();
        }

        //carts = carts.OrderBy(tuple => (tuple.Item4 << 8) | tuple.Item3).ToArray();

        while (true)
        {
            carts = carts.OrderBy(tuple => (tuple[3] << 8) | tuple[2]).ToArray();
            foreach (byte[] cart in carts)
            {
                switch (map[cart[3]][cart[2]])
                {
                    case '|':
                    case '-':
                        break;
                    case '\\':
                        cart[1] = cart[1] switch
                        {
                            (byte)Directions.LEFT => (byte)Directions.UP,
                            (byte)Directions.UP => (byte)Directions.LEFT,
                            (byte)Directions.RIGHT => (byte)Directions.DOWN,
                            (byte)Directions.DOWN => (byte)Directions.RIGHT,
                            _ => cart[1]
                        };
                        break;
                    case '/':
                        cart[1] = cart[1] switch
                        {
                            (byte)Directions.LEFT => (byte)Directions.DOWN,
                            (byte)Directions.UP => (byte)Directions.RIGHT,
                            (byte)Directions.RIGHT => (byte)Directions.UP,
                            (byte)Directions.DOWN => (byte)Directions.LEFT,
                            _ => cart[1]
                        };
                        break;
                    case '+':
                        switch (cart[0])
                        {
                            case 0 when cart[1] == (byte)Directions.DOWN:
                                cart[1] = (byte)Directions.RIGHT;
                                break;
                            case 0:
                                cart[1]--;
                                break;
                            case 2 when cart[1] == (byte)Directions.RIGHT:
                                cart[1] = (byte)Directions.DOWN;
                                break;
                            case 2:
                                cart[1]++;
                                break;
                        }

                        cart[0]++;
                        cart[0] = (byte)(cart[0] % 3);

                        break;

                    default:
                        Console.WriteLine("malformed input - " + map[cart[3]][cart[2]]);
                        break;
                }

                switch (cart[1])
                {
                    case (byte)Directions.UP:
                        cart[3]--;
                        break;
                    case (byte)Directions.DOWN:
                        cart[3]++;
                        break;
                    case (byte)Directions.LEFT:
                        cart[2]--;
                        break;
                    case (byte)Directions.RIGHT:
                        cart[2]++;
                        break;
                }


                if (carts.Any(otherCart =>
                        otherCart[2] == cart[2] && otherCart[3] == cart[3] && otherCart[4] != cart[4]))
                    return $"{cart[2]},{cart[3]}";
            }
        }
    }

    public override string Part2(string input)
    {
        input = rawInput;
        
        //y, then x
        char[][] map = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(line => line.ToCharArray())
            .ToArray();

        //TURNNUMBER, DIRECTION, X, Y, ID
        byte[][] carts;

        {
            List<byte[]> cartList =
                new();

            for (byte x = 0; x < map.Length; x++)
            for (byte y = 0; y < map[x].Length; y++)
                switch (map[y][x])
                {
                    case '^':
                        cartList.Add(new byte[] { 0, (byte)Directions.UP, x, y, (byte)cartList.Count });
                        map[y][x] = '|';
                        break;
                    case 'v':
                        cartList.Add(new byte[] { 0, (byte)Directions.DOWN, x, y, (byte)cartList.Count });
                        map[y][x] = '|';
                        break;
                    case '<':
                        cartList.Add(new byte[] { 0, (byte)Directions.LEFT, x, y, (byte)cartList.Count });
                        map[y][x] = '-';
                        break;
                    case '>':
                        cartList.Add(new byte[] { 0, (byte)Directions.RIGHT, x, y, (byte)cartList.Count });
                        map[y][x] = '-';
                        break;
                }

            carts = cartList.ToArray();
        }

        //carts = carts.OrderBy(tuple => (tuple.Item4 << 8) | tuple.Item3).ToArray();

        while (true)
        {
            carts = carts.OrderBy(tuple => (tuple[3] << 8) | tuple[2]).ToArray();
            foreach (byte[] cart in carts.Where(cart => cart[4] != 255))
            {
                switch (map[cart[3]][cart[2]])
                {
                    case '|':
                    case '-':
                        break;
                    case '\\':
                        cart[1] = cart[1] switch
                        {
                            (byte)Directions.LEFT => (byte)Directions.UP,
                            (byte)Directions.UP => (byte)Directions.LEFT,
                            (byte)Directions.RIGHT => (byte)Directions.DOWN,
                            (byte)Directions.DOWN => (byte)Directions.RIGHT,
                            _ => cart[1]
                        };
                        break;
                    case '/':
                        cart[1] = cart[1] switch
                        {
                            (byte)Directions.LEFT => (byte)Directions.DOWN,
                            (byte)Directions.UP => (byte)Directions.RIGHT,
                            (byte)Directions.RIGHT => (byte)Directions.UP,
                            (byte)Directions.DOWN => (byte)Directions.LEFT,
                            _ => cart[1]
                        };
                        break;
                    case '+':
                        switch (cart[0])
                        {
                            case 0 when cart[1] == (byte)Directions.DOWN:
                                cart[1] = (byte)Directions.RIGHT;
                                break;
                            case 0:
                                cart[1]--;
                                break;
                            case 2 when cart[1] == (byte)Directions.RIGHT:
                                cart[1] = (byte)Directions.DOWN;
                                break;
                            case 2:
                                cart[1]++;
                                break;
                        }

                        cart[0]++;
                        cart[0] = (byte)(cart[0] % 3);

                        break;

                    default:
                        Console.WriteLine("malformed input - " + map[cart[3]][cart[2]]);
                        break;
                }

                switch (cart[1])
                {
                    case (byte)Directions.UP:
                        cart[3]--;
                        break;
                    case (byte)Directions.DOWN:
                        cart[3]++;
                        break;
                    case (byte)Directions.LEFT:
                        cart[2]--;
                        break;
                    case (byte)Directions.RIGHT:
                        cart[2]++;
                        break;
                }


                foreach (byte[] otherCart in carts.Where(otherCart =>
                             otherCart[2] == cart[2] && otherCart[3] == cart[3] && otherCart[4] != cart[4] &&
                             otherCart[4] != 255))
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

    private enum Directions : byte
    {
        DOWN,
        LEFT,
        UP,
        RIGHT
    }
}