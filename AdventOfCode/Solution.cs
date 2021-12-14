namespace AdventOfCode;

// ReSharper disable once InconsistentNaming
public abstract class Solution
{
    // for those few puzzles where leading/trailing whitespace matters and fixing it would be very annoying
    public string rawInput;
        
    public abstract string Part1(string input);
    public abstract string Part2(string input);
}