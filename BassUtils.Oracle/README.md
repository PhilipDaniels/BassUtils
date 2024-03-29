﻿# BassUtils.Oracle
[![NuGet Badge](https://buildstats.info/nuget/bassutils.oracle)](https://www.nuget.org/packages/BassUtils.Oracle/)

Low-level utility functions to simplify working with Oracle via
Oracle.ManagedDataAccess.Core.
Available on [NuGet](https://www.nuget.org/packages/BassUtils.Oracle)

The [GitHub repository](https://www.github.com/PhilipDaniels/BassUtils)
includes a sample project called **BassUtils.OracleExamples**.

It shows how to use all the extension methods in this project, especially
the `OracleParameterCollectionExtensions`. You can install Oracle via Docker
if need be - see the SQL script for instructions.

[Oracle's official documentation for ODP.Net](https://docs.oracle.com/en/database/oracle/oracle-data-access-components/19.3.2/odpnt/features.html#GUID-FF58E75D-B5D1-4327-B65E-CE263E3A5C6C)

Also available: [BassUtils](https://www.nuget.org/packages/BassUtils)
and [BassUtils.NetCore](https://www.nuget.org/packages/BassUtils.NetCore)

# Features

* `DbTransactionExtensions`: wrapper methods to log success/failure methods
  when performing database transactions.
* `OracleConnectionExtensions`: Easier to use overloads of the `OracleUdt.GetValue`
  and `OracleUdt.SetValue` methods.
* `WrappedTransaction`: bundles a transaction and its connection together to
  make them easier to dispose correctly.
* `OracleParameterExtensions`: extension methods to get the `Value` property as
  an `OracleDataReader` or as a list of strings or numbers.
* `OracleParameterCollectionExtensions`: many extension methods to help with creating
  parameters and RETURN parameters for [UDTs](https://docs.oracle.com/en/database/oracle/oracle-data-access-components/19.3.2/odpnt/featUDTs.html#GUID-7913CDD0-CB22-4257-828F-FBCCA3FE9126)
  and tables of UDTs,
  [Associative Arrays](https://docs.oracle.com/en/database/oracle/oracle-data-access-components/19.3.2/odpnt/featOraCommand.html#GUID-05A6D391-E77F-41AF-83A2-FE86A3D98872),
  and [Array Binding](https://docs.oracle.com/en/database/oracle/oracle-data-access-components/19.3.2/odpnt/featOraCommand.html#GUID-FACB870D-6F8B-46EA-95EA-65C6C6536B9E),
  and [Ref Cursors](https://docs.oracle.com/en/database/oracle/oracle-data-access-components/19.3.2/odpnt/featRefCursor.html#GUID-4215DACA-977E-473F-AF4E-764841A476D7)


# Change History

#### [4.7.0]
###### Added
- OracleDb simple abstract base class.

#### [4.6.3]
###### Fixed
- In `GetStringLengths`, handle null strings by returning 0.

#### [4.6.2]
###### Changed
- Improved the description of the NuGet package.

#### [4.6.1]
###### Fixed
- The `AddAssociativeArray` family of functions must actually call `ToArray`
  on their values or ODP.Net will give an error (e.g. if you pass it a LINQ
  enumerable).

#### [4.6.0]
###### Added
- Added the `WrappedTransaction` class, which bundles an `OracleTransaction` together
  with its corresponding `OracleConnection` and ensures they are properly disposed together.
###### Fixed
- Fixed the logging in `DbTransactionExtensions` so that it works properly with
  non-MS loggers (tested with Serilog).

#### [4.4.2]
###### Fixed
- Trimmed the READMEs for the 3 BassUtils packages to be just relevant to them.
- Added NuGet badges.

#### [4.4.0]
- Library created.
