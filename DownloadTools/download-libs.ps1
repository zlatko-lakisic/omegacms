param (
    [string]$mode = $(throw "-mode is required."),
    [string]$version = $(throw "-version is required.")
)
.\download-component.ps1 "md-tools-helpers-core" "portable" "$mode" "$version" $true
.\download-component.ps1 "md-tools-basedataaccess-core" "portable" "$mode" "$version" $true
.\download-component.ps1 "md-tools-basedataaccess-pluginmethods-core" "portable" "$mode" "$version" $true
.\download-component.ps1 "md-tools-basedataaccess-plugins-core" "portable" "$mode" "$version" $true
.\download-component.ps1 "md-cms-businesslogic-core" "portable" "$mode" "$version" $true
.\download-component.ps1 "md-cms-businesslogic-webapi-core" "portable" "$mode" "$version" $true