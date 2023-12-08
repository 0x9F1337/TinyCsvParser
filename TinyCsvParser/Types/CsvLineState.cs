using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCsvParser.Types
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
