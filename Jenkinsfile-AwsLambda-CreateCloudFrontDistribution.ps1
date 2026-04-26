Param
(
    [Parameter(Mandatory=$true)] [string]$billingGroup,
    [Parameter(Mandatory=$true)] [string]$distributionname,
    [Parameter(Mandatory=$true)] [string]$distributionalias,
    [Parameter(Mandatory=$true)] [string]$distributionsslarn,
    [Parameter(Mandatory=$true)] [string]$adminlambdaurl,
    [Parameter(Mandatory=$true)] [string]$wslambdaurl,
    [Parameter(Mandatory=$true)] [string]$wssocketlambdaurl,
    [Parameter(Mandatory=$true)] [string]$s3adminassetsurl,
    [Parameter(Mandatory=$true)] [string]$s3uploadsurl,
    [Parameter(Mandatory=$false)] [string]$adminpath,
    [Parameter(Mandatory=$true)] [string]$wspath,
    [Parameter(Mandatory=$true)] [string]$wssocketpath,
    [Parameter(Mandatory=$false)] [string]$adminoriginpath = "/Prod",
    [Parameter(Mandatory=$false)] [string]$wsoriginpath = "/Prod",
    [Parameter(Mandatory=$false)] [string]$wssocketoriginpath = ""
)

$randomFileName = [GUID]::NewGUID()

$jsonString = @"
{
    "DistributionConfigWithTags": {
        "Tags": {
            "Items": [
                {
                    "Key": "BillingGroup",
                    "Value": "${billingGroup}"
                }
            ]
        },
        "DistributionConfig": {
            "CallerReference": "${randomFileName}",
            "Aliases": {
                "Quantity": 1,
                "Items": [
                    "${distributionalias}"
                ]
            },
            "DefaultRootObject": "",
            "Origins": {
                "Quantity": 5,
                "Items": [
                    {
                        "Id": "S3-omega-cms-admin-assets",
                        "DomainName": "${s3adminassetsurl}",
                        "OriginPath": "",
                        "CustomHeaders": {
                            "Quantity": 0,
                            "Items": []
                        },
                        "S3OriginConfig": {
                            "OriginAccessIdentity": ""
                        }
                    },
                    {
                        "Id": "S3-omega-cms-uploads",
                        "DomainName": "${s3uploadsurl}",
                        "OriginPath": "",
                        "CustomHeaders": {
                            "Quantity": 0,
                            "Items": []
                        },
                        "S3OriginConfig": {
                            "OriginAccessIdentity": ""
                        }
                    },
                    {
                        "Id": "Custom-omega-admin-lambda",
                        "DomainName": "${adminlambdaurl}",
                        "OriginPath": "${adminoriginpath}",
                        "CustomHeaders": {
                            "Quantity": 0,
                            "Items": []
                        },
                        "CustomOriginConfig": {
                            "HTTPPort": 80,
                            "HTTPSPort": 443,
                            "OriginProtocolPolicy": "https-only",
                            "OriginSslProtocols": {
                                "Quantity": 1,
                                "Items": [
                                    "TLSv1.1"
                                ]
                            },
                            "OriginReadTimeout": 60,
                            "OriginKeepaliveTimeout": 5
                        }
                    },
                    {
                        "Id": "Custom-omega-ws-lambda",
                        "DomainName": "${wslambdaurl}",
                        "OriginPath": "${wsoriginpath}",
                        "CustomHeaders": {
                            "Quantity": 0,
                            "Items": []
                        },
                        "CustomOriginConfig": {
                            "HTTPPort": 80,
                            "HTTPSPort": 443,
                            "OriginProtocolPolicy": "https-only",
                            "OriginSslProtocols": {
                                "Quantity": 1,
                                "Items": [
                                    "TLSv1.1"
                                ]
                            },
                            "OriginReadTimeout": 60,
                            "OriginKeepaliveTimeout": 5
                        }
                    },
                    {
                        "Id": "Custom-omega-ws-socket-lambda",
                        "DomainName": "${wssocketlambdaurl}",
                        "OriginPath": "${wssocketoriginpath}",
                        "CustomHeaders": {
                            "Quantity": 0,
                            "Items": []
                        },
                        "CustomOriginConfig": {
                            "HTTPPort": 80,
                            "HTTPSPort": 443,
                            "OriginProtocolPolicy": "https-only",
                            "OriginSslProtocols": {
                                "Quantity": 1,
                                "Items": [
                                    "TLSv1.1"
                                ]
                            },
                            "OriginReadTimeout": 60,
                            "OriginKeepaliveTimeout": 5
                        }
                    }
                ]
            },
            "OriginGroups": {
                "Quantity": 0,
                "Items": []
            },
            "DefaultCacheBehavior": {
                "TargetOriginId": "Custom-omega-admin-lambda",
                "ForwardedValues": {
                    "QueryString": false,
                    "Cookies": {
                        "Forward": "none",
                        "WhitelistedNames": {
                            "Quantity": 0,
                            "Items": []
                        }
                    },
                    "Headers": {
                        "Quantity": 0,
                        "Items": []
                    },
                    "QueryStringCacheKeys": {
                        "Quantity": 0,
                        "Items": []
                    }
                },
                "TrustedSigners": {
                    "Enabled": false,
                    "Quantity": 0,
                    "Items": []
                },
                "ViewerProtocolPolicy": "redirect-to-https",
                "MinTTL": 0,
                "AllowedMethods": {
                    "Quantity": 2,
                    "Items": [
                        "GET",
                        "HEAD"
                    ],
                    "CachedMethods": {
                        "Quantity": 2,
                        "Items": [
                            "GET",
                            "HEAD"
                        ]
                    }
                },
                "SmoothStreaming": false,
                "DefaultTTL": 0,
                "MaxTTL": 0,
                "Compress": true,
                "LambdaFunctionAssociations": {
                    "Quantity": 0,
                    "Items": []
                },
                "FieldLevelEncryptionId": ""
            },
            "CacheBehaviors": {
                "Quantity": 7,
                "Items": [
                    {
                        "PathPattern": "${wspath}/*",
                        "TargetOriginId": "Custom-omega-ws-lambda",
                        "ForwardedValues": {
                            "QueryString": true,
                            "Cookies": {
                                "Forward": "none",
                                "WhitelistedNames": {
                                    "Quantity": 0,
                                    "Items": []
                                }
                            },
                            "Headers": {
                                "Quantity": 4,
                                "Items": [
                                    "authorization",
                                    "administration",
                                    "lcid",
                                    "content-type"
                                ]
                            },
                            "QueryStringCacheKeys": {
                                "Quantity": 0,
                                "Items": []
                            }
                        },
                        "TrustedSigners": {
                            "Enabled": false,
                            "Quantity": 0,
                            "Items": []
                        },
                        "ViewerProtocolPolicy": "redirect-to-https",
                        "MinTTL": 0,
                        "AllowedMethods": {
                            "Quantity": 7,
                            "Items": [
                                "GET", 
                                "HEAD", 
                                "OPTIONS", 
                                "PUT", 
                                "POST", 
                                "PATCH", 
                                "DELETE"
                            ],
                            "CachedMethods": {
                                "Quantity": 3,
                                "Items": [
                                    "GET", 
                                    "HEAD", 
                                    "OPTIONS"
                                ]
                            }
                        },
                        "SmoothStreaming": false,
                        "DefaultTTL": 0,
                        "MaxTTL": 0,
                        "Compress": true,
                        "LambdaFunctionAssociations": {
                            "Quantity": 0,
                            "Items": []
                        },
                        "FieldLevelEncryptionId": ""
                    },
                    {
                        "PathPattern": "/scripts/documentation/*",
                        "TargetOriginId": "S3-omega-cms-admin-assets",
                        "ForwardedValues": {
                            "QueryString": false,
                            "Cookies": {
                                "Forward": "none",
                                "WhitelistedNames": {
                                    "Quantity": 0,
                                    "Items": []
                                }
                            },
                            "Headers": {
                                "Quantity": 0,
                                "Items": []
                            },
                            "QueryStringCacheKeys": {
                                "Quantity": 0,
                                "Items": []
                            }
                        },
                        "TrustedSigners": {
                            "Enabled": false,
                            "Quantity": 0,
                            "Items": []
                        },
                        "ViewerProtocolPolicy": "redirect-to-https",
                        "MinTTL": 0,
                        "AllowedMethods": {
                            "Quantity": 2,
                            "Items": [
                                "GET",
                                "HEAD"
                            ],
                            "CachedMethods": {
                                "Quantity": 2,
                                "Items": [
                                    "GET",
                                    "HEAD"
                                ]
                            }
                        },
                        "SmoothStreaming": false,
                        "DefaultTTL": 0,
                        "MaxTTL": 0,
                        "Compress": true,
                        "LambdaFunctionAssociations": {
                            "Quantity": 0,
                            "Items": []
                        },
                        "FieldLevelEncryptionId": ""
                    },
                    {
                        "PathPattern": "/assets/*",
                        "TargetOriginId": "S3-omega-cms-admin-assets",
                        "ForwardedValues": {
                            "QueryString": false,
                            "Cookies": {
                                "Forward": "none",
                                "WhitelistedNames": {
                                    "Quantity": 0,
                                    "Items": []
                                }
                            },
                            "Headers": {
                                "Quantity": 0,
                                "Items": []
                            },
                            "QueryStringCacheKeys": {
                                "Quantity": 0,
                                "Items": []
                            }
                        },
                        "TrustedSigners": {
                            "Enabled": false,
                            "Quantity": 0,
                            "Items": []
                        },
                        "ViewerProtocolPolicy": "redirect-to-https",
                        "MinTTL": 0,
                        "AllowedMethods": {
                            "Quantity": 2,
                            "Items": [
                                "GET",
                                "HEAD"
                            ],
                            "CachedMethods": {
                                "Quantity": 2,
                                "Items": [
                                    "GET",
                                    "HEAD"
                                ]
                            }
                        },
                        "SmoothStreaming": false,
                        "DefaultTTL": 0,
                        "MaxTTL": 0,
                        "Compress": true,
                        "LambdaFunctionAssociations": {
                            "Quantity": 0,
                            "Items": []
                        },
                        "FieldLevelEncryptionId": ""
                    },
                    {
                        "PathPattern": "/css/*",
                        "TargetOriginId": "S3-omega-cms-admin-assets",
                        "ForwardedValues": {
                            "QueryString": false,
                            "Cookies": {
                                "Forward": "none",
                                "WhitelistedNames": {
                                    "Quantity": 0,
                                    "Items": []
                                }
                            },
                            "Headers": {
                                "Quantity": 0,
                                "Items": []
                            },
                            "QueryStringCacheKeys": {
                                "Quantity": 0,
                                "Items": []
                            }
                        },
                        "TrustedSigners": {
                            "Enabled": false,
                            "Quantity": 0,
                            "Items": []
                        },
                        "ViewerProtocolPolicy": "redirect-to-https",
                        "MinTTL": 0,
                        "AllowedMethods": {
                            "Quantity": 2,
                            "Items": [
                                "GET",
                                "HEAD"
                            ],
                            "CachedMethods": {
                                "Quantity": 2,
                                "Items": [
                                    "GET",
                                    "HEAD"
                                ]
                            }
                        },
                        "SmoothStreaming": false,
                        "DefaultTTL": 0,
                        "MaxTTL": 0,
                        "Compress": true,
                        "LambdaFunctionAssociations": {
                            "Quantity": 0,
                            "Items": []
                        },
                        "FieldLevelEncryptionId": ""
                    },
                    {
                        "PathPattern": "/js/*",
                        "TargetOriginId": "S3-omega-cms-admin-assets",
                        "ForwardedValues": {
                            "QueryString": false,
                            "Cookies": {
                                "Forward": "none",
                                "WhitelistedNames": {
                                    "Quantity": 0,
                                    "Items": []
                                }
                            },
                            "Headers": {
                                "Quantity": 0,
                                "Items": []
                            },
                            "QueryStringCacheKeys": {
                                "Quantity": 0,
                                "Items": []
                            }
                        },
                        "TrustedSigners": {
                            "Enabled": false,
                            "Quantity": 0,
                            "Items": []
                        },
                        "ViewerProtocolPolicy": "redirect-to-https",
                        "MinTTL": 0,
                        "AllowedMethods": {
                            "Quantity": 2,
                            "Items": [
                                "GET",
                                "HEAD"
                            ],
                            "CachedMethods": {
                                "Quantity": 2,
                                "Items": [
                                    "GET",
                                    "HEAD"
                                ]
                            }
                        },
                        "SmoothStreaming": false,
                        "DefaultTTL": 0,
                        "MaxTTL": 0,
                        "Compress": true,
                        "LambdaFunctionAssociations": {
                            "Quantity": 0,
                            "Items": []
                        },
                        "FieldLevelEncryptionId": ""
                    },
                    {
                        "PathPattern": "/uploads/*",
                        "TargetOriginId": "S3-omega-cms-uploads",
                        "ForwardedValues": {
                            "QueryString": false,
                            "Cookies": {
                                "Forward": "none",
                                "WhitelistedNames": {
                                    "Quantity": 0,
                                    "Items": []
                                }
                            },
                            "Headers": {
                                "Quantity": 0,
                                "Items": []
                            },
                            "QueryStringCacheKeys": {
                                "Quantity": 0,
                                "Items": []
                            }
                        },
                        "TrustedSigners": {
                            "Enabled": false,
                            "Quantity": 0,
                            "Items": []
                        },
                        "ViewerProtocolPolicy": "redirect-to-https",
                        "MinTTL": 0,
                        "AllowedMethods": {
                            "Quantity": 2,
                            "Items": [
                                "GET",
                                "HEAD"
                            ],
                            "CachedMethods": {
                                "Quantity": 2,
                                "Items": [
                                    "GET",
                                    "HEAD"
                                ]
                            }
                        },
                        "SmoothStreaming": false,
                        "DefaultTTL": 0,
                        "MaxTTL": 0,
                        "Compress": true,
                        "LambdaFunctionAssociations": {
                            "Quantity": 0,
                            "Items": []
                        },
                        "FieldLevelEncryptionId": ""
                    },
                    {
                        "PathPattern": "${wssocketpath}/*",
                        "TargetOriginId": "Custom-omega-ws-socket-lambda",
                        "ForwardedValues": {
                            "QueryString": true,
                            "Cookies": {
                                "Forward": "none",
                                "WhitelistedNames": {
                                    "Quantity": 0,
                                    "Items": []
                                }
                            },
                            "Headers": {
                                "Quantity": 0,
                                "Items": []
                            },
                            "QueryStringCacheKeys": {
                                "Quantity": 0,
                                "Items": []
                            }
                        },
                        "TrustedSigners": {
                            "Enabled": false,
                            "Quantity": 0,
                            "Items": []
                        },
                        "ViewerProtocolPolicy": "redirect-to-https",
                        "MinTTL": 0,
                        "AllowedMethods": {
                            "Quantity": 7,
                            "Items": [
                                "GET", 
                                "HEAD", 
                                "OPTIONS", 
                                "PUT", 
                                "POST", 
                                "PATCH", 
                                "DELETE"
                            ],
                            "CachedMethods": {
                                "Quantity": 3,
                                "Items": [
                                    "GET", 
                                    "HEAD", 
                                    "OPTIONS"
                                ]
                            }
                        },
                        "SmoothStreaming": false,
                        "DefaultTTL": 0,
                        "MaxTTL": 0,
                        "Compress": true,
                        "LambdaFunctionAssociations": {
                            "Quantity": 0,
                            "Items": []
                        },
                        "FieldLevelEncryptionId": ""
                    }
                ]
            },
            "CustomErrorResponses": {
                "Quantity": 0,
                "Items": []
            },
            "Comment": "${distributionname}",
            "Logging": {
                "Enabled": false,
                "IncludeCookies": false,
                "Bucket": "",
                "Prefix": ""
            },
            "PriceClass": "PriceClass_All",
            "Enabled": true,
            "ViewerCertificate": {
                "CloudFrontDefaultCertificate": false,
                "ACMCertificateArn": "${distributionsslarn}",
                "MinimumProtocolVersion": "TLSv1.2_2019",
                "SSLSupportMethod": "sni-only"
            },
            "WebACLId": "",
            "HttpVersion": "http2",
            "IsIPV6Enabled": true
        }
    }
}
"@

$randomFileName = "${randomFileName}.json"

[void](New-Item -Path $randomFileName -Value $jsonString)

[void](aws cloudfront create-distribution-with-tags --cli-input-json "file://${randomFileName}")

[void](Remove-Item $randomFileName)