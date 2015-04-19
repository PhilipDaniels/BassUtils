$source = @"
using System.IO;

public class ConfigInfo
{
    public string Configuration { get; set; }
    public string OutputAssemblyFileName { get; set; }
}

public class Project
{
    public string Name { get; set; }
    public string FileName { get; set; }
    public string Directory { get; set; }
    public string NuSpecFileName { get; set; }
    public Dictionary<string, ConfigInfo> Configs { get; private set; }

    public Project()
    {
        Configs = new Dictionary<string, ConfigInfo>();
    }
}
"@

Add-Type -TypeDefinition $source

# Returns $true if a command exists, $false otherwise.
function CommandExists
    (
    $command
    )
{
    $oldPreference = $ErrorActionPreference
    $ErrorActionPreference = 'stop'

    try
    {
        if (Get-Command $command)
        {
            return $true
        }
    }
    catch
    {
        return $false
    }
    finally
    {
        $ErrorActionPreference = $oldPreference
    }
}

# Locate MSBuild. Returns the path to MSBuild.exe if found.
function LocateMsBuild()
{
    foreach ($dotNetVersion in @("4.0", "3.5", "2.0"))
    {
	    $regKey = "HKLM:\software\Microsoft\MSBuild\ToolsVersions\$dotNetVersion"
	    $regProperty = "MSBuildToolsPath"
	    $msbuild = Join-Path -path (Get-ItemProperty $regKey).$regProperty -childpath "msbuild.exe"
        if (Test-Path $msbuild)
        {
	        return $msbuild
        }
    }
}

# Locates NuGet.exe. If it exists in the same path as the script that NuGet.exe is used,
# else we look for it on the path. If it is not on the path there is another function
# that can download it.
function LocateNuGet
    (
    [bool] $downloadIfNecessary
    )
{
    $nuget = Join-Path -path $PSScriptRoot -ChildPath "NuGet.exe"
    if (Test-Path $nuget)
    {
        return $nuget
    }
    else
    {
        $exists = CommandExists "NuGet.exe"
        if ($exists)
        {
            return "NuGet.exe"
        }
        if ($downloadIfNecessary)
        {
            DownloadNuGet
            if (Test-Path $nuget)
            {
                return $nuget
            }
        }
    }
}

# Downloads the latest NuGet.exe and places it in the same directory as the script.
# If NuGet.exe already exists it will be overwritten.
function DownloadNuGet()
{
    $sourceNuGetExe = "http://NuGet.org/NuGet.exe"
    $targetNuGetExe = "$PSScriptRoot\NuGet.exe"

    Write-Host "Downloading http://NuGet.org/NuGet.exe into script directory, please wait..."
    Invoke-WebRequest $sourceNuGetExe -OutFile $targetNuGetExe
    Write-Host "Download NuGet.exe complete."
}

# Search for a file in a set of paths.
# Returns the resolved path if found, else null.
function LocateFile
	(
	[string] $fileName,		# The file name to find. Does not include a path component.
	[string[]] $paths		# Priority-ordered list of paths to search for $fileName.
	)
{
	foreach ($path in $paths)
	{
		$p = Join-Path -path $path -childpath $fileName
		if (Test-Path $p)
		{
			$p = Resolve-Path "$p"
			return $p
		}
	}
}

# Finds the csproj for a project name. Returns an object of type Project which has several useful
# properties (so we can avoid continually calculating them).
function LocateProject
	(
	[string] $projectName,		     # For example, "Misc.Core"
    [string[]] $searchDirectories    # Array of directories to search. Leave blank to search "the usual places".
	)
{
	$projectFile = "$projectName.csproj"

    if (!$searchDirectories)
    {
        $searchDirectories = $defaults.ProjectSearchDirectories
    }

    # Try the search paths as supplied.
	$f = LocateFile $projectFile $searchDirectories
	if (!$f)
	{
        # Try the search paths with the project name on the end as well. (The Foo project is usually in a folder as Foo\Foo.csproj
        # so this is the most likely match).
    	$f = LocateFile "$projectName\$projectFile" $searchDirectories
        if ($f)
        {
            $projectObject = New-Object -type Project
            $projectObject.Name = $projectName
            $projectObject.FileName = $f
            $projectObject.Directory = (Split-Path -Parent $f);
            $projectObject.NuSpecFileName = (LocateNuSpec $projectObject)

            return $projectObject
        }
	}
}

# Locates a set of projects.
# Will exit if a project cannot be located.
function LocateProjects
    (
    [string[]] $projects		# Array of projects to locate.
    )
{
    $projectObjects = @()

    foreach ($project in $projects)
    {
        $po = LocateProject $project
        if ($po)
        {
            $projectObjects += $po
        }
        else
        {
            Write-Error "Could not locate $project"
            Exit 1
        }
    }

    return $projectObjects
}

# Loads the project file and returns it as an XML document.
function GetProjectXml
    (
    $projectObject            # From LocateProject.
    )
{
    [xml]$xml = Get-Content $projectObject.Filename
    return $xml
}

# Returns the complete set of arguments to be used when invoking MSBuild. Based on the options and
# whether we need to do a Clean or a Clean and Build.
function GetMSBuildArguments
    (
    $projectObject,            # From LocateProject.
    [bool] $clean,             # If true, does a clean (before the build).
    [bool] $build,             # If true, does a build.
    [string] $verbosity,       # MS Build verbosity. Leave blank to use default from $defaults.
    [string] $configuration,   # Project configuration to build. Leave blank to use default from $defaults.
    [string] $platform         # Platform to build. Leave blank to use default from $defaults.
    )
{
    $targets = ""
    if ($clean)
    {
        $targets = "Clean"
    }
    if ($build)
    {
        if ($clean)
        {
            $targets += ";"
        }
        $targets += "Build"
    }

    if (!$targets)
    {
        Write-Error "Must specify either clean, build or both"
        Exit 1
    }

    if (!$verbosity)
    {
        $verbosity = $defaults.MsBuildVerbosity
    }
    if (!$configuration)
    {
        $configuration = $defaults.Configuration
    }
    if (!$platform)
    {
        $platform = $defaults.Platform
    }


    return @(
        $($projectObject.FileName),
        "/target:$targets",
        "/nologo",
        "/verbosity:$verbosity",
        "/p:Configuration=$configuration",
        "/p:PlatformTarget=$platform",
        "/p:RunCodeAnalysis=False"
        )
}

# Delete a directory and all its contents, recursively.
# Unbelievably, the -Recurse flag to Remove-Item is broken.
function DeleteDirectory
    (
    [string] $directory
    )
{
    if (Test-Path $directory)
    {
        Get-ChildItem -Path $directory -Recurse | Remove-Item -Force -Recurse
        Remove-Item $directory -Force
    }
}

# Does an MSBuild clean of the specified configuration.
function CleanProject
    (
	$projectObject,            # From LocateProject.
    [string] $configuration    # Configuration to clean. Leave blank to use default from options.
	)
{
    $msbuild = LocateMsBuild
    $args = GetMSBuildArguments $projectObject $true $false $null $configuration
    Write-Host "Cleaning $($projectObject.FileName)"
    &$msbuild $args

	if ($lastExitCode -ne 0)
    {
        Write-Error "Error while running MSBuild for clean."
        Exit 1
    }

    Write-Host "Cleaning $($projectObject.FileName) complete"
}

# Deletes the bin and obj directories for the project.
function DeepCleanProject
    (
	$projectObject            # From LocateProject.
	)
{
    foreach ($d in @("bin", "obj"))
    {
        $dir = Join-Path $projectObject.Directory $d
        Write-Host "Deleting $dir"
        DeleteDirectory $dir
    }
}

# Builds the specified project.
# It is recommended that you do a clean or a deep-clean first.
function BuildProject
	(
	$projectObject,            # From LocateProject.
    [string] $verbosity,       # MS Build verbosity. Leave blank to use default from $defaults.
    [string] $configuration,   # Project configuration to build. Leave blank to use default from $defaults.
    [string] $platform         # Platform to build. Leave blank to use default from $defaults.
	)
{
    $msbuild = LocateMsBuild
    $args = GetMSBuildArguments $projectObject $false $true $verbosity $configuration $platform
    Write-Host "Building $($projectObject.FileName): MSBuild.exe $args"
    &$msbuild $args

	if ($lastExitCode -ne 0)
    {
        Write-Error "Error while running MSBuild."
        Exit 1
    }

    Write-Host "Building $($projectObject.FileName) complete"
}

# Locates the output assembly for a project and stores it on the project object.
function SetOutputAssemblyFileName
    (
    $projectObject,           # From LocateProject.
    [string] $configuration   # Configuration, which will determine the output directory we will be looking in.
    )
{
    $filename = (Join-Path -Path $projectObject.PowerPackDirectory -ChildPath $projectObject.Name) + ".exe"
    if (Test-Path $filename)
    {
        $projectObject.OutputAssemblyFileName = $filename
    }
    else
    {
        $filename = (Join-Path -Path $projectObject.PowerPackDirectory -ChildPath $projectObject.Name) + ".dll"
        if (Test-Path $filename)
        {
            $projectObject.OutputAssemblyFileName = $filename
        }
        else
        {
            $projectObject.OutputAssemblyFileName = $null
        }
    }
}

# Returns the version of an assembly (as set in your AssemblyInfo.cs).
function GetAssemblyVersion
    (
    [string] $filename               # Full path to the assembly, e.g. C:\temp\foo.dll.
    )
{
	# Retrieve the assembly version without loading the assembly. A naive version using
	#   $asm = [Reflection.Assembly]::LoadFile($exePath)
    #   $asm = [Reflection.Assembly]::ReflectionOnlyLoadFrom($exePath)
    #   $asmName = $asm.GetName()
	# will load the assembly into the powershell's appdomain and keep it locked, meaning
	# that the script is not rerunnable.
	#
	# This version does not suffer from that problem.

    $version = [System.Reflection.AssemblyName]::GetAssemblyName($filename).Version;
	return $version
}

# Finds the Nuspec file for a project. It is expected to be in the project directory,
# named (in order of priority): $projectName.nuspec or Package.nuspec.
function LocateNuSpec
    (
    $projectObject            # From LocateProject.
    )
{
    $NuSpecFileName = Join-Path -Path $projectObject.Directory -ChildPath "$($projectObject.Name).nuspec"
    if (Test-Path $NuSpecFileName)
    {
        return $NuSpecFileName
    }
    $NuSpecFileName = Join-Path -Path $projectObject.Directory -ChildPath "Package.nuspec"
    if (Test-Path $NuSpecFileName)
    {
        return $NuSpecFileName
    }
}

# Gets all the nupkg filenames that exist in a directory.
# Since we have no way of knowing the exact name of the package (it depends on the version number it was built
# with and it may not have the same name as the project) this function can return 0, 1 (typically) or several
# filenames, and possibly for several projects.
function GetExistingNuGetPackageFilenames
    (
    [string] $directory      # Directory to look in. Leave blank to use $defaults.PackageDirectory
    )
{
    if (!$directory)
    {
        $directory = $defaults.PackageDirectory
    }

    $filter = "*.nupkg"
    $filenames = Get-ChildItem -Path $directory -Filter $filter
    return $filenames
}

# Lists the nupkg files that exist for a project in its package output directory.
function NuGetList
    (
    [string] $directory      # Directory to look in. Leave blank to use $defaults.PackageDirectory
    )
{
    $files = GetExistingNuGetPackageFilenames $directory
    foreach ($file in $files)
    {
        Write-Host $file.Fullname
    }
}

# Removes one or more nupkg files in the specified directory.
# The files to delete are determined by calling GetExistingNuGetPackageFilenames.
function NuGetClean
    (
    [string] $directory      # Directory to look in. Leave blank to use $defaults.PackageDirectory
    )
{
    $files = GetExistingNuGetPackageFilenames $directory
    foreach ($file in $files)
    {
        Remove-Item $file.Fullname
    }
}

# Runs "NuGet pack" on a project. The project should be built (using BuildProject) first, this function won't do it.
# It will download NuGet if necessary.
function NuGetPack
    (
    $projectObject,           # From LocateProject.
    [string] $version,         # Version to use. Optional, in which case the version from the assembly is used.
    [string] $outputDirectory # Directory to place the nupkg file in. Optional, in which case $defaults.PackageDirectory is used.
    )
{
    $nuget = LocateNuGet $true
    if (!$nuget)
    {
        Write-Error "Could not locate NuGet.exe. Please install or download it and put it in the same directory as this script."
        Exit 1
    }

	if (!$projectObject.Filename)
	{
		Write-Error "Could not locate project file for $($projectObject.Name)"
		Exit 1
	}

	if (!$projectObject.NuSpecFileName)
	{
		Write-Error "Could not locate nuspec file for $($projectObject.Name)"
		Exit 1
	}

    if (!$version)
    {
        SetOutputAssemblyFileName $projectObject
        if (!$projectObject.OutputAssemblyFileName)
        {
            Write-Error "Could not locate output assembly for $($projectObject.Name), so version could not be determined."
            Exit 1
        }
        $version = GetAssemblyVersion $projectObject.OutputAssemblyFileName
    }


    # Build the nuspec with that version number.
    if (!$outputDirectory)
    {
        $outputDirectory = $defaults.PackageDirectory
    }

    $args = @("pack", $projectObject.NuSpecFileName, "-Version", "$version", "-OutputDirectory", "$outputDirectory") + $defaults.PackOptions
    Write-Host "Running NuGet pack for $($projectObject.Name): NuGet.exe $args"

    &$nuget $args

	if ($lastExitCode -ne 0)
    {
        Write-Error "Error while running NuGet pack."
        Exit 1
    }

    Write-Host "NuGet pack for $($projectObject.Name) completed"
}

# Publish the specified nupkg files. You can use
# GetExistingNuGetPackageFilenames to get all the nupkg files in a directory.
function NuGetPush
    (
    [string[]] $nupkgFilenames,   # Array of .nupkg file to push.
    [string] $feed                # Feed to push to. Blank means nuget.org.
    )
{
    $nuget = LocateNuGet $true
    if (!$nuget)
    {
        Write-Error "Could not locate NuGet.exe. Please install or download it and put it in the same directory as this script."
        Exit 1
    }

    foreach ($nupkgFilenames in $nupkgFilenames)
    {
        if ($feed)
        {
            $args = @("push", "-Source", "$feed", "$($nupkgFilename.FullName)")
        }
        else
        {
            $args = @("push", "$($nupkgFilename.FullName)")
        }

        # Push the package to the configured feed.
        Write-Host "Running NuGet push for $($nupkgFilename.FullName): NuGet.exe $args"
        &$nuget $args
        Write-Host "NuGet push to $feed of $($nupkgFilename.FullName) completed"
    }
}

<#
function PowerPackProject
    (
    $projectObject            # From LocateProject.
    )
{
    if ($defaults.DoClean)
    {
        CleanProject $projectObject
    }
    if ($defaults.DoBuild)
    {
        BuildProject $projectObject
    }
    if ($defaults.DoPack)
    {
        NuGetPack $projectObject
    }
    if ($defaults.DoPush)
    {
        NuGetPush $projectObject
    }
}

# Process a list of projects. The projects are processed, in order, according to the current
# settings of the $defaults object.
function PowerPack
    (
    [string[]] $projects		# Array of projects to build and pack. 
    )
{
    if (!$projects)
    {
        Write-Error "Please specify at least 1 project."
        Exit 1
    }

    # Early check: check that NuGet can be found.
    $nuget = LocateNuGet $true
    if (!$nuget)
    {
        Write-Error "Could not locate NuGet.exe. Please install or download it and put it in the same directory as this script."
        Exit 1
    }

    # Ditto for MSBuild.exe.
    $msbuild = LocateMSBuild
    if (!$msbuild)
    {
        Write-Error "Could not locate MSBuild.exe"
        Exit 1
    }


    foreach ($project in $projects)
    {
		$projectObject = LocateProject $project
		if (!$projectObject)
		{
			Write-Host "Could not locate project $project"
			Exit 1
		}

        PowerPackProject $projectObject
    }
}
#>


$defaults = new-object -type PSObject -Property @{
    ProjectSearchDirectories = @("$PSScriptRoot", "$PSScriptRoot/..");       # Ordered array of places to look for projects.
	Configuration = "Release";          # Configuration to build.
    Platform = "AnyCPU";                # Platform to build.
	MsBuildVerbosity = "minimal";       # Verbosity flag to pass to MSBuild. quiet, minimal and normal are most used.
	NuGetFeed = "LocalNuGetFeed";       # The NuGet feed to push to. You probably don't have this which means your pushes will fail unless you change it (a good thing).
                                        # Set to blank to push to nuget.org.
    PackageDirectory = "$PSScriptRoot"; # Where to place the nupkg files after building them (but before pushing).
    PackOptions = ""                    # Default options to NuGet pack. Set below.
	}


$defaults.PackOptions = @("-Verbosity", "normal", "-Properties", "Configuration=$($defaults.Configuration)")


Export-ModuleMember -Variable defaults
Export-ModuleMember -Function *
