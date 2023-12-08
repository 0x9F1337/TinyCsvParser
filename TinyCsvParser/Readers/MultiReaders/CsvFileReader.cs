using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.Exceptions;

namespace TinyCsvParser.Readers.MultiReaders
{
    public class CsvFileReader : BaseMultiReader, IDisposable
    {
        private readonly string _filePath;
        private readonly FileStream _fileStream;
        private readonly StreamReader _streamReader;

        public CsvFileReader(string filePath, CsvParserOptions options)
            : base(options)
        {
            if (filePath == null) throw new ArgumentNullException(filePath, "file path cannot be null.");

            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath, $"file \"{filePath}\" does not exist.");

            _filePath = filePath;

            _fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            _streamReader = new StreamReader(_fileStream);
        }

        public IEnumerable<List<string>> ReadLines()
        {
            string line = null;
            while ((line = _streamReader.ReadLine()) != null)
            {
                var fields = ReadLine(line);

                if (fields == null)
                {
                    continue;
                }

                yield return fields;
            }
        }

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
