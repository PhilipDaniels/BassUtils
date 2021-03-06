# BassUtils
Low-level utility functions for use in any .Net project. Zero dependencies.


# Change History

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
**ArgumentValidators** Throw exceptions if method arguments do not meet expectations:

```
Name = name.ThrowIfNull("name");
FileName = fileName.ThrowIfFileDoesNotExist("name");
```

**AssemblyExtensions** Get embedded resources:
```
string sql = typeof(this).Assembly.GetResourceAsString("Foo.sql");
```

**Comb** Generate Guids in a monotonic way for SQL server that will not lead to
index fragmentation:
```
Guid g = Comb.NewGuid();
```

**ConfigurationLoader** Extremely useful class for loading custom configuration sections.
```
// See documentation in the class or the cases in BassUtils.Tests.
// You want to use this, trust me.
```

**Conv** Convenient conversion functions:
```
int x = Conv.StringToBest("42");
```

**DirectoryWatcher** A wrapper around FileSystemWatcher that will de-duplicate
events.

**Glob** A Unix-like file globber, capable of recursing globbing, for example@
```
var filenames = Glob.ExpandNames(@"C:\temp\**\*.cs");
```

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

// Simple CSV-like "listifier". Also on StringBuilder.
string list = "".AppendCSV(number, name, street, town, county, country);
list = "Hello,".AppendIfDoesNotEndWith(",");
list = list.TrimAppend("  world  ");
```
 