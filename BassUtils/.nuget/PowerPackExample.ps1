Import-Module "$PSScriptRoot\PowerPack" -Force

# Edit this file to suit your projects. See the README for more info.
$projects = @("Foo.Core", "Foo.Server")
PowerPack $projects



# The easiest way to run this script in Visual Studio is from the NuGet
# Package Manager Console, which is really a PowerShell window.
# From within the .nuget folder, enter:
#
# .\PowerPackExample.ps1
