using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCsvParser.NetFramework20.Exceptions
{
    public class CsvParseErrorException : Exception
    {
        public int LineNo { get; set; }
        public CsvParseErrorException( int lineNo, string message )
            : base( message )
        {
            this.LineNo = lineNo;
        }
    }
}
