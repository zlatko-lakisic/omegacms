if([System.Diagnostics.EventLog]::SourceExists('Omega Helpers')){
    Remove-EventLog -Source "Omega Helpers"
}

if([System.Diagnostics.EventLog]::Exists('OmegaCMS')){
    Remove-EventLog -LogName "OmegaCMS"
}
New-EventLog -LogName "OmegaCMS" -Source "Omega Helpers"
pause