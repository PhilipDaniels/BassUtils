# Nuke-Package.
# Removes a package from the NuGet package cache.
# This allows it to be re-installed in a project (after a clean)
# without having to bump the version number.

if (!$args)
{
	Write-Host "Specify name(s) of package as command line parameter(s)"
	Write-Host "  e.g. nuke_package.ps1 BassUtils BassUtils.NetCore log4net"
	exit
}

foreach ($package in $args)
{
	$pkgdir = "C:\Users\pdaniels\.nuget\packages\$package"
	
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

