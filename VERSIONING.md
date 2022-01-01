# How to publish and version with Nerdbank - aide-mémoire

If version 4.1 is published on nuget.org, then to start the next version:

- Make a branch v4.2, but do not update version.json yet
- Make commits on that branch and build using Nerdbank.GitVersioning - they
  will come out as 4.1.x-alpha-GUID.nupkg.
- Use clear_package_cache.ps1 to facilitate local development.
- Merge down to master branch and build
- When done, change version.json to "4.2" and build
- Upload packages to NuGet.

