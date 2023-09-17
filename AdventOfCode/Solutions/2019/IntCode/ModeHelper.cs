using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions.IntCode;

// ReSharper disable once InconsistentNaming
public class ModeHelper
{
    public long relativeBase;

    /// <param name="param">The raw value of the parameter</param>
    /// <param name="mode">The mode</param>
    /// <param name="writeParam">True if the param is to be written to, false otherwise</param>
    /// <param name="mem">The program's current memory</param>
    /// <returns>
    ///     the value of parameters in a given mode if the parameter is to be read from, or the address to write to
    ///     otherwise.
    /// </returns>
    public long GetValue(long param, long mode, Dictionary<long, long> mem, bool writeParam = false)
    {
        if (writeParam && mode == 1)
            throw new ArgumentException("Output parameters may not be in immediate mode.");
        if (mode is >= 3 or < 0)
            throw new ArgumentException("Only modes 0-2 are supported at this time");

        if (!writeParam)
            return mode switch
            {
                0 => mem.ContainsKey(param) ? mem[param] : 0,
                1 => param,
                2 => mem.ContainsKey(this.relativeBase + param) ? mem[this.relativeBase + param] : 0
            };

        if (mode == 0)
            return param;
        return this.relativeBase + param;
    }
}