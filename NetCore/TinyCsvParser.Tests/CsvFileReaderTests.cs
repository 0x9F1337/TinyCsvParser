using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.Readers;
using TinyCsvParser.Readers.MultiReaders;
using TinyCsvParser.Types;

namespace TinyCsvParser.Tests
{
    [TestOf( typeof( CsvFileReaderTests ) )]
    public class CsvFileReaderTests
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

            using ( CsvFileReader reader = new CsvFileReader(filePath, _options) )
            {
                int line = 0;
                foreach ( var fields in reader.ReadLines() )
                {
                    line++;
                    
                    Assert.That( fields.Count, Is.GreaterThanOrEqualTo( 3 ) );
                    Assert.That( line, Is.EqualTo( reader.LineNumber ) );
                    Assert.That( reader.HasErrors, Is.False );

                    switch ( reader.LineNumber )
                    {
                        case 1:
                            Assert.That( fields[ 0 ], Is.EqualTo( "example" ) );
                            Assert.That( fields[ 1 ], Is.EqualTo( "data" ) );
                            Assert.That( fields[ 2 ], Is.EqualTo( "123" ) );
                            break;

                        case 2:
                            Assert.That( fields[ 0 ], Is.EqualTo( "example" ) );
                            Assert.That( fields[ 1 ], Is.EqualTo( "data" ) );
                            Assert.That( fields[ 2 ], Is.EqualTo( "123" ) );
                            break;

                        case 3:
                            Assert.That( fields[ 0 ], Is.EqualTo( "example" ) );
                            Assert.That( fields[ 1 ], Is.EqualTo( "da;ta" ) );
                            Assert.That( fields[ 2 ], Is.EqualTo( "123" ) );
                            break;

                        case 4:
                            Assert.That( fields[ 0 ], Is.EqualTo( "example" ) );
                            Assert.That( fields[ 1 ], Is.EqualTo( @"da ""42"" ta" ) );
                            Assert.That( fields[ 2 ], Is.EqualTo( "123" ) );
                            break;

                        case 5:
                            Assert.That( fields[ 0 ], Is.EqualTo( @"ex""a""mple" ) );
                            Assert.That( fields[ 1 ], Is.EqualTo( @"da ""42"" ta" ) );
                            Assert.That( fields[ 2 ], Is.EqualTo( @"1;2"";""3" ) );
                            Assert.That( fields[ 3 ], Is.EqualTo( "test" ) );
                            break;

                        default:
                            Assert.Fail( "unexpected line number." );
                            break;
                    }
                }

                Assert.Pass();
            }
        }
    }
}
