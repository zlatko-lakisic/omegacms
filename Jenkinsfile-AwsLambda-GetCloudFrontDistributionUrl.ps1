$distributionName = $args[0]
$jsonString = aws cloudfront list-distributions
$json = $jsonString | ConvertFrom-Json
$distribution = $json.DistributionList.Items | where { $_.Comment -eq $distributionName }
echo $distribution.DomainName