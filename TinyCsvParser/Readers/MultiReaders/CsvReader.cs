using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.Exceptions;

namespace TinyCsvParser.Readers.MultiReaders
{
    public class CsvReader : BaseMultiReader
    {
        private readonly CsvParserOptions _options;
        private readonly IEnumerable<string> _lines;

        public CsvReader(IEnumerable<string> lines, CsvParserOptions options)
            : base(options)
        {
            _lines = lines;
        }

        public IEnumerable<List<string>> ReadLines()
        {
            foreach (var line in _lines)
            {
                var fields = ReadLine(line);

                if (fields == null)
                {
                    continue;
                }

                yield return fields;
            }
        }
    }
}
