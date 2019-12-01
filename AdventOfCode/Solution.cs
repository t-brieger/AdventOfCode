using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode
{
    public abstract class Solution
    {
        public abstract String Part1(string input);
        public abstract String Part2(string input);
    }
}