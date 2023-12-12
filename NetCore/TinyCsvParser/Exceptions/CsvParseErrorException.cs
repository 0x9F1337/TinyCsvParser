using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCsvParser.Exceptions
{
    public class CsvParseErrorException : Exception
    {
        public int LineNo { get; set; }
        public CsvParseErrorException( int lineNo,  string message ) 
            : base( message )
        {
            this.LineNo = lineNo;
        }   
    }
}
