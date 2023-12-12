using TinyCsvParser.Readers;
using TinyCsvParser.Types;

namespace TinyCsvParser.Tests
{
    [TestOf( typeof( CsvSingleLineTests ) )]
    public class CsvSingleLineTests
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
        public void TestSimple()
        {
            string csv = "example;data;123";

            CsvLineParser parser = new CsvLineParser( csv, _options );

            Assert.That( parser.State, Is.EqualTo( CsvLineState.Parsed ) );
            Assert.That( parser.ParsedFields.Count, Is.EqualTo( 3 ) );
            Assert.That( parser.ParsedFields[ 0 ], Is.EqualTo( "example" ) );
            Assert.That( parser.ParsedFields[ 1 ], Is.EqualTo( "data" ) );
            Assert.That( parser.ParsedFields[ 2 ], Is.EqualTo( "123" ) );

            Assert.Pass();
        }

        [Test]
        public void TestSimpleWithBlankEnding()
        {
            string csv = "example;data;123;";

            CsvLineParser parser = new CsvLineParser( csv, _options );

            Assert.That( parser.State, Is.EqualTo( CsvLineState.Parsed ) );
            Assert.That( parser.ParsedFields.Count, Is.EqualTo( 3 ) );
            Assert.That( parser.ParsedFields[ 0 ], Is.EqualTo( "example" ) );
            Assert.That( parser.ParsedFields[ 1 ], Is.EqualTo( "data" ) );
            Assert.That( parser.ParsedFields[ 2 ], Is.EqualTo( "123" ) );

            Assert.Pass();
        }

        [Test]
        public void TestEscapedSegment()
        {
            string csv = @"example;""data"";123";

            CsvLineParser parser = new CsvLineParser( csv, _options );

            Assert.That( parser.State, Is.EqualTo( CsvLineState.Parsed ) );
            Assert.That( parser.ParsedFields.Count, Is.EqualTo( 3 ) );
            Assert.That( parser.ParsedFields[ 0 ], Is.EqualTo( "example" ) );
            Assert.That( parser.ParsedFields[ 1 ], Is.EqualTo( "data" ) );
            Assert.That( parser.ParsedFields[ 2 ], Is.EqualTo( "123" ) );

            Assert.Pass();
        }

        [Test]
        public void TestEscapedSegmentWithSeparator()
        {
            string csv = @"example;""da;ta"";123";

            CsvLineParser parser = new CsvLineParser( csv, _options );

            Assert.That( parser.State, Is.EqualTo( CsvLineState.Parsed ) );
            Assert.That( parser.ParsedFields.Count, Is.EqualTo( 3 ) );
            Assert.That( parser.ParsedFields[ 0 ], Is.EqualTo( "example" ) );
            Assert.That( parser.ParsedFields[ 1 ], Is.EqualTo( "da;ta" ) );
            Assert.That( parser.ParsedFields[ 2 ], Is.EqualTo( "123" ) );

            Assert.Pass();
        }

        [Test]
        public void TestEscapedSequence()
        {
            string csv = @"example;""da """"42"""" ta"";123";

            CsvLineParser parser = new CsvLineParser( csv, _options );

            Assert.That( parser.State, Is.EqualTo( CsvLineState.Parsed ) );
            Assert.That( parser.ParsedFields.Count, Is.EqualTo( 3 ) );
            Assert.That( parser.ParsedFields[ 0 ], Is.EqualTo( "example" ) );
            Assert.That( parser.ParsedFields[ 1 ], Is.EqualTo( @"da ""42"" ta" ) );
            Assert.That( parser.ParsedFields[ 2 ], Is.EqualTo( "123" ) );

            Assert.Pass();
        }

        [Test]
        public void TestEverything()
        {
            string csv = @"""ex""""a""""mple"";""da """"42"""" ta"";""1;2"""";""""3"";test";

            CsvLineParser parser = new CsvLineParser( csv, _options );

            Assert.That( parser.State, Is.EqualTo( CsvLineState.Parsed ) );
            Assert.That( parser.ParsedFields.Count, Is.EqualTo( 4 ) );
            Assert.That( parser.ParsedFields[ 0 ], Is.EqualTo( @"ex""a""mple" ) );
            Assert.That( parser.ParsedFields[ 1 ], Is.EqualTo( @"da ""42"" ta" ) );
            Assert.That( parser.ParsedFields[ 2 ], Is.EqualTo( @"1;2"";""3" ) );
            Assert.That( parser.ParsedFields[ 3 ], Is.EqualTo( "test" ) );

            Assert.Pass();
        }
    }
}