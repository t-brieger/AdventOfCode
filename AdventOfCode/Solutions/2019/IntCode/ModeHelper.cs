using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions.IntCode
{
    public static class ModeHelper
    {
        /// <param name="param">The raw value of the parameter</param>
        /// <param name="mode">The mode</param>
        /// <param name="writeParam">True if the param is to be written to, false otherwise</param>
        /// <param name="mem">The program's current memory</param>
        /// <returns>the value of parameters in a given mode if the parameter is to be read from, or the address to write to otherwise.</returns>
        public static int getValue(int param, int mode, Dictionary<int, int> mem, bool writeParam = false)
        {
            if (writeParam && mode == 1)
                throw new ArgumentException("Output parameters may not be in immediate mode.");
            if (mode >= 2 || mode < 0)
                throw new ArgumentException("Modes other than 0 and 1 are not supported at this time.");

            if (!writeParam)
            {
                if (mode == 0)
                    return mem.ContainsKey(param) ? mem[param] : 0;
                return param;
            }

            return param;
        }
    }
}