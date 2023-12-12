using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCsvParser.Readers
{
    /// <summary>
    /// Options required for TinyCsvParser to work.
    /// </summary>
    public class CsvParserOptions
    {
        public char SplitCharacter { get; set; }
        public char EscapeCharacter { get; set; }
        public string CommentLine { get; set; }
        public bool ThrowOnParseError { get; set; }

        /// <summary>
        /// Standard CSV parser configuration.<br></br>
        /// Split Character = ,<br></br>
        /// CommentLine = #<br></br>
        /// Escape Character = "<br></br>
        /// Throws on parse error.<br></br>
        /// </summary>
        public static CsvParserOptions Default => new CsvParserOptions()
        {
            SplitCharacter = ',',
            CommentLine = "#",
            EscapeCharacter = '"',
            ThrowOnParseError = true
        };
    }
}
