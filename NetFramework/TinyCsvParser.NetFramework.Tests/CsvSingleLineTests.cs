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
    public class CsvSingleLineTests
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
        public void TestSimple()
        {
            string csv = "example;data;123";

            CsvLineParser parser = new CsvLineParser( csv, _options );

            Assert.AreEqual<CsvLineState>( parser.State, CsvLineState.Parsed );
            Assert.AreEqual<int>( parser.ParsedFields.Count, 3 );
            Assert.AreEqual<string>( parser.ParsedFields[ 0 ],  "example" );
            Assert.AreEqual<string>( parser.ParsedFields[ 1 ], "data" );
            Assert.AreEqual<string>( parser.ParsedFields[ 2 ], "123" );
        }

        [TestMethod]
        public void TestSimpleWithBlankEnding()
        {
            string csv = "example;data;123;";

            CsvLineParser parser = new CsvLineParser( csv, _options );

            Assert.AreEqual<CsvLineState>( parser.State, CsvLineState.Parsed );
            Assert.AreEqual<int>( parser.ParsedFields.Count, 3 );
            Assert.AreEqual<string>( parser.ParsedFields[ 0 ], "example" );
            Assert.AreEqual<string>( parser.ParsedFields[ 1 ], "data" );
            Assert.AreEqual<string>( parser.ParsedFields[ 2 ], "123" );
        }

        [TestMethod]
        public void TestEscapedSegment()
        {
            string csv = @"example;""data"";123";

            CsvLineParser parser = new CsvLineParser( csv, _options );

            Assert.AreEqual<CsvLineState>( parser.State, CsvLineState.Parsed );
            Assert.AreEqual<int>( parser.ParsedFields.Count, 3 );
            Assert.AreEqual<string>( parser.ParsedFields[ 0 ], "example" );
            Assert.AreEqual<string>( parser.ParsedFields[ 1 ], "data" );
            Assert.AreEqual<string>( parser.ParsedFields[ 2 ], "123" );
        }

        [TestMethod]
        public void TestEscapedSegmentWithSeparator()
        {
            string csv = @"example;""da;ta"";123";

            CsvLineParser parser = new CsvLineParser( csv, _options );

            Assert.AreEqual<CsvLineState>( parser.State, CsvLineState.Parsed );
            Assert.AreEqual<int>( parser.ParsedFields.Count, 3 );
            Assert.AreEqual<string>( parser.ParsedFields[ 0 ], "example" );
            Assert.AreEqual<string>( parser.ParsedFields[ 1 ], "da;ta" );
            Assert.AreEqual<string>( parser.ParsedFields[ 2 ], "123" );
        }

        [TestMethod]
        public void TestEscapedSequence()
        {
            string csv = @"example;""da """"42"""" ta"";123";

            CsvLineParser parser = new CsvLineParser( csv, _options );

            Assert.AreEqual<CsvLineState>( parser.State, CsvLineState.Parsed );
            Assert.AreEqual<int>( parser.ParsedFields.Count, 3 );
            Assert.AreEqual<string>( parser.ParsedFields[ 0 ], "example" );
            Assert.AreEqual<string>( parser.ParsedFields[ 1 ], @"da ""42"" ta" );
            Assert.AreEqual<string>( parser.ParsedFields[ 2 ], "123" );
        }

        [TestMethod]
        public void TestEverything()
        {
            string csv = @"""ex""""a""""mple"";""da """"42"""" ta"";""1;2"""";""""3"";test";

            CsvLineParser parser = new CsvLineParser( csv, _options );

            Assert.AreEqual<CsvLineState>( parser.State, CsvLineState.Parsed );
            Assert.AreEqual<int>( parser.ParsedFields.Count, 4 );
            Assert.AreEqual<string>( parser.ParsedFields[ 0 ], @"ex""a""mple" );
            Assert.AreEqual<string>( parser.ParsedFields[ 1 ], @"da ""42"" ta" );
            Assert.AreEqual<string>( parser.ParsedFields[ 2 ], @"1;2"";""3" );
            Assert.AreEqual<string>( parser.ParsedFields[ 3 ], "test" );
        }
    }
}
