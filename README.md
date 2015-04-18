# BassUtils
Low-level utility functions for use in any .Net project. Zero dependencies.


# Change History

#### [Unreleased]
###### Added
- ExportedInterfacesView

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

**IDataReaderExtensions, IDataRecordExtensions** Many extensions to make working
with these classes more convenient, including extensions to get columns by name,
get columns with defaults if null, and get nullable columns. Also includes
convenient Hydrate() methods to create entities.

**IndentingStringBuilder** A wrapper around StringBuilder that can create
indented text. Handy for code generation.

**IniParser** A simple parser for INI files.

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

**StringExtensions*** Some handy string extensions:
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
 