# BassUtils.NetCore
[![NuGet Badge](https://buildstats.info/nuget/bassutils.netcore)](https://www.nuget.org/packages/BassUtils.NetCore/)

Low-level utility functions for use in any .Net Core 3.1 and later project.
Available on [NuGet](https://www.nuget.org/packages/BassUtils.NetCore)

Notable mainly for its middleware (`app.UseRuntimeInfo()`), which will produce a lot of detailed information
about loaded assemblies, machine environment etc., at a '/runtimeinfo' endpoint.

Also contains some extensions to `IServiceCollection` to aid with registering strongly-typed
configuration classes, including those that will respond to changes in the configuration
at runtime without restarting the app.

Also available: [BassUtils](https://www.nuget.org/packages/BassUtils)
and [BassUtils.Oracle](https://www.nuget.org/packages/BassUtils.Oracle)

# Change History

#### [4.6.0]
###### Added
- Display the process up-time on the `runtimeinfo` middleware endpoint, plus the
  value of the two critical ASP.Net Core [environment variables](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-6.0).

#### [4.5.0]
###### Added
- Added new extension method `AddConfigurationModelWithMonitoring` to allow the
easy registration of configuration classes that will respond to changes in the environment
or configuration file, allowing configuration to be easily changed at runtime.

###### Changed
- Deprecated extension method `AddConfigurationModel` in favour of the identical,
but more explicitly named, `AddConfigurationModelAsSingleton`.
- Exposed `ValidateConfigurationAndThrow` as a public helper method. It is not
an extension method, however, as there is nothing really for it to extend.

#### [4.4.2]
###### Fixed
- Trimmed the READMEs for the 3 BassUtils packages to be just relevant to them.
- Added NuGet badges.

#### [4.4.0]
###### Changed
- Bumped dependencies. No functional changes.

#### [4.1.12]
###### Added BassUtils.NetCore
- Added BassUtils.NetCore library.
- Added `RuntimeInformationMiddleware` and `app.UseRuntimeInfo` extension method.
- Added `services.AddConfigurationModel`.
