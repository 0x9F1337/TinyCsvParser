using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCsvParser.Readers
{
    public class CsvParserOptions
    {
        public char SplitCharacter { get; set; }
        public char EscapeCharacter { get; set; }
        public string CommentLine { get; set; }
        public bool ThrowOnParseError { get; set; }

        public static CsvParserOptions Default => new CsvParserOptions()
        {
            SplitCharacter = ',',
            CommentLine = "#",
            EscapeCharacter = '"',
            ThrowOnParseError = true
        };
    }
}
