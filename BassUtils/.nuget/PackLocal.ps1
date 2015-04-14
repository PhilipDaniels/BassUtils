Import-Module "$PSScriptRoot\PowerPack" -Force

# Edit this file to suit your projects. See the README for more info.
$projects = @("BassUtils")
$options.DoPush = $true
PowerPack $projects

