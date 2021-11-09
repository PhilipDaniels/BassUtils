$latest = (Get-ChildItem -Attributes !Directory .\BassUtils\bin\Debug\BassUtils*.nupkg | Sort-Object -Descending -Property LastWriteTime | select -First 1)
copy -v $latest c:\nuget

$latest = (Get-ChildItem -Attributes !Directory .\BassUtils.NetCore\bin\Debug\BassUtils.NetCore*.nupkg | Sort-Object -Descending -Property LastWriteTime | select -First 1)
copy -v $latest c:\nuget
