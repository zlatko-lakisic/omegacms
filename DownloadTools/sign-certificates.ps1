$cert = @(Get-ChildItem cert:\CurrentUser\My -codesigning)[0]
Set-AuthenticodeSignature download-component.ps1 $cert
Set-AuthenticodeSignature download-administration.ps1 $cert
Set-AuthenticodeSignature download-webapi.ps1 $cert
Set-AuthenticodeSignature download-libs.ps1 $cert