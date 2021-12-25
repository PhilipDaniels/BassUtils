# BassUtils
Low-level utility functions for use in any .Net Standard 2.0 project.
Available on [NuGet](https://www.nuget.org/packages/BassUtils/)

# BassUtils.NetCore
Low-level utility functions for use in any .Net Core 3.1 and later project.
Available on [NuGet](https://www.nuget.org/packages/BassUtils.NetCore)

# BassUtils.Oracle
Low-level utility functions to simplify working with Oracle via
Oracle.ManagedDataAccess.Core.
Available on [NuGet](https://www.nuget.org/packages/BassUtils.Oracle)

The [GitHub repository]() includes a sample project called *BassUtils.OracleExamples*.
Clone it and read the examples. (You can install Oracle via Docker if need be).


# Change History

#### [4.3.0]
- Library created. Extension methods for `OracleParameterCollection` to aid in creating
  parameters, especially with Oracle UDTs, tables and associative arrays and return
  parameters.
- DbTransaction extensions to help with logging around transaction commit/rollback.
