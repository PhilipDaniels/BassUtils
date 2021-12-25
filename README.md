# BassUtils
Low-level utility functions for use in any .Net Standard 2.0 project.
Available on [NuGet](https://www.nuget.org/packages/BassUtils/)

# BassUtils.NetCore
Low-level utility functions for use in any .Net Core 3.1 and later project.
Available on [NuGet](https://www.nuget.org/packages/BassUtils.NetCore)


See individual READMEs for more information.


# How to publish and version - aide-mémoire

If version 4.1 is published on nuget.org, then to start the next version:

- Make a branch v4.2
- Update version.json to read:    "version": "4.2-alpha"
- Make commits on that branch and build using Nerdbank.GitVersioning - they
  will come out as 4.2.x-alpha-GUID.nupkg.
- Use clear_package_cache.ps1 to facilitate local development.
- When done, change version.json to "4.2"
- Merge down to master branch and build - packages should be at version 4.2.
- Upload packages to NuGet.
