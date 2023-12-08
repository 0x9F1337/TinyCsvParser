using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCsvParser.Types
{
    public enum CsvLineState
    {
        None,
        Parsed,
        Ignore,
        Errors
    }
}
