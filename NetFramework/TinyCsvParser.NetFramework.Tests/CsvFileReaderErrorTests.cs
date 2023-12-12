using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinyCsvParser.NetFramework20.Exceptions;
using TinyCsvParser.NetFramework20.Readers;
using TinyCsvParser.NetFramework20.Readers.MultiReaders;

namespace TinyCsvParser.NetFramework48.Tests
{
    [TestClass]
    public class CsvFileReaderErrorTests
    {
        private CsvParserOptions _options;

        [TestInitialize]
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

        [TestMethod]
        public void InvalidNumberOfEscapesTest()
        {
            /* content:
example;"data;123
example;"data";"123
             */

            string filePath = @"D:\BadFile.InvalidNumberOfEscapes.txt";

            Assert.ThrowsException<CsvParseErrorException>( () =>
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
