param (
    [string]$projectName = $(throw "-projectName is required."),
    [string]$platform = $(throw "-platform is required."),
    [string]$mode = $(throw "-mode is required."),
    [string]$version = $(throw "-version is required."),
    [bool]$isLibs = $false
)
#Add-Type -AssemblyName System.Web
#[string]$fileUrlEncoded = [System.Web.HTTPUtility]::UrlEncode("$(if($isLibs) { 'lib/' } else { '' })$projectName/artifact/$projectName.$mode.$platform.$version.7z").replace(".", "%2E")

#wget "http://git.omegacms.io/api/v4/projects/$projectId/repository/files/$fileUrlEncoded/raw?ref=master"  -Headers @{"PRIVATE-TOKEN" = "$token"} -OutFile "$projectName.$mode.7z"

xcopy "\\10.0.10.3\IIS_Shares\CMS\Artifacts\$projectName\$projectName.$mode.$platform.$version.7z" ".\" /y/f/i


# SIG # Begin signature block
# MIIFdgYJKoZIhvcNAQcCoIIFZzCCBWMCAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQUFpYzJ6VUw8CwxJFH0Vz5JIu4
# V06gggMOMIIDCjCCAfKgAwIBAgIQZvu4EC8Z5q5JnauVVGoOFjANBgkqhkiG9w0B
# AQUFADAdMRswGQYDVQQDDBJMb2NhbCBDb2RlIFNpZ25pbmcwHhcNMjAwMjE1MDMw
# MjE4WhcNMjEwMjE1MDMyMjE4WjAdMRswGQYDVQQDDBJMb2NhbCBDb2RlIFNpZ25p
# bmcwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDa6cW9HmZtQmXdHcxK
# sV4cJUfSGgZnTnK+EqqfXMGOYL6d0T8DT0arRySI80GpaWuRObmmMz2WLe0lDFAq
# zNqPRucrsXi9Kb3lD7eO3CGNdQ7iBzjjyUs3psE7Nuq590DIfg/kUBcwcknVQFmJ
# cwY3kZme9yh5jX/zV7fyZpvgTsC5Xt2OFLlsv0Ds9JI/9KeBE6pS05u+yVeOg/hr
# Rnfht5imCbvCzRB6wgJZrCBh0e6WB4stMY1FJwv2X5fNbskciBjM8MWU9jrj6NWz
# uluq7m2UgZuTblz4YuZlwAOfUB9z3w2UhTpcPgvLNHMPIr2jrCLID79dooDV6ohy
# soUdAgMBAAGjRjBEMA4GA1UdDwEB/wQEAwIHgDATBgNVHSUEDDAKBggrBgEFBQcD
# AzAdBgNVHQ4EFgQUVOhNWN1n+uLt9hHAPePsznHS6rUwDQYJKoZIhvcNAQEFBQAD
# ggEBAMPCqR90xdnlRQQ8KZ/w0Vn2vTUbSUYyPCgdcxSJAKlVT6UJ/iZoiMhJ4Rra
# S/9kUVCfq0ozI5WTrjR6vdJhFzaUHj6y7z+TWRsv6h4Zd+66m2h3TZzs8d36AXzW
# U4U0SeCnTNRZIWE6pG80trph2Of75OPaBnmM0btJNE1fHGYIyZ9eYKEEBSo8w+Dd
# QuQLfPTCH7n+Le2TiHvaGITGiN6dami5WkzbXoaqP1H8twMyA6u8IvFgwWHFb5rR
# vrYgvP4hF5bQBVWem9ZRx9wYSMRKzPtJfLYehZ0/BS1pRiX7t3lESN3dqYDirpKW
# IhRcYHX7lY6os9TuFHQk96hBoy4xggHSMIIBzgIBATAxMB0xGzAZBgNVBAMMEkxv
# Y2FsIENvZGUgU2lnbmluZwIQZvu4EC8Z5q5JnauVVGoOFjAJBgUrDgMCGgUAoHgw
# GAYKKwYBBAGCNwIBDDEKMAigAoAAoQKAADAZBgkqhkiG9w0BCQMxDAYKKwYBBAGC
# NwIBBDAcBgorBgEEAYI3AgELMQ4wDAYKKwYBBAGCNwIBFTAjBgkqhkiG9w0BCQQx
# FgQUaFLQz+NtmYjAyK3jBpvKEuFFW6UwDQYJKoZIhvcNAQEBBQAEggEAGZJKjaEd
# BwhBuJ1ISBJTUnUIRytPnTJw3OGt/bZdXMC/BaS0ApT72QrKRlFxGLN9HVqNtpag
# ZLZO99crJz5TGuDCimGSZGnEYAD3TUiY05GiCyO8zzpcjhvhCxtN5kKMPbrPCbQM
# EHX2cXmOgI7TOpssO7KPEzpggKiZr2wsxofmuYMaMHZ18nJnID9WdnyCdDo4uGRv
# WFszDz2g8mu7tsX4Jhg3NJSRnwucZ/gZ1B9pJ2tHmOnCr8JBORKTb7ydf3k+WVdk
# gJM23buGjDR0cMoMkQmZEHqAHIarET8KRqQvScVfSGNRoFVm3rEQQS0ygnZTKfIJ
# bPOf4qjfhntjRw==
# SIG # End signature block
