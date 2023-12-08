using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.Exceptions;
using TinyCsvParser.Types;

namespace TinyCsvParser.Readers.MultiReaders
{
    public class BaseMultiReader
    {
        public CsvParserOptions ParserOptions { get; set; }
        public StringBuilder Errors { get; set; }
        public int LineNumber { get; private set; }
        public bool HasErrors => Errors.Length > 0;

        public BaseMultiReader(CsvParserOptions options)
        {
            if (options == null) options = CsvParserOptions.Default;

            Errors = new StringBuilder();
            ParserOptions = options;
        }

        public List<string> ReadLine(string line)
        {
            LineNumber++;

            CsvLineParser csvLineParser = new CsvLineParser(line, ParserOptions);

            if (csvLineParser.State == CsvLineState.Ignore)
                return null;

            if (csvLineParser.State == CsvLineState.Errors)
            {
                StringBuilder errors = new StringBuilder();

                foreach (var error in csvLineParser.GetErrors())
                    errors.AppendLine(error.ToString());

                errors.Insert(0, $"Csv parsing error(s) in line {LineNumber}:\r\n");

                Errors.Append(errors);

                if (ParserOptions.ThrowOnParseError)
                    throw new CsvParseErrorException(LineNumber, errors.ToString());
            }
            else if (csvLineParser.State == CsvLineState.Parsed)
            {
                return csvLineParser.ParsedFields;
            }

            return null;
        }

        public virtual void Reset()
        {
            LineNumber = 0;
            Errors.Clear();
        }
    }
}
