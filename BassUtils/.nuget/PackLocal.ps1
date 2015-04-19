Import-Module "$PSScriptRoot\PowerPack" -Force

# Edit this file to suit your projects. See the README for more info.
$projects = @("BassUtils")

$projectObjects = LocateProjects $projects

# First clean everything.
foreach ($projectObject in $projectObjects)
{
    DeepCleanProject $projectObject
}

# Then build everything under all configurations we support.
foreach ($projectObject in $projectObjects)
{
    foreach ($config in @("ReleaseNet40", "ReleaseNet45"))
    {
        $options.Configuration = $config
        BuildProject $projectObject
    }
}

# Then pack once.
#foreach ($projectObject in $projectObjects)
#{
#    NuGetPack $projectObject
#    NuGetPush $projectObject
#}

