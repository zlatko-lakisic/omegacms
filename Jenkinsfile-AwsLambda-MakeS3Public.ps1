Param
(
    [Parameter(Mandatory=$true)] [string]$s3bucketname
)

$randomFileName = [GUID]::NewGUID()

$jsonString = @"
{
    "Version": "2008-10-17",
    "Id": "PolicyForCloudFrontPrivateContent",
    "Statement": [
        {
            "Sid": "Allow-Public-Access-To-Bucket",
            "Effect": "Allow",
            "Principal": "*",
            "Action": "s3:GetObject",
            "Resource": "arn:aws:s3:::${s3bucketname}/*"
        }
    ]
}
"@

$randomFileName = "${randomFileName}.json"

[void](New-Item -Path $randomFileName -Value $jsonString)

[void](aws s3api put-bucket-policy --bucket ${s3bucketname} --policy "file://${randomFileName}")

[void](Remove-Item $randomFileName)