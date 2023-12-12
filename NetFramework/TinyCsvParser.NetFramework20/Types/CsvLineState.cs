using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCsvParser.NetFramework20.Types
{
    /// <summary>
    /// State of each line. Set by CsvLineParser internally. Can be used to identify errors.
    /// </summary>
    public enum CsvLineState
    {
        None,
        Parsed,
        Ignore,
        Errors
    }
}
