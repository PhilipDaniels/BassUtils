# Example of packing a project. For more, type 'Get-Help PowerPack' after importing the module.
# The individual functions also have help available.
Import-Module .\PowerPack -Force

# Get latest version of NuGet into this folder: nuget restore requires at least v2.7.
if (!(Test-Path "NuGet.exe"))
{
	Invoke-NuGetDownload
}

Find-File "*.sln" | Invoke-NuGetRestore

$version = "2.2.0.0"
$configs = "Release40", "Release45"

Find-Project "BassUtils" | Get-Project | % { $_.VersionNumber = $version; $_ } |
	Update-NuSpecDependencies | Invoke-DeepClean |
	% { $project = $_ ; $configs | % { Invoke-MSBuild $project -Configuration $_ } } |
	Sort -Unique | Invoke-NuGetPack -Version $version |
    Invoke-NuGetPush -NuGetFeed "LocalNuGetFeed"
