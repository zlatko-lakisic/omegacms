$i = 0
Remove-Item domain_keys -ErrorAction Ignore -Recurse
New-Item -Name "domain_keys" -ItemType "directory"

function generateDomainKey{
    [CmdletBinding()]
    param($domain)

    $keyname = $domain.Replace("*.", "").Replace(".", "_")

    $keyname = "domain_keys\${keyname}.key"

    openssl genrsa -out $keyname 2048
}

$certDomain = if ($env:OMEGA_SSL_CERT_DOMAIN) { $env:OMEGA_SSL_CERT_DOMAIN } else { "*.core.cms.example.com" }
generateDomainKey -domain $certDomain