using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.Types;

namespace TinyCsvParser.Readers
{
    public class CsvLineParser
    {
        public bool Success => State == CsvLineState.Parsed;
        public string RawLine { get; private set; }
        public CsvLineState State { get; private set; }
        public List<string> ParsedFields => _parsedFields;

        private List<string> _parsedFields;
        private readonly CsvParserOptions _options;
        private readonly List<CsvError> _errors;

        public CsvLineParser( string line, CsvParserOptions options )
        {
            RawLine = line;
            _options = options;
            _errors = new List<CsvError>();

            State = TryParse();
        }

        private CsvLineState TryParse()
        {
            if ( RawLine.StartsWith( _options.CommentLine ) )
            {
                return CsvLineState.Ignore;
            }

            _parsedFields = new List<string>();
            StringBuilder currentFieldString = new StringBuilder();

            bool inEscapedSegment = false;  // When a data segment is surrounded by escape
            bool inEscapedSequence = false; // When a sequence within a data segment is surrounded by escape
            bool mustEscapeNext = false;    // Makes sure to escape the next character, this will occur when double-escapes are used.
                                            // Will work automatically, do not touch this.

            CsvCharacterType lastCharacterType = CsvCharacterType.None;
            int numberOfEscapedCharacters = 0;

            // Escape rules:
            // 1. Escape MUST start right at the beginning of a segment if necessary.
            // 2. Escape MUST end right before next data segment.
            // 3. Sequences within a data segment can only be escaped when current segment is Escaped.
            // 4. Escaped sequences must be surrounded by double escape characters.

            for ( int i = 0; i < RawLine.Length; i++ )
            {
                char currentChar = RawLine[ i ];
                var characterType = GetCharacterType( currentChar );
                var nextCharacterType = GetCharacterTypeAt( RawLine, i + 1 );

                if ( mustEscapeNext )
                {
                    characterType = CsvCharacterType.Regular;
                    mustEscapeNext = false;
                }

                switch ( characterType )
                {
                    case CsvCharacterType.Regular:
                        currentFieldString.Append( currentChar );
                        break;

                    case CsvCharacterType.Split:
                        if ( inEscapedSequence || inEscapedSegment )
                        {
                            characterType = CsvCharacterType.Regular;
                            currentFieldString.Append( currentChar );
                        }
                        else
                        {
                            _parsedFields.Add( currentFieldString.ToString() );
                            currentFieldString.Clear();
                        }
                        break;

                    case CsvCharacterType.Escape:
                        numberOfEscapedCharacters++;
                        if ( lastCharacterType == CsvCharacterType.None || lastCharacterType == CsvCharacterType.Split || nextCharacterType == CsvCharacterType.Split )
                        {
                            inEscapedSegment = !inEscapedSegment;
                            break;
                        }

                        if ( !inEscapedSegment )
                        {
                            _errors.Add( new CsvError( i, "sequence escapes must be within an escaped segment." ) );
                            return CsvLineState.Errors;
                        }

                        if ( nextCharacterType != CsvCharacterType.Escape )
                        {
                            _errors.Add( new CsvError( i, "sequence escapes must be marked with double-escape characters." ) );
                            return CsvLineState.Errors;
                        }

                        inEscapedSequence = !inEscapedSequence;
                        mustEscapeNext = true;

                        break;
                }

                lastCharacterType = characterType;
            }

            if ( ( numberOfEscapedCharacters & 1 ) == 1 )
            {
                _errors.Add( new CsvError( -1, "potential data corruption as the total number of escape characters looks bad." ) );
                return CsvLineState.Errors;
            }

            if ( currentFieldString.Length > 0 )
            {
                _parsedFields.Add( currentFieldString.ToString() );
            }

            return CsvLineState.Parsed;
        }

        private CsvCharacterType GetCharacterType( char characterType )
        {
            if ( characterType == _options.SplitCharacter )
                return CsvCharacterType.Split;
            else if ( characterType == _options.EscapeCharacter )
                return CsvCharacterType.Escape;
            else
                return CsvCharacterType.Regular;
        }

        private CsvCharacterType GetCharacterTypeAt( string line, int index )
        {
            if ( index >= line.Length )
                return CsvCharacterType.Regular;

            return GetCharacterType( line[ index ] );
        }

        public List<CsvError> GetErrors()
            => _errors;
    }
}
