$source = @"
using System.IO;

public class Project
{
    public string Name { get; set; }
    public string FileName { get; set; }
    public string Directory { get; set; }
    public string PowerPackDirectory { get; set; }
    public string OutputAssemblyFileName { get; set; }
    public string NuSpecFileName { get; set; }
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

# Finds the csproj for a project name. Searches in various likely places, which can
# be customised by adjusting $options.ProjectSearchDirectories before running.
# Returns an object of type Project which has several useful properties (so we
# can avoid continually calculating them).
function LocateProject
	(
	[string] $projectName		# For example, "Misc.Core"
	)
{
	$projectFile = "$projectName.csproj"

    # Try the search paths as supplied.
	$f = LocateFile $projectFile $options.ProjectSearchDirectories
	if (!$f)
	{
        # Try the search paths with the project name on the end as well. (The Foo project is usually in a folder as Foo\Foo.csproj
        # so this is the most likely match).
    	$f = LocateFile "$projectName\$projectFile" $options.ProjectSearchDirectories
        if ($f)
        {
            $projectObject = New-Object -type Project
            $projectObject.Name = $projectName
            $projectObject.FileName = $f
            $projectObject.Directory = (Split-Path -Parent $f);
            $projectObject.PowerPackDirectory = Join-Path -Path $projectObject.Directory -ChildPath ($options.OutputDirectory)
            $projectObject.NuSpecFileName = (LocateNuSpec $projectObject)

            return $projectObject
        }
	}
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
    [bool] $build              # If true, does a build.
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

    return @(
        $($projectObject.FileName),
        "/target:$targets",
        "/nologo",
        "/verbosity:$($options.MsBuildVerbosity)",
        "/p:Configuration=$($options.Configuration)",
        "/p:PlatformTarget=$($options.Platform)",
        "/p:RunCodeAnalysis=False",
        "/p:OutDir=$($options.OutputDirectory)"
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

# Does an MSBuild clean and for good measure removes the PowerPack output folder
# because it will usually contain the nupkg files.
function CleanProject
    (
	$projectObject            # From LocateProject.
	)
{
    $msbuild = LocateMsBuild
    $args = GetMSBuildArguments $projectObject $true $false
    Write-Host "Cleaning $($projectObject.FileName)"
    &$msbuild $args

	if ($lastExitCode -ne 0)
    {
        Write-Error "Error while running MSBuild for clean."
        Exit 1
    }

    # Go from PowerPack\Release to PowerPack.
    $pn = Split-Path -Parent $projectObject.PowerPackDirectory
    if (Test-Path $pn)
    {
        DeleteDirectory $pn
    }

    Write-Host "Cleaning $($projectObject.FileName) complete"
}

# Builds the specified project. Gets put in the special PowerPack output folder.
# It is recommended that you do a clean first.
function BuildProject
	(
	$projectObject            # From LocateProject.
	)
{
    $msbuild = LocateMsBuild
    $args = GetMSBuildArguments $projectObject $false $true
    Write-Host "Building $($projectObject.FileName): MSBuild.exe $args"
    &$msbuild $args

	if ($lastExitCode -ne 0)
    {
        Write-Error "Error while running MSBuild."
        Exit 1
    }

    Write-Host "Building $($projectObject.FileName) complete"
}

# Locates the output assembly for a project and stores it on the project object. It looks for the output
# in $projectObject.PowerPackDirectory. We use a known path when building because it is very difficult
# to tell from the project file where the assembly will be put.
function SetOutputAssemblyFileName
    (
    $projectObject            # From LocateProject.
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

# Returns the directory in which the nupkg files should be put.
function GetPackageDirectory
    (
    $projectObject            # From LocateProject.
    )
{
    if ($options.PackageDirectory)
    {
        return $options.PackageDirectory
    }
    else
    {
        return $projectObject.PowerPackDirectory
    }
}

# Gets all the nupkg filenames that exist for the specified project. Since we have no way of knowing
# the exact name of the package (it depends on the version number it was built with and it may not
# have the same name as the project) this function can return 0, 1 (typically) or several filenames.
function GetExistingNuGetPackageFilenames
    (
    $projectObject            # From LocateProject.
    )
{
    $packageDir = GetPackageDirectory $projectObject
    $filter = "*.nupkg"
    $filenames = Get-ChildItem -Path $packageDir -Filter $filter
    return $filenames
}

# Lists the nupkg files that exist for a project in its package output directory.
function NuGetList
    (
    $projectObject            # From LocateProject.
    )
{
    $files = GetExistingNuGetPackageFilenames $projectObject
    foreach ($file in $files)
    {
        Write-Host $file.Fullname
    }
}

# Removes one or more nupkg file for the specified project.
# The files to delete are determined by calling GetExistingNuGetPackageFilenames.
function NuGetClean
    (
    $projectObject            # From LocateProject.
    )
{
    $files = GetExistingNuGetPackageFilenames $projectObject
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
    [string] $version         # Version to use. Optional, in which case the version from the assembly is used.
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
    $packageDir = GetPackageDirectory $projectObject
    if (!$packageDir)
    {
        Write-Error "Could not determine output directory for NuGet pack for project $($projectObject.Name)"
        Exit 1
    }

    $args = @("pack", $projectObject.NuSpecFileName, "-Version", "$version", "-OutputDirectory", "$packageDir") + $options.PackOptions
    Write-Host "Running NuGet pack for $($projectObject.Name): NuGet.exe $args"

    &$nuget $args

	if ($lastExitCode -ne 0)
    {
        Write-Error "Error while running NuGet pack."
        Exit 1
    }

    Write-Host "NuGet pack for $($projectObject.Name) completed"
}

# Publish the nupkg for a project. The package file is pushed to the NuGetFeed specified in the options object.
# This function deals with multiple files because we have no way of knowing what the full nupkg filename is,
# it depends on the version that was used to build it. However, typically there will only be 1 because you do
# a clean before a build.
function NuGetPush
    (
    $projectObject            # From LocateProject.
    )
{
    $nuget = LocateNuGet $true
    if (!$nuget)
    {
        Write-Error "Could not locate NuGet.exe. Please install or download it and put it in the same directory as this script."
        Exit 1
    }

    $packages = GetExistingNuGetPackageFilenames $projectObject
    if (!$packages)
    {
        Write-Error "No packages were found for $($projectObject.Name)"
        Exit 1
    }

    # Push the package to the configured feed.
    foreach ($package in $packages)
    {
        if ($options.NuGetFeed)
        {
            $args = @("push", "-Source", "$($options.NuGetFeed)", "$($package.FullName)")
        }
        else
        {
            $args = @("push", "$($package.FullName)")
        }

        Write-Host "Running NuGet push for $($projectObject.Name): NuGet.exe $args"
        &$nuget $args
        Write-Host "NuGet push to $($options.NuGetFeed) of $($package.FullName) completed"
    }
}

function PowerPackProject
    (
    $projectObject            # From LocateProject.
    )
{
    if ($options.DoClean)
    {
        CleanProject $projectObject
    }
    if ($options.DoBuild)
    {
        BuildProject $projectObject
    }
    if ($options.DoPack)
    {
        NuGetPack $projectObject
    }
    if ($options.DoPush)
    {
        NuGetPush $projectObject
    }
}

# Process a list of projects. The projects are processed, in order, according to the current
# settings of the $options object.
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

$options = new-object -type PSObject -Property @{
    ProjectSearchDirectories = @("$PSScriptRoot", "$PSScriptRoot/..");       # Ordered array of places to look for projects.
	Configuration = "Release";          # Configuration to build.
    Platform = "AnyCPU";                # Platform to build.
	OutputDirectory = ""                # We put the build artifacts into a well-known place so that we can locate them more easily later. See below.
	MsBuildVerbosity = "minimal";       # Verbosity flag to pass to MSBuild. quiet, minimal and normal are most used.
	NuGetFeed = "LocalNuGetFeed";       # The NuGet feed to push to. You probably don't have this which means your pushes will fail unless you change it (a good thing).
                                        # Set to blank to push to nuget.org.
    PackageDirectory = "";              # Where to place the nupkg files. Blank means "same as OutputDirectory".
    PackOptions = ""                    # Default options to NuGet pack. Set below.

    # Things you can do.
    DoClean = $true;                    # Clean projects before building them (recommended).
    DoBuild = $true;                    # Build projects.
    DoPack = $true;                     # Pack projects into .nupkg files using their .nuspec files.
    DoPush = $false;                    # Push .nupkg files to the configured NuGet feed.
	}


$options.OutputDirectory = "bin\PowerPack\$($options.Configuration)"

$options.PackOptions = @(
    "-Verbosity",
    "normal",
    "-NoPackageAnalysis",
    "-Properties",
    "Configuration=$($options.Configuration)"
    )


Export-ModuleMember -Variable options
Export-ModuleMember -Function *
