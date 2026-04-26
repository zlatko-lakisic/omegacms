param (
    [string]$platform = $(throw "-platform is required."),
    [string]$mode = $(throw "-mode is required."),
    [string]$version = $(throw "-version is required.")
)
.\download-component.ps1 "webapi.hosted" "$platform" "$mode" "$version"