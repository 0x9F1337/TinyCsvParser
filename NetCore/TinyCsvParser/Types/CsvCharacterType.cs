namespace TinyCsvParser.Types
{
    /// <summary>
    /// Self explanatory. Used by CsvLineParser internally.
    /// </summary>
    public enum CsvCharacterType
    {
        None,
        Regular,
        Split,
        Escape
    }
}
