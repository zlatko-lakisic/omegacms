Param
(
    [Parameter(Mandatory=$true)] [string]$framework,
    [Parameter(Mandatory=$true)] [string]$product,
    [Parameter(Mandatory=$true)] [string]$version
)

$version = $version.Replace("v", "").Replace(".", "-")

$jsonString = aws lambda list-layers --compatible-runtime $framework
$json = $jsonString | ConvertFrom-Json
$layers = $json.Layers | Where-Object { $_.LayerName -match "MD-CMS-${product}-Core-AwsLambda-v${version}"} | select -Property LayerArn
echo $layers