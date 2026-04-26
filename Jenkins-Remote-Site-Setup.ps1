Param
(
    [Parameter(Mandatory=$true)] [string]$stackName,
    [Parameter(Mandatory=$true)] [string]$projectDirectory
)

New-Item -ItemType Directory -Force -Path "${projectDirectory}\${stackName}\admin"
New-Item -ItemType Directory -Force -Path "${projectDirectory}\${stackName}\ws"

New-WebAppPool -Force -Name "${stackName}.demo.omegacms.run"
Set-ItemProperty -Path IIS:\AppPools\"${stackName}.demo.omegacms.run" managedRuntimeVersion "No Managed Code"
New-Website -Force -Name "${stackName}.demo.omegacms.run" -Port 80 -HostHeader "${stackName}.demo.omegacms.run" -PhysicalPath "${projectDirectory}\${stackName}\admin" -ApplicationPool "${stackName}.demo.omegacms.run"
New-WebApplication -Force -Site "${stackName}.demo.omegacms.run" -Name "ws" -PhysicalPath "${projectDirectory}\${stackName}\ws" -ApplicationPool "${stackName}.demo.omegacms.run"

if([System.Diagnostics.EventLog]::SourceExists('${stackName} Omega Helpers')){
    Remove-EventLog -Source "${stackName} Omega Helpers"
}

if([System.Diagnostics.EventLog]::Exists('${stackName}-OmegaCMS')){
    Remove-EventLog -LogName "${stackName}-OmegaCMS"
}
New-EventLog -LogName "${stackName}-OmegaCMS" -Source "${stackName} Omega Helpers"