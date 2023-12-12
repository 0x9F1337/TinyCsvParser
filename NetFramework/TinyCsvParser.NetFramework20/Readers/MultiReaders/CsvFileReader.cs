using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace TinyCsvParser.NetFramework20.Readers.MultiReaders
{
    public class CsvFileReader : BaseMultiReader, IDisposable
    {
        private readonly string _filePath;
        private readonly FileStream _fileStream;
        private readonly StreamReader _streamReader;

        /// <summary>
        /// Opens an handle to given file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public CsvFileReader( string filePath, CsvParserOptions options )
            : base( options )
        {
            if ( filePath == null ) throw new ArgumentNullException( filePath, "file path cannot be null." );
            if ( !File.Exists( filePath ) ) throw new FileNotFoundException( filePath, $"file \"{filePath}\" does not exist." );

            _filePath = filePath;

            _fileStream = new FileStream( filePath, FileMode.Open, FileAccess.Read, FileShare.Read );
            _streamReader = new StreamReader( _fileStream );
        }

        /// <summary>
        /// Read current line and advance reading position in stream.
        /// </summary>
        /// <returns>List of parsed fields from current line.</returns>
        public IEnumerable<List<string>> ReadLines()
        {
            string line = null;
            while ( ( line = _streamReader.ReadLine() ) != null )
            {
                var fields = ReadLine( line );

                if ( fields == null )
                {
                    continue;
                }

                yield return fields;
            }
        }

        /// <summary>
        /// Reset data and stream. Use this if you want to re-read from the same instance.
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            _streamReader.DiscardBufferedData();
            _fileStream.Flush();
            _fileStream.Position = 0;
        }

        public void Dispose()
        {
            _streamReader?.Dispose();
            _fileStream?.Dispose();
        }
    }
}
