using System;

namespace AdventOfCode;

// ReSharper disable once InconsistentNaming
public abstract class Solution
{
    // for those few puzzles where leading/trailing whitespace matters and fixing it would be very annoying
    public string rawInput;
    public Action IncreaseBar;
    public Action<int> RescaleBar;
    public Action<int> SetBar;
    public Func<int> GetBarScale;

    public abstract string Part1(string input);
    public abstract string Part2(string input);
}
