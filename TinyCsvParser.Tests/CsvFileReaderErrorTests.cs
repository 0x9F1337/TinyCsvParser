using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.Exceptions;
using TinyCsvParser.Readers;
using TinyCsvParser.Readers.MultiReaders;

namespace TinyCsvParser.Tests
{
    public class CsvFileReaderErrorTests
    {
        private CsvParserOptions _options;

        [SetUp]
        public void Setup()
        {
            _options = new CsvParserOptions()
            {
                CommentLine = "#",
                EscapeCharacter = '"',
                SplitCharacter = ';',
                ThrowOnParseError = true
            };
        }

        [Test]
        public void InvalidNumberOfEscapesTest()
        {
            /* content:
example;"data;123
example;"data";"123
             */

            string filePath = @"D:\BadFile.InvalidNumberOfEscapes.txt";

            Assert.Throws<CsvParseErrorException>( () =>
            {
                using ( CsvFileReader reader = new CsvFileReader( filePath, _options ) )
                {
                    foreach ( var fields in reader.ReadLines() )
                    {
                    }
                }
            } );
        }
    }
}
