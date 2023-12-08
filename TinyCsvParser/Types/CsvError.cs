using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCsvParser.Types
{
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
