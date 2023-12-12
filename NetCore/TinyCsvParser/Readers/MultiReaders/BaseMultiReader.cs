using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.Exceptions;
using TinyCsvParser.Types;

namespace TinyCsvParser.Readers.MultiReaders
{
    /// <summary>
    /// Base class that utilizes core CSV parser. Inherit from this to create multi-readers.
    /// </summary>
    public class BaseMultiReader
    {
        /// <summary>
        /// Your parser options.
        /// </summary>
        public CsvParserOptions ParserOptions { get; set; }

        /// <summary>
        /// Collects parsing errors during adventure.
        /// </summary>
        /// 
        public StringBuilder Errors { get; set; }
        
        /// <summary>
        /// Current line number. Advances automatically.
        /// </summary>
        public int LineNumber { get; private set; }

        public bool HasErrors => Errors.Length > 0;

        /// <summary>
        /// Self explanatory.
        /// </summary>
        /// <param name="options">If options is null, Standard configuration will be used.</param>
        public BaseMultiReader(CsvParserOptions options)
        {
            if (options == null) options = CsvParserOptions.Default;

            Errors = new StringBuilder();
            ParserOptions = options;
        }

        /// <summary>
        /// Utilizes CsvLineParser. Exception is only thrown when CsvParserOptions.ThrowOnParseError is true.
        /// </summary>
        /// <param name="line">Csv Line.</param>
        /// <returns>Csv Fields generated from given csv line or null, when given csv-line should be ignored due to comment identification or an unexpected state.</returns>
        /// <exception cref="CsvParseErrorException">Only when CsvParserOptions.ThrowOnParseError is true.</exception>
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

        /// <summary>
        /// Refreshes data.
        /// </summary>
        public virtual void Reset()
        {
            LineNumber = 0;
            Errors.Clear();
        }
    }
}
