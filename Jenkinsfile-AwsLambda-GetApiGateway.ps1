$apiName = $args[0]
$jsonString = aws apigateway get-rest-apis
$json = $jsonString | ConvertFrom-Json
$api = $json.items | where { $_.name -eq $apiName }
echo $api.id