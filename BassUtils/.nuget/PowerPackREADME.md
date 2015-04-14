# PowerPack


## Synopsis
A Powershell module for building and NuGet-packing .Net projects.

A convenient and easy to use module for those who need to package projects but
don't have a full blown continuous-integration environment to do it for them.


## Installation
Available on NuGet

```
install-package PowerPack
```


## Un-Installation
Do 

```
uninstall-package PowerPack
```

and then manually remove the three PowerPack files from the .nuget folder.


## Features
The main function is `PowerPack` which takes an array of projects to build,
for example

```
PowerPack @("Foo.Core", "Foo.Model", "Foo.Server")
```

What PowerPack does is controlled by the "Do" settings on the `$options` object
which is exported by the module. The defaults are

```
$options.DoClean = $true;        # Clean projects before building them (recommended).
$options.DoBuild = $true;        # Build projects.
$options.DoPack = $true;         # Pack projects into .nupkg files using their .nuspec files.
$options.DoPush = $false;        # Push .nupkg files to the configured NuGet feed.
```

If you are using a standard solution layout PowerPack should find your projects
automatically based on their name. If you are using something more esoteric, or
you move the psm1 file to another location you will need to adjust
`$options.ProjectSearchDirectories`. 

For the NuGet commands, PowerPack will look for a nuspec file automatically.
For a project called "Foo.Server", PowerPack will look for "Foo.Server.nuspec"
in the project directory. If not found, PowerPack will then look for "Package.nuspec".
If that is also not found, an error will result.

NuGet push is disabled by default to avoid pushing files to nuget.org by accident.
You must enable it by setting `$options.DoPush` to true and setting `$options.NuGetFeed`
to the feed you want to push to (blank for nuget.org).

For further details, see the code, especially the $options object at the bottom of
the module. It is well commented.


## Supporting functions
- BuildProject
- CleanProject
- CommandExists
- DeleteDirectory (because Remove-Item -Recurse is broken!)
- DownloadNuGet
- GetAssemblyVersion
- GetExistingNuGetPackageFilenames
- GetMSBuildArguments
- GetPackageDirectory
- GetProjectXml
- LocateFile
- LocateMsBuild
- LocateNuGet
- LocateNuSpec
- LocateProject
- NuGetClean
- NuGetList
- NuGetPack
- NuGetPush
- PowerPackProject
- SetOutputAssemblyFileName
