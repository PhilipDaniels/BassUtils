# BassUtils
Low-level utility functions for use in any .Net project.


# Change History
2.0.0 - Add PropertyCopier, ThrottledBlockingQueue, ICollectionExtensions.
        Breaking change: remove ToCSV() methods from DataTableExtensions
        and DataViewExtensions. There are better ways of doing this,
        for example see http://joshclose.github.io/CsvHelper/
        More docs.
1.1.1 - Add XML documentation for most functions and classes.
1.1.0 - Add Conv.StringToBest function.


## Highlights (not exhaustive)
*ArgumentValidators* Throw exceptions if method arguments do not meet expectations.

```
Name = name.ThrowIfNull("name");
FileName = fileName.ThrowIfFileDoesNotExist("name");
```

*AssemblyExtensions* Get embedded resources
```
string sql = typeof(this).Assembly.GetResourceAsString("Foo.sql");
```

