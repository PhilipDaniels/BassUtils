# BassUtils

[![NuGet Badge](https://buildstats.info/nuget/bassutils)](https://www.nuget.org/packages/BassUtils/)

Low-level utility functions for use in any .Net Standard 2.0 project.
Available on [NuGet](https://www.nuget.org/packages/BassUtils/)

Also available: [BassUtils.NetCore] (https://www.nuget.org/packages/BassUtils.NetCore)
and [BassUtils.Oracle](https://www.nuget.org/packages/BassUtils.Oracle)


# Change History

#### [4.4.2]
###### Fixed
- Trimmed the READMEs for the 3 BassUtils packages to be just relevant to them.
- Added NuGet badges.

#### [4.4.0]
###### Added
- Better RuntimeInfo.
- Add TVP helpers for MS SQL server.

#### [4.2.0]
###### Added
- `AppendLines` methods to the StringBuilder extensions and the `IndentingStringBuilder`.
- `BeginCodeBlock` and dispose functionality to `IndentingStringBuilder`.
- Async versions of `ReadOne`, `Hydrate` and `HydrateAll`. These are extensions on
  `DbDataReader` though, not `IDataReader`, which doesn't provide async methods.

#### [4.1.12]

###### Added
- Added `IDataReader.ReadOne`, `HydrateOne` and `TryHydrateOne` methods.
- Added `SqlDataRecord` extensions for use with MS-SQL TVPs.
- Added RuntimeInformation classes.
- Added BassUtils.NetCore downstream library

###### Changed
- `ObjectDataReader` now inherits from `DbDataReader`. Previously it just implemented
  the interface `IDataReader`, which caused type conversion problems in some advanced
  scenarios.

#### [4.0.0]

###### Changed
- Upgraded to Net Standard 2.0.
- Some things moved to sub-namespaces 'Data' and 'MsSql'.
- IniData remains, but the recommendation in its doc-comment has changed to reference
  [ini-parser-netstandard](https://www.nuget.org/packages/ini-parser-netstandard/).
- StreamExtensions.ReadFully has become ReadToEnd.
- The SqlBulkCopyDataReader class now targets Microsoft.Data.SqlClient, not System.Data.SqlClient.
  See https://devblogs.microsoft.com/dotnet/introducing-the-new-microsoftdatasqlclient/ for background.
  This means that BassUtils now has a dependency on the Microsoft.Data.SqlClient package.
- CSV has been changed to 'Csv' for compliance with Microsoft recommended naming standards.
- GetLeadingNumber and GetTrailingNumber have had their APIs change, and they now work!
- Regularzie naming in IDataRecordExtensions: GetSingle becomes GetFloat, GetStringOrNull
  becomes GetNullableString, GetValueOrNull becomes GetNullableValue.

###### Removed
- Various utilities have been removed due to better alternatives now becoming available in the community
  or me simply not making any use of them.
- ArgumentValidators - use [Dawn.Guard](https://www.nuget.org/packages/Dawn.Guard/) instead (this
  library includes it as a dependency).
- Glob - use [Microsoft.Extensions.FileSystemGlobbing](https://www.nuget.org/packages/Microsoft.Extensions.FileSystemGlobbing)
- Partition and DistinctBy methods - use [MoreLinq](https://www.nuget.org/packages/morelinq/).
- ConfigurationLoader, DoubleExtensions, ExpandoExtensions, PredicateBuilder, PropertyCopier, TableAdapterBase,
  ThrottledBlockingQueue, XmlWriterExtensions have been removed.
- Removed the Hydate and HydrateAll functions that use reflection to guess a ctor to call. Use the
  overloads that take a lambda instead, they are more predictable.

#### [3.1.0]
###### Fixed
- Make AssemblyExtensions.GetResourceFileName work in ILMerge scenarios by searching for names by
  partial match if an exact match cannot be found. Still requires unique names.

###### Added
- Take the existing AppendCSV functionality and move it to be extensions to the TextWriter class,
  therefore making it available for use in StreamWriter and StringWriter classes. Also extend the
  CSVOptions so that the class writes CSV that is more in line with the unofficial CSV
  specification as described [on Wikipedia](https://en.wikipedia.org/wiki/Comma-separated_values);
  in particular see the [Standardization](https://en.wikipedia.org/wiki/Comma-separated_values#Standardization)
  section.
- Add `[DebuggerStepThrough]` attributes to the ArgumentValidators.

#### [3.0.0] - 2015-10-29
###### Fixed
- In PropertyCopier, only write to properties with a setter.

###### Added
- StringExtensions.TrimAll, RemoveNewLines, ContainsAll.
- New overload of StringExtensions.SafeFormat that defaults to InvariantCulture.
- New Glob class (Unix-like file globber)
- IEnumerableExtensions.DistinctBy.
- ConfigurationLoader.CanLoad method.

###### Changed
- IniParser has been refactored into IniData and IniSection classes, and a
  static Parse method is now the starting point. Tests added, and the ability
  to parse INI files with duplicate keys and valueless keys added (based on
  data seen in real-life INI files). This change is the reason for the bump
  in the version number.

#### [2.2.1] - 2015-04-27
###### Fixed
- Bug fix in ConfigurationLoader regarding multiple named sections.

#### [2.2.0] - 2015-04-26
###### Added
- Built 4.0 and 4.5 versions of the assembly.
- Gave ConfigurationLoader the ability to load a section by name, so you can use
  the same type to load several different (named!) configuration sections.
- AppDomainExtensions.IsLoaded
- FileUtils.GetExeDirectory, NormalizeToExeDirectory, NormalizeToDirectory.
- IEnumerableExtensions.ToHashSet.

#### [2.1.0] - 2015-04-18
###### Added
- StringExtensions.SetChar
- StringBuilderExtensions.EndsWith, AppendCSV, AppendIfDoesNotEndWith, TrimAppend
- StringExtensions.AppendCSV, AppendIfDoesNotEndWith, TrimAppend
- ConfigurationLoader (see documentation in the class or BassUtils.Tests)

#### [2.0.0] - 2015-03-25
###### Added
- PropertyCopier, ThrottledBlockingQueue, ICollectionExtensions.
- More XML documentation for methods.

###### Removed
- ToCSV() methods from DataTableExtensions and DataViewExtensions. There are better ways
of doing this, for example see http://joshclose.github.io/CsvHelper/

#### [1.1.1] - 2015-03-23
###### Added
- XML documentation for most functions and classes.

#### [1.1.0] - 2015-03-19
###### Added
- Conv.StringToBest function.



# Highlights (not exhaustive)
**AssemblyExtensions** Get embedded resources:
```
string sql = typeof(this).Assembly.GetResourceAsString("Foo.sql");
```

**Comb** Generate Guids in a monotonic way for SQL server that will not lead to
index fragmentation:
```
Guid g = Comb.NewGuid();
```

**Conv** Convenient conversion functions:
```
int x = Conv.StringToBest("42");
```

**DirectoryWatcher** A wrapper around FileSystemWatcher that will de-duplicate
events.

**IDataReaderExtensions, IDataRecordExtensions** Many extensions to make working
with these classes more convenient, including extensions to get columns by name,
get columns with defaults if null, and get nullable columns. Also includes
convenient Hydrate() methods to create entities.

**IndentingStringBuilder** A wrapper around StringBuilder that can create
indented text. Handy for code generation.

**IniData** A simple parser for INI files.

**ObjectDataReader** A very useful class that can wrap any class in an IDataReader,
making it possible to bind to it, use SqlBulkCopy etc. Often used in conjunction
with the **SqlBulkCopyDataReader** to perform validation on fields before bulk
inserting them:
```
var orders = new List<Order>();
...
using (var orderReader = new ObjectDataReader(orders))
using (var bulkCopy = new SqlBulkCopy(conn) { DestinationTableName = "dbo.Orders" })
using (var sbcReader = new SqlBulkCopyDataReader(orderReader, conn, bulkCopy))
{
    bulkCopy.WriteToServer(sbcReader);
}
```

**SqlBulkCopyExtensions** An extension to get the number of rows uploaded:
```
int numRows = bulkCopy.TotalRowsCopied();
```

**StringExtensions** Some handy string extensions:
```
string key = "Name=Phil".Before("=");
string value = "Name=Phil".After("=");
string s = "Hello".Repeat(3);
string t = "hi".Left(3); // safe.

// Simple CSV-like "listifier". Also on StringBuilder and TextWriter.
string list = "".AppendCsv(number, name, street, town, county, country);
list = "Hello,".AppendIfDoesNotEndWith(",");
list = list.TrimAppend("  world  ");
```
 