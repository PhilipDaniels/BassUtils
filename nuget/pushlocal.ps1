if (!(Test-Path *.nupkg)) {
	Write-Host "No nupkg file found."
	Exit
}

# Push the package to my local testing feed.
$nuget = ".\nuget.exe push -source LocalNuGetFeed *.nupkg"
Invoke-Expression $nuget
