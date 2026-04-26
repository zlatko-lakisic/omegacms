Param
(
    [Parameter(Mandatory=$true)] [string]$username,
    [Parameter(Mandatory=$true)] [string]$password,
    [Parameter(Mandatory=$true)] [string]$remotecomputer,
    [Parameter(Mandatory=$true)] [string]$stackName,
    [Parameter(Mandatory=$true)] [string]$projectDirectory
)

$pass = ConvertTo-SecureString -AsPlainText $password -Force
$Cred = New-Object System.Management.Automation.PSCredential -ArgumentList $username,$pass

$scriptBlock = {
	param($rstackName,$rprojectDirectory)
	New-Item -ItemType Directory -Force -Path "${rprojectDirectory}\${rstackName}\admin"
    New-Item -ItemType Directory -Force -Path "${rprojectDirectory}\${rstackName}\ws"

    New-WebAppPool -Force -Name "${rstackName}.demo.omegacms.run"
    Set-ItemProperty -Path IIS:\AppPools\"${rstackName}.demo.omegacms.run" managedRuntimeVersion ""
    New-Website -Force -Name "${rstackName}.demo.omegacms.run" -Port 80 -HostHeader "${rstackName}.demo.omegacms.run" -PhysicalPath "${rprojectDirectory}\${rstackName}\admin" -ApplicationPool "${rstackName}.demo.omegacms.run"
    New-WebApplication -Force -Site "${rstackName}.demo.omegacms.run" -Name "ws" -PhysicalPath "${rprojectDirectory}\${rstackName}\ws" -ApplicationPool "${rstackName}.demo.omegacms.run"

    if([System.Diagnostics.EventLog]::SourceExists('${rstackName} Omega Helpers')){
        Remove-EventLog -Source "${rstackName} Omega Helpers"
    }

    if([System.Diagnostics.EventLog]::Exists('${rstackName}-OmegaCMS')){
        Remove-EventLog -LogName "${rstackName}-OmegaCMS"
    }
    New-EventLog -LogName "${rstackName}-OmegaCMS" -Source "${rstackName} Omega Helpers"
}

Invoke-Command -ComputerName $remotecomputer -Credential $Cred -ScriptBlock $scriptBlock -ArgumentList $stackName,$projectDirectory