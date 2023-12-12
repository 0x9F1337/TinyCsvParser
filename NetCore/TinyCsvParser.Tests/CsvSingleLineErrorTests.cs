using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.Readers;
using TinyCsvParser.Types;

namespace TinyCsvParser.Tests
{
    [TestOf( typeof( CsvSingleLineErrorTests ) )]
    public class CsvSingleLineErrorTests
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
            string data = @"bla;""test;blub";
            string data2 = @"bla;""test"";""lala";

            CsvLineParser csvLineParser1 = new CsvLineParser( data, _options );
            CsvLineParser csvLineParser2 = new CsvLineParser( data2, _options );

            Assert.That( csvLineParser1.State, Is.EqualTo( CsvLineState.Errors ) );
            Assert.That( csvLineParser2.State, Is.EqualTo( CsvLineState.Errors ) );

            Assert.That( csvLineParser1.Success, Is.False );
            Assert.That( csvLineParser2.Success, Is.False );

            var dataCsvErrors1 = csvLineParser1.GetErrors();
            var dataCsvErrors2 = csvLineParser2.GetErrors();

            Assert.That( dataCsvErrors1.Any( i => i.Message.Contains( "potential data corruption" ) ), Is.True );
            Assert.That( dataCsvErrors2.Any( i => i.Message.Contains( "potential data corruption" ) ), Is.True );
        }

        [Test]
        public void EscapeSequenceInUnescapedSegmentTest()
        {
            string data = @"bla;t""""e""""st;blub";

            CsvLineParser csvLineParser = new CsvLineParser( data, _options );

            Assert.That( csvLineParser.State, Is.EqualTo( CsvLineState.Errors ) );
            Assert.That( csvLineParser.Success, Is.False );
            Assert.That( csvLineParser.GetErrors().Any( i => i.Message.Contains( "sequence escapes must be within an escaped segment" ) ), Is.True );
        }

        [Test]
        public void EscapedSequenceNotDoubleCharactersTest()
        {
            string data = @"bla;""t""e""st"";blub";

            CsvLineParser csvLineParser = new CsvLineParser( data, _options );

            Assert.That( csvLineParser.State, Is.EqualTo( CsvLineState.Errors ) );
            Assert.That( csvLineParser.Success, Is.False );
            Assert.That( csvLineParser.GetErrors().Any( i => i.Message.Contains( "sequence escapes must be marked with double-escape characters" ) ), Is.True );
        }
    }
}
