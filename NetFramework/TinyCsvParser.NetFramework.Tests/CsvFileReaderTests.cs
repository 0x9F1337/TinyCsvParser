using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.NetFramework20.Readers;
using TinyCsvParser.NetFramework20.Readers.MultiReaders;

namespace TinyCsvParser.NetFramework.Tests
{
    [TestClass]
    public class CsvFileReaderTests
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
        public void TestFile()
        {
            /* File Content:
example;data;123
example;"data";123
example;"da;ta";123
example;"da ""42"" ta";123
"ex""a""mple";"da ""42"" ta";"1;2"";""3";test
             */

            string filePath = @"D:\TestFile.txt";

            using ( CsvFileReader reader = new CsvFileReader( filePath, _options ) )
            {
                int line = 0;
                foreach ( var fields in reader.ReadLines() )
                {
                    line++;

                    Assert.IsTrue( fields.Count >= 3 );
                    Assert.AreEqual<int>( line, reader.LineNumber );
                    Assert.IsFalse( reader.HasErrors );

                    switch ( reader.LineNumber )
                    {
                        case 1:
                            Assert.AreEqual<string>( fields[ 0 ], "example" );
                            Assert.AreEqual<string>( fields[ 1 ], "data" );
                            Assert.AreEqual<string>( fields[ 2 ], "123" );
                            break;

                        case 2:
                            Assert.AreEqual<string>( fields[ 0 ], "example" );
                            Assert.AreEqual<string>( fields[ 1 ], "data" );
                            Assert.AreEqual<string>( fields[ 2 ], "123" );
                            break;

                        case 3:
                            Assert.AreEqual<string>( fields[ 0 ], "example" );
                            Assert.AreEqual<string>( fields[ 1 ], "da;ta" );
                            Assert.AreEqual<string>( fields[ 2 ], "123" );
                            break;

                        case 4:
                            Assert.AreEqual<string>( fields[ 0 ], "example" );
                            Assert.AreEqual<string>( fields[ 1 ], @"da ""42"" ta" );
                            Assert.AreEqual<string>( fields[ 2 ], "123" );
                            break;

                        case 5:
                            Assert.AreEqual<string>( fields[ 0 ], @"ex""a""mple" );
                            Assert.AreEqual<string>( fields[ 1 ], @"da ""42"" ta" );
                            Assert.AreEqual<string>( fields[ 2 ], @"1;2"";""3" );
                            Assert.AreEqual<string>( fields[ 3 ], "test" );
                            break;

                        default:
                            Assert.Fail( "unexpected line number." );
                            break;
                    }
                }
            }
        }
    }
}
