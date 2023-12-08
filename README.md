# TinyCsvParser
A lightweight CSV text parser which supports custom Escape chars, Splitting chars and Comment line identification.

## Features
- Custom escape character
- Custom splitting character
- Custom comment line ignore
- Detailed error messages
- Lightweight & compact


## Example 1: Parsing CSV contents of a file.
  
```csharp
var options = new CsvParserOptions()
{
    CommentLine = "#",
    EscapeCharacter = '"',
    SplitCharacter = ';',
    ThrowOnParseError = false
}; 

using ( CsvFileReader reader = new CsvFileReader( @"C:\Data.csv", options ) )
{
	foreach ( var fields in reader.ReadLines() )
	{
		Console.WriteLine( "Current Line: {0} - First Field Value: {1}", reader.LineNumber, fields[0] );
		
		// ...
	}
}
```

## Example 2: Parsing CSV contents of a collection.
```csharp
var options = new CsvParserOptions()
{
    CommentLine = "#",
    EscapeCharacter = '"',
    SplitCharacter = ';',
    ThrowOnParseError = false
}; 

List<string> myCsvLines = ...

CsvReader reader = new CsvReader( myCsvLines, options );

foreach ( var fields in reader.ReadLines() )
{
	Console.WriteLine( "Current Line: {0} - First Field Value: {1}", reader.LineNumber, fields[0] );
	
	// ...
}
```

## Example 3: Parsing a CSV line directly.
```csharp
var options = new CsvParserOptions()
{
    CommentLine = "#",
    EscapeCharacter = '"',
    SplitCharacter = ';',
    ThrowOnParseError = false
}; 

string csv = @"example;""data"";123";
 
CsvLineParser parser = new CsvLineParser( csv, options );

if ( parser.Success ) 
{
	// ...
}
```
