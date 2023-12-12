using System;
using System.Collections.Generic;
using System.Text;
using TinyCsvParser.NetFramework20.Types;

namespace TinyCsvParser.NetFramework20.Readers
{
    /// <summary>
    /// Core CSV parser.
    /// </summary>
    public class CsvLineParser
    {
        public bool Success => State == CsvLineState.Parsed;

        /// <summary>
        /// Raw string of CSV line. This line is unparsed and remains original.
        /// </summary>
        public string RawLine { get; private set; }

        /// <summary>
        /// Parse state. This value will be set when a new instance is created.
        /// </summary>
        public CsvLineState State { get; private set; }

        /// <summary>
        /// Parsed CSV fields collection.
        /// </summary>
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

        /// <summary>
        /// Parses given csv line. Internal use only. 
        /// </summary>
        /// <returns>Line parse state.</returns>
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
                            currentFieldString = new StringBuilder();
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

        /// <summary>
        /// Gets CsvCharacterType of given character. Internal use only.
        /// </summary>
        /// <param name="characterType">Character.</param>
        /// <returns>CsvCharacterType</returns>
        private CsvCharacterType GetCharacterType( char characterType )
        {
            if ( characterType == _options.SplitCharacter )
                return CsvCharacterType.Split;
            else if ( characterType == _options.EscapeCharacter )
                return CsvCharacterType.Escape;
            else
                return CsvCharacterType.Regular;
        }

        /// <summary>
        /// Gets CsvCharacterType of given character with a specific index. Internal use only.
        /// </summary>
        /// <param name="line">Line string. Use RawString.</param>
        /// <param name="index">Index to look at.</param>
        /// <returns>CsvCharacterType</returns>
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
