if (!(Test-Path *.nupkg)) {
	Write-Host "No nupkg file found."
	Exit
}

# Push the package to the official Internet feed.
$nuget = ".\nuget.exe push *.nupkg"
Invoke-Expression $nuget
