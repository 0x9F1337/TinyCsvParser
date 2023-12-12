using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.NetFramework20.Readers;
using TinyCsvParser.NetFramework20.Types;

namespace TinyCsvParser.NetFramework.Tests
{
    [TestClass]
    public class CsvSingleLineErrorTests
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
            string data = @"bla;""test;blub";
            string data2 = @"bla;""test"";""lala";

            CsvLineParser csvLineParser1 = new CsvLineParser( data, _options );
            CsvLineParser csvLineParser2 = new CsvLineParser( data2, _options );

            Assert.AreEqual<CsvLineState>( csvLineParser1.State, CsvLineState.Errors );
            Assert.AreEqual<CsvLineState>( csvLineParser2.State, CsvLineState.Errors );

            Assert.IsFalse( csvLineParser1.Success );
            Assert.IsFalse( csvLineParser2.Success );

            var dataCsvErrors1 = csvLineParser1.GetErrors();
            var dataCsvErrors2 = csvLineParser2.GetErrors();

            Assert.IsTrue( dataCsvErrors1.Any( i => i.Message.Contains( "potential data corruption" ) ) );
            Assert.IsTrue( dataCsvErrors2.Any( i => i.Message.Contains( "potential data corruption" ) ) );
        }

        [TestMethod]
        public void EscapeSequenceInUnescapedSegmentTest()
        {
            string data = @"bla;t""""e""""st;blub";

            CsvLineParser csvLineParser = new CsvLineParser( data, _options );

            Assert.AreEqual<CsvLineState>( csvLineParser.State, CsvLineState.Errors  );
            Assert.IsFalse( csvLineParser.Success );
            Assert.IsTrue( csvLineParser.GetErrors().Any( i => i.Message.Contains( "sequence escapes must be within an escaped segment" ) ) );
        }

        [TestMethod]
        public void EscapedSequenceNotDoubleCharactersTest()
        {
            string data = @"bla;""t""e""st"";blub";

            CsvLineParser csvLineParser = new CsvLineParser( data, _options );

            Assert.AreEqual<CsvLineState>( csvLineParser.State,  CsvLineState.Errors  );
            Assert.IsFalse( csvLineParser.Success );
            Assert.IsTrue( csvLineParser.GetErrors().Any( i => i.Message.Contains( "sequence escapes must be marked with double-escape characters" ) ) );
        }
    }
}
