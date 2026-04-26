$i = 0;
Remove-Item csr -ErrorAction Ignore -Recurse
New-Item -Name "csr" -ItemType "directory"

function generateCSR{
    [CmdletBinding()]
    param($domain)

    Write-Host "Creating CertificateRequest(CSR) for ${domain} `r "

    $path = (Get-Item -Path ".\").FullName;
    $CertName = "${domain}"
    $keyname = $domain.Replace("*.", "").Replace(".", "_")
    $CSRPath = "${path}\csr\${keyname}.csr"
    $INFPath = "${path}\csr\${keyname}.inf"
    $Signature = '$Windows NT$' 

    Remove-Item $CSRPath -ErrorAction Ignore
    Remove-Item $INFPath -ErrorAction Ignore


$INF =
@"
[Version]
Signature= "$Signature" 

[NewRequest]
Subject = "CN=$CertName, OU=Omega CMS, O=Omega IT LLC, L=New York City, S=New York, C=US"
KeySpec = 1
KeyLength = 2048
Exportable = TRUE
MachineKeySet = TRUE
SMIME = False
PrivateKeyArchive = FALSE
UserProtected = FALSE
UseExistingKeySet = FALSE
ProviderName = "Microsoft RSA SChannel Cryptographic Provider"
ProviderType = 12
RequestType = PKCS10
KeyUsage = 0xa0

[EnhancedKeyUsageExtension]

OID=1.3.6.1.5.5.7.3.1 
"@

    write-output "Certificate Request is being generated `r "
    $INF | out-file -filepath $INFPath -force
    certreq -new $INFPath $CSRPath
    write-output "Certificate Request has been generated"
}

$certDomain = if ($env:OMEGA_SSL_CERT_DOMAIN) { $env:OMEGA_SSL_CERT_DOMAIN } else { "*.core.cms.example.com" }
generateCSR -domain $certDomain