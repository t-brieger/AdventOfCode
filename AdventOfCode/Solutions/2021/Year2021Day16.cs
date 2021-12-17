using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2021Day16 : Solution
{
    private class Packet
    {
        public byte Version;
        public byte Type;
    }

    private class LiteralValuePacket : Packet
    {
        public long Value;
    }

    private class OperatorPacket : Packet
    {
        public List<Packet> SubPackets;
    }

    private static Packet ParsePacketHeader(bool[] b, ref int ix)
    {
        int startIx = ix;

        byte v = 0;
        for (; ix < startIx + 3; ix++)
            v = (byte) ((v << 1) | (b[ix] ? 1 : 0));

        byte t = 0;
        for (; ix < startIx + 6; ix++)
            t = (byte) ((t << 1) | (b[ix] ? 1 : 0));

        Packet p = t == 4 ? ParseLiteralBody(b, ref ix) : ParseOperatorBody(b, ref ix);

        p.Type = t;
        p.Version = v;

        return p;
    }

    private static OperatorPacket ParseOperatorBody(bool[] b, ref int ix)
    {
        OperatorPacket p = new OperatorPacket
        {
            SubPackets = new List<Packet>()
        };

        bool lengthType = b[ix++];

        int length = 0;

        for (int i = 0; i < (lengthType ? 11 : 15); i++)
        {
            length = (length << 1) | (b[ix++] ? 1 : 0);
        }

        int ixStartSubs = ix;
        
        if (!lengthType)
            while (ix < ixStartSubs + length)
                p.SubPackets.Add(ParsePacketHeader(b, ref ix));
        else
            while (p.SubPackets.Count < length)
                p.SubPackets.Add(ParsePacketHeader(b, ref ix));

        return p;
    }

    private static LiteralValuePacket ParseLiteralBody(bool[] b, ref int ix)
    {
        // read until it the first bit of the group isnt 1 anymore, then 1 more time

        long val = 0;
        while (b[ix++])
        {
            for (int i = 0; i < 4; i++)
                val = val << 1 | (uint) (b[ix++] ? 1 : 0);
        }

        for (int i = 0; i < 4; i++)
            val = val << 1 | (uint) (b[ix++] ? 1 : 0);

        LiteralValuePacket p = new LiteralValuePacket
        {
            Value = val
        };

        return p;
    }

    private static int AddVersions(Packet p)
    {
        int s = 0;
        s += p.Version;
        if (p is OperatorPacket p2)
        {
            s += p2.SubPackets.Sum(AddVersions);
        }

        return s;
    }
    
    public override string Part1(string input)
    {
        bool[,] hexToBits = {
            {false, false, false, false},
            {false, false, false, true},
            {false, false, true, false},
            {false, false, true, true},
            {false, true, false, false},
            {false, true, false, true},
            {false, true, true, false},
            {false, true, true, true},
            {true, false, false, false},
            {true, false, false, true},
            {true, false, true, false},
            {true, false, true, true},
            {true, true, false, false},
            {true, true, false, true},
            {true, true, true, false},
            {true, true, true, true}
        };

        bool[] bits = new bool[input.Length * 4];

        for (int i = 0; i < bits.Length; i += 4)
        {
            char c = input[i / 4];
            int index;
            if (c is <= '9' and >= '0')
                index = c - '0';
            else
                index = c - 'A' + 10;

            bits[i] = hexToBits[index, 0];
            bits[i + 1] = hexToBits[index, 1];
            bits[i + 2] = hexToBits[index, 2];
            bits[i + 3] = hexToBits[index, 3];
        }

        int ix = 0;

        Packet p = ParsePacketHeader(bits, ref ix);

        return AddVersions(p).ToString();
    }

    private static long GetValue(Packet p)
    {
        if (p is LiteralValuePacket lvp)
            return lvp.Value;
        OperatorPacket op = p as OperatorPacket;
        return op!.Type switch
        {
            0 => op.SubPackets.Aggregate(0L, (sum, packet) => sum + GetValue(packet)),
            1 => op.SubPackets.Aggregate(1L, (product, packet) => product * GetValue(packet)),
            2 => op.SubPackets.Min(GetValue),
            3 => op.SubPackets.Max(GetValue),
            5 => GetValue(op.SubPackets[0]) > GetValue(op.SubPackets[1]) ? 1 : 0,
            6 => GetValue(op.SubPackets[0]) < GetValue(op.SubPackets[1]) ? 1 : 0,
            7 => GetValue(op.SubPackets[0]) == GetValue(op.SubPackets[1]) ? 1 : 0,
            _ => -1
        };
    }

    public override string Part2(string input)
    {
        bool[,] hexToBits = {
            {false, false, false, false},
            {false, false, false, true},
            {false, false, true, false},
            {false, false, true, true},
            {false, true, false, false},
            {false, true, false, true},
            {false, true, true, false},
            {false, true, true, true},
            {true, false, false, false},
            {true, false, false, true},
            {true, false, true, false},
            {true, false, true, true},
            {true, true, false, false},
            {true, true, false, true},
            {true, true, true, false},
            {true, true, true, true}
        };

        bool[] bits = new bool[input.Length * 4];

        for (int i = 0; i < bits.Length; i += 4)
        {
            char c = input[i / 4];
            int index;
            if (c is <= '9' and >= '0')
                index = c - '0';
            else
                index = c - 'A' + 10;

            bits[i] = hexToBits[index, 0];
            bits[i + 1] = hexToBits[index, 1];
            bits[i + 2] = hexToBits[index, 2];
            bits[i + 3] = hexToBits[index, 3];
        }

        int ix = 0;

        Packet p = ParsePacketHeader(bits, ref ix);

        return GetValue(p).ToString();
    }
}