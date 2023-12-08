using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCsvParser.Types
{
    /// <summary>
    /// Helps to create detailed error messages. Used by CsvLineParser internally.
    /// </summary>
    public class CsvError
    {
        public int Index { get; }
        public string Message { get; }

        public CsvError(int index, string message)
        {
            Index = index;
            Message = message;
        }

        public override string ToString()
            => $"Error at index {Index}: {Message}";
    }
}
