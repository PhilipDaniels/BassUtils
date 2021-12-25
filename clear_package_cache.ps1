# Clear-Package-Cache
# Removes a package from the NuGet package cache.
# This allows it to be re-installed in a project (after a clean)
# without having to bump the version number.
#
#
# List the location of the global package cache
#     dotnet nuget locals global-packages --list
#
# List the location of all NuGet caches
#     dotnet nuget locals all --list
#
# Clear the global package cache (clears all packages)
#     dotnet nuget locals global-packages --clear
#
# Clear all NuGet caches
#     dotnet nuget locals all --clear


if (!$args)
{
	Write-Host "Specify name(s) of package as command line parameter(s):"
	Write-Host ""
	Write-Host "  ./clear_package_cache.ps1 BassUtils BassUtils.NetCore log4net"
	Write-Host ""
    Write-Host "Alternatively, clear **all** packages from the global cache with:"
	Write-Host ""
    Write-Host "  ./clear_package_cache.ps1 ALL"
	Write-Host ""
	exit
}

# Outputs:    global-packages: C:\Users\phil\.nuget\packages\
$cache_dir = (dotnet nuget locals global-packages --list) | Out-String
$cache_dir = $cache_dir.Split(':', 2)[1].Trim();

if ($args.Length -eq 1 -and $args[0] -eq "ALL")
{
	dotnet nuget locals global-packages --clear
    exit
}


foreach ($package in $args)
{
	$pkgdir = "$cache_dir$package"
	
	if (Test-Path $pkgdir)
	{
		Write-Host "Deleting package directory $pkgdir"
		Remove-Item -Force -Recurse -Path $pkgdir
	}
	else
	{
		Write-Host "Package directory $pkgdir does not exist, nothing to do"
	}
}
