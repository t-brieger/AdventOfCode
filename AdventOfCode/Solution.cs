using System.IO;

namespace AdventOfCode
{
    // ReSharper disable once InconsistentNaming
    public abstract class Solution
    {
        public StreamWriter VisSw = null;
        public void WriteVisual(object o)
        {
            // if our streamwriter hasn't been initialized to anything other than null, this is a no-op.
            this.VisSw?.WriteLine(o);
        }

        // for those few puzzles where leading/trailing whitespace matters and fixing it would be very annoying
        public string RawInput;
        
        public abstract string Part1(string input);
        public abstract string Part2(string input);
    }
}