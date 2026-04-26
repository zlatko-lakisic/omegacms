# Requires TestRun.env (see TestRun.env.example) with AWS/VPC, SMTP, DB, and access keys.
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

$root = $PSScriptRoot
. (Join-Path $root "Powershell\Import-TestRunEnv.ps1")
Import-TestRunEnv -RootPath $root
Assert-TestRunRequiredEnv -Names @(
    "OMEGA_CF_STACK_NAME",
    "OMEGA_CF_DOMAIN",
    "OMEGA_CF_CONTAINER_VERSION",
    "OMEGA_CF_CMS_VERSION",
    "OMEGA_CF_PLUGINS_LAYER_VERSION",
    "OMEGA_CF_OTHER_PLUGINS_ARN",
    "OMEGA_CF_VPC_SECURITY_GROUP_ID",
    "OMEGA_CF_VPC_SUBNET1_ID",
    "OMEGA_CF_VPC_SUBNET2_ID",
    "OMEGA_CF_ACM_CERTIFICATE_ARN",
    "OMEGA_SMTP_USER",
    "OMEGA_SMTP_PASSWORD",
    "OMEGA_SMTP_HOST",
    "OMEGA_SMTP_PORT",
    "OMEGA_SMTP_USE_SSL",
    "OMEGA_CMS_MYSQL_CONNECTION_STRING",
    "OMEGA_CMS_UPLOADS_ACCESS_KEY_ID",
    "OMEGA_CMS_UPLOADS_SECRET_ACCESS_KEY",
    "OMEGA_AWS_REGION"
)

$stackName = $env:OMEGA_CF_STACK_NAME
$stackNameLower = $stackName.ToLower()
$domain = $env:OMEGA_CF_DOMAIN

. (Join-Path $root "Powershell\Create.ps1") `
    -stackName $stackName `
    -containerVersion $env:OMEGA_CF_CONTAINER_VERSION `
    -cmsVersionParam $env:OMEGA_CF_CMS_VERSION `
    -pluginsLayerVersion $env:OMEGA_CF_PLUGINS_LAYER_VERSION `
    -otherPlugins $env:OMEGA_CF_OTHER_PLUGINS_ARN `
    -vpcSecurityGroupId $env:OMEGA_CF_VPC_SECURITY_GROUP_ID `
    -vpcSubnet1Id $env:OMEGA_CF_VPC_SUBNET1_ID `
    -vpcSubnet2Id $env:OMEGA_CF_VPC_SUBNET2_ID `
    -certificateArn $env:OMEGA_CF_ACM_CERTIFICATE_ARN `
    -domain $domain `
    -emailUsername $env:OMEGA_SMTP_USER `
    -emailPassword $env:OMEGA_SMTP_PASSWORD `
    -emailHost $env:OMEGA_SMTP_HOST `
    -emailPort $env:OMEGA_SMTP_PORT `
    -emailSsl $env:OMEGA_SMTP_USE_SSL `
    -connectionString $env:OMEGA_CMS_MYSQL_CONNECTION_STRING `
    -uploadsAccessKey $env:OMEGA_CMS_UPLOADS_ACCESS_KEY_ID `
    -uploadsSecretKey $env:OMEGA_CMS_UPLOADS_SECRET_ACCESS_KEY `
    -regionName $env:OMEGA_AWS_REGION
