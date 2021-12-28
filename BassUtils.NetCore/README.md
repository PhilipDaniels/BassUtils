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

The [GitHub repository](https://www.github.com/PhilipDaniels/BassUtils)
includes a sample project called **BassUtils.OracleExamples**.

It shows how to use all the extension methods in this project, especially
the `OracleParameterCollectionExtensions`. You can install Oracle via Docker
if need be - see the SQL script for instructions.

# Change History

#### [4.3.0]
###### Changed
- Bumped dependencies. No functional changes.

#### [4.1.12]
###### Added BassUtils.NetCore
- Added BassUtils.NetCore library.
- Added RuntimeInformationMiddleware and `app.UseRuntimeInfo` extension method.
- Added `services.AddConfigurationModel`.
