New-Item -ItemType Directory -Force -Path ./nuget

$cd = Get-Location

# build libs
Get-ChildItem ".\src" -File -Filter "build.ps1" -Recurse | 
Foreach-Object {
    Set-Location $_.DirectoryName 
    & .\build.ps1 $args

    if ($LASTEXITCODE -ne 0)
    {
        exit $LASTEXITCODE
    }

    $artifacts = $_.DirectoryName + "\artifacts\*.nupkg"
    $sources = $cd.Path + "\nuget"

    Copy-Item -Force -path $artifacts -Destination $sources
}

Set-Location $cd