$ErrorActionPreference = 'Stop'

# Ensure version number on command line 
if([String]::IsNullOrEmpty($Args[0])) { Write-Host "Version is required"; exit 1; }
$version = $Args[0];
if($version.StartsWith('v')) { Write-Host "Version must only be a number"; exit 1; }

# Update csproj version number
# Trim starting v --> v0.9.9 --> 0.9.9 because csproj version must be specific x.y.m.n format 
$projPath = get-item "src\CheckAndNotify.csproj"
$csproj = [xml] (get-content $projPath)
$csproj.Project.PropertyGroup.Version = $version
$csProj.Save($projPath)

# Ensure git committed (we will tag and push on release)
# Do after modifying csproj version to catch that update
$changes = $(git status --porcelain)
if(![String]::IsNullOrEmpty($changes)) { Write-Host "Uncommitted changes in git"; exit 1; }

# Invoke the linux build - cwd is transformed by wsl.exe and passes through
wsl -- ./publish.sh $version

# Version it
git tag $version
#git push --tags
Write-Host "Run git push --tags"
