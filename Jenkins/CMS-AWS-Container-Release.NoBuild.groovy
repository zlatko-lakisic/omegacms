adminVersion = 0
webApiVersion = 0

buildProfile = ""
baseArtifactDirectory = ""
nugetKey = ""

genericTools = null
dotNetTools = null
cmsTools = null

uniqueId = ''
stackName = ''
setupDbUser = ''
setupDbPassword = ''
dbUser = ''
dbPassword = ''
dbName = ''
dbServerAddress = ''
dbServerPort = ''
omegaAdminS3BucketName = ''
omegaWsS3BucketName = ''
omegaWsSocketS3BucketName = ''
omegaAdminUrl = ''
omegaWsUrl = ''
omegaWsSocketUrl = ''
omegaAdminStackName = ''
omegaWsStackName = ''
omegaWsSocketStackName = ''
omegaAdminApiGatewayUrl = ''
omegaWsApiGatewayUrl = ''
omegaWsSocketApiGatewayUrl = ''
omegaAdminAssetsS3BucketName = ''
omegaUploadsS3BucketName = ''
omegaPluginsS3BucketName = ''
omegaRdsSecurityGroupName = ''
omegaRdsSubnetGroupName = ''
omegaRdsParameterGroupName = ''
projectDirectory = 'E:'
vpcId = ''
subnet1VpcId = ''
subnet2VpcId = ''
igwId = ""
subnet1Id = ''
subnet2Id = ''
subnetAclId = ''
subnetRouteTableId = ''
rdsSecurityGroupId = ''
vpcSecurityGroupId = ''
rollbackRequired = false
rollbackException = null
rdsDbInstance = null
subnet1Exists = false
subnet2Exists = false
awsReturnedEndpoint = false
forceRollback = false
createNewRds = false
stackRegion = ""
cloudFrontDistributionName = ""
cloudFrontAdminPath = ""
cloudFrontWsPath = ""
cloudFrontWsSocketPath = ""
cloudFrontDistributionUrl = ""
createSingleS3LambdaBucket = false
singleS3LambdaBucketName = ""
singleS3PublicBucketName = ""
cms_googleApiKey = ""
cms_lcid = 0
cms_emailEnabled = false
cms_emailHost = ""
cms_emailPort = 0
cms_emailUseSsl = false
cms_emailUsername = ""
cms_emailPassword = ""
cms_rootAccountEnabled = false
cms_rootAccountUsername = ""
cms_rootAccountPassword = ""
clientName = ""
clientId = 0
cloudFrontSslArn = ""
customDomain = ""

def getCmsVersions(){
    def gitUrl = System.getenv("OMEGA_GIT_URL")
    if (gitUrl == null || gitUrl.trim() == "")
        error("OMEGA_GIT_URL is not set. Add it in Jenkins (see Jenkins/omega-pipeline.env.example) or copy from .env.example.")
    def gettags = ("git ls-remote -t ${gitUrl}").execute()
    return gettags.text.readLines().collect { 
        it.split()[1].replaceAll('refs/heads/', '').replaceAll('refs/tags/', '').replaceAll("\\^\\{\\}", '').replaceAll("release-", "").toString()
    }.reverse()
}

def stage_variableSetup(params){
    if (params.version == null || params.version == "") {
        error("CMS Version Required!")
    }

    if (params.vpcId == null || params.vpcId == "") {
        error("AWS VPC ID Required!")
    }
    if (params.igwId == null || params.igwId == "") {
        error("AWS IGW ID Required!")
    }

    if (params.subnet1VpcCidrBlock == null || params.subnet1VpcCidrBlock == "") {
        error("AWS Lambda VPC Subnet 1 CIDR Required")
    }

    if (params.subnet2VpcCidrBlock == null || params.subnet2VpcCidrBlock == "") {
        error("AWS Lambda VPC Subnet 2 CIDR Required")
    }

    if (params.createNewRds && (params.rdsSubnet1CidrBlock == null || params.rdsSubnet1CidrBlock == "")) {
        error("AWS RDS Subnet 1 CIDR Required")
    }

    if (params.createNewRds && (params.rdsSubnet2CidrBlock == null || params.rdsSubnet2CidrBlock == "")) {
        error("AWS RDS Subnet 2 CIDR Required")
    }

    if (params.rdsSubnet1CidrBlock == null || params.rdsSubnet1CidrBlock == "") {
        error("AWS RDS Subnet 1 CIDR Required")
    }

    if (params.rdsSubnet2CidrBlock == null || params.rdsSubnet2CidrBlock == "") {
        error("AWS RDS Subnet 2 CIDR Required")
    }

    buildProfile = params.buildProfile
    baseArtifactDirectory = params.artifactDirectory
    nugetKey = params.nugetKey

    clientName = params.clientName
    clientId = params.clientId

    vpcId = params.vpcId
    igwId = params.igwId

    forceRollback = params.forceRollback

    uniqueId = genericTools.getUniqueId()
    stackName = "${params.stackName}-${uniqueId}"
    dbUser = genericTools.trim("${params.stackName}${genericTools.getUniqueId()}".replace(".", "").replace("-", "").toLowerCase(), 15)
    dbPassword = genericTools.getUniqueId()
    projectDirectory = "${projectDirectory}\\cms-${uniqueId}"
    publishDirectory = "${projectDirectory}-dist"

    setupDbUser = params.setupDbUser
    setupDbPassword = params.setupDbPassword

    cms_googleApiKey = params.googleApiKey
    cms_lcid = params.lcid
    cms_emailEnabled = params.cms_emailEnabled
    cms_emailHost = params.emailHost
    cms_emailPort = params.emailPort
    cms_emailUseSsl = params.emailUseSsl
    cms_emailUsername = params.emailUsername
    cms_emailPassword = params.emailPassword
    cms_rootAccountEnabled = params.rootAccountEnabled
    cms_rootAccountUsername = params.rootAccountUsername
    cms_rootAccountPassword = params.rootAccountPassword

    createSingleS3LambdaBucket = params.createSingleS3LambdaBucket

    cloudFrontDistributionName = stackName
    if(params.cloudFrontDistributionName != null && params.cloudFrontDistributionName != ""){
        cloudFrontDistributionName = params.cloudFrontDistributionName
    }
    cloudFrontAdminPath = params.cloudFrontAdminPath
    cloudFrontWsPath = params.cloudFrontWsPath
    cloudFrontWsSocketPath = params.cloudFrontWsSocketPath

    omegaAdminStackName = genericTools.trim("lambda-${params.stackName}Admin-${uniqueId}".replace(".", "-").toLowerCase(), 35)
    omegaWsStackName = genericTools.trim("lambda-${params.stackName}Ws-${uniqueId}".replace(".", "-").toLowerCase(), 35)
    omegaWsSocketStackName = genericTools.trim("lambda-${params.stackName}WsSockets-${uniqueId}".replace(".", "-").toLowerCase(), 35)

    if(createSingleS3LambdaBucket){
        singleS3LambdaBucketName = genericTools.trim("lambda-${params.stackName}-v${params.version}-${uniqueId}".replace(".", "-").toLowerCase(), 62)
        singleS3PublicBucketName = genericTools.trim("public-${params.stackName}-v${params.version}-${uniqueId}".replace(".", "-").toLowerCase(), 62)
        omegaAdminS3BucketName = singleS3LambdaBucketName
        omegaWsS3BucketName = omegaWsS3BucketName
        omegaWsSocketS3BucketName = omegaWsSocketS3BucketName
        omegaAdminAssetsS3BucketName = singleS3PublicBucketName
        omegaUploadsS3BucketName = singleS3PublicBucketName
    } else {
        omegaAdminS3BucketName = genericTools.trim("lambda-${params.stackName}Admin-v${params.version}-${uniqueId}".replace(".", "-").toLowerCase(), 62)
        omegaWsS3BucketName = genericTools.trim("lambda-${params.stackName}Ws-v${params.version}-${uniqueId}".replace(".", "-").toLowerCase(), 62)
        omegaWsSocketS3BucketName = genericTools.trim("lambda-${params.stackName}WsSockets-v${params.version}-${uniqueId}".replace(".", "-").toLowerCase(), 62)
        omegaAdminAssetsS3BucketName = genericTools.trim("${params.stackName}AdminAssets-v${params.version}-${uniqueId}".replace(".", "-").toLowerCase(), 62)
        omegaUploadsS3BucketName = genericTools.trim("${params.stackName}Uploads-v${params.version}-${uniqueId}".replace(".", "-").toLowerCase(), 62)
    }
    omegaPluginsS3BucketName = genericTools.trim("${params.stackName}Plugins-v${params.version}-${uniqueId}".replace(".", "-").toLowerCase(), 62)

    omegaVpcSecurityGroupName = genericTools.trim("${params.stackName}-vpc-securitygroup-${uniqueId}".replace(".", "-").toLowerCase(), 62)

    omegaRdsSecurityGroupName = genericTools.trim("${params.stackName}-rds-securitygroup-${uniqueId}".replace(".", "-").toLowerCase(), 62)
    omegaRdsSubnetGroupName = genericTools.trim("${params.stackName}-subnetgroup-${uniqueId}".replace(".", "-").toLowerCase(), 62)
    omegaRdsParameterGroupName = genericTools.trim("${params.stackName}-parametergroup-${uniqueId}".replace(".", "-").toLowerCase(), 62)
                    
    createNewRds = params.createNewRds
    if(params.dbName != null && params.dbName != ""){
        dbName = genericTools.trim(params.dbName, 62)
    } else {
        dbName = genericTools.trim("${params.stackName}database${uniqueId}".replace(".", "").replace("-", "").toLowerCase(), 62)
    }

    if (params.dbServerAddress != null && params.dbServerAddress.trim() != '') {
        dbServerAddress = params.dbServerAddress
    }

    if (params.dbServerPort != null && params.dbServerPort.trim() != '') {
        dbServerPort = params.dbServerPort
    }

    subnetAclId = params.rdsSubnetAclId
    subnetRouteTableId = params.rdsSubnetRouteId

    stackRegion = params.region

    cloudFrontSslArn = params.cloudFrontSslArn

    bat "aws configure set aws_access_key_id ${params.awsAccessKey}"
    bat "aws configure set aws_secret_access_key ${params.awsSecret}"
    bat "aws configure set default.region ${params.region}"
}

def stage_initialProjectSetup(){
    try {
        println "Creating project directory..."
        genericTools.mkdir(projectDirectory)
        def gitUrl = System.getenv("OMEGA_GIT_URL")
        if (gitUrl == null || gitUrl.trim() == "")
            error("OMEGA_GIT_URL is not set. Add it in Jenkins (see Jenkins/omega-pipeline.env.example) or copy from .env.example.")
        dir(projectDirectory) {
            bat "git clone --branch production ${gitUrl} ."
            bat "git checkout tags/release-${params.version}"
        }
        println "Project directory created!"
    } catch (Exception e) {
        println "Error while setting up project"
        rollbackRequired = true
        rollbackException = e
        println e.toString()
    }
}

def getRdsDetailsFromAws(){
    if(createNewRds){
        awsReturnedEndpoint = rdsDbInstance.Endpoint != null

        while (!awsReturnedEndpoint) {
            rdsDbInstance = awsTools.getRdsDbInstanceInfo(rdsDbInstance.DBInstanceIdentifier)
            awsReturnedEndpoint = rdsDbInstance.Endpoint != null
            if (!awsReturnedEndpoint) {
                sleep 10
            }
        }

        dbName = rdsDbInstance.DBName
        dbServerAddress = rdsDbInstance.Endpoint.Address
        dbServerPort = rdsDbInstance.Endpoint.Port
    }
}

def stage_createAwsInfrastructure(params){
    try {
        println "Creating S3 buckets..."
        if(createSingleS3LambdaBucket){
            awsTools.createS3Bucket(uniqueId, singleS3LambdaBucketName, stackRegion)
            awsTools.createS3Bucket(uniqueId, singleS3PublicBucketName, stackRegion)
            awsTools.makeS3BucketPublic(singleS3PublicBucketName, projectDirectory)
        } else {
            awsTools.createS3Bucket(uniqueId, omegaAdminS3BucketName, stackRegion)
            awsTools.createS3Bucket(uniqueId, omegaWsS3BucketName, stackRegion)
            awsTools.createS3Bucket(uniqueId, omegaWsSocketS3BucketName, stackRegion)
            awsTools.createS3Bucket(uniqueId, omegaAdminAssetsS3BucketName, stackRegion, true)
            awsTools.makeS3BucketPublic(omegaAdminAssetsS3BucketName, projectDirectory)
            awsTools.createS3Bucket(uniqueId, omegaUploadsS3BucketName, stackRegion, true)
            awsTools.makeS3BucketPublic(omegaUploadsS3BucketName, projectDirectory)
        }
        awsTools.createS3Bucket(uniqueId, omegaPluginsS3BucketName, stackRegion)
        println "S3 buckets created!"

        subnet1VpcId = awsTools.createSubnet("${params.stackName} (${uniqueId}) VPC Subnet", vpcId, params.subnet1VpcCidrBlock, params.subnet1VpcCidrBlockAvailabilityZone)
        if (!subnet1VpcId) {
            subnet1VpcId = awsTools.findSubnet(params.subnet1VpcCidrBlock)
        }
        subnet2VpcId = awsTools.createSubnet("${params.stackName} (${uniqueId}) VPC Subnet", vpcId, params.subnet2VpcCidrBlock, params.subnet2VpcCidrBlockAvailabilityZone)
        if (!subnet2VpcId) {
            subnet2VpcId = awsTools.findSubnet(params.subnet2VpcCidrBlock)
        }

        println "Creating VPC security groups..."
        vpcSecurityGroupId = awsTools.createSecurityGroup(vpcId, omegaVpcSecurityGroupName, "${params.stackName} (${uniqueId}) VPC Security Group")
        awsTools.addSecurityGroupIngress(vpcSecurityGroupId, "tcp", dbServerPort, "0.0.0.0/0")
        awsTools.addSecurityGroupIngress(vpcSecurityGroupId, "tcp", 80, "0.0.0.0/0")
        awsTools.addSecurityGroupIngress(vpcSecurityGroupId, "tcp", 443, "0.0.0.0/0")
        println "Security VPC groups created!"

        if (createNewRds) {
            println "Creating subnets and subnet group..."
            subnet1Id = awsTools.createSubnet("${params.stackName} (${uniqueId}) RDS Subnet", vpcId, params.rdsSubnet1CidrBlock, params.rdsSubnet1CidrBlockAvailabilityZone)
            if (!subnet1Id) {
                subnet1Id = awsTools.findSubnet(params.rdsSubnet1CidrBlock)
            }
            subnet2Id = awsTools.createSubnet("${params.stackName} (${uniqueId}) RDS Subnet", vpcId, params.rdsSubnet2CidrBlock, params.rdsSubnet2CidrBlockAvailabilityZone)
            if (!subnet2Id) {
                subnet2Id = awsTools.findSubnet(params.rdsSubnet2CidrBlock)
            }
            awsTools.createRdsSubnetGroup(omegaRdsSubnetGroupName, "${omegaRdsSubnetGroupName} Subnet Group", subnet1Id, subnet2Id)

            //awsTools.associateSubnetWithAcl(subnet1Id, subnetAclId)
            //awsTools.associateSubnetWithAcl(subnet2Id, subnetAclId)

            awsTools.associateSubnetWithRouteTable(subnet1Id, subnetRouteTableId)
            awsTools.associateSubnetWithRouteTable(subnet2Id, subnetRouteTableId)

            println "Subnets and subnet group created!"

            println "Creating RDS security groups..."
            rdsSecurityGroupId = awsTools.createSecurityGroup(vpcId, omegaRdsSecurityGroupName, "${params.stackName} (${uniqueId}) RDS Security Group")
            awsTools.addSecurityGroupIngress(rdsSecurityGroupId, "tcp", dbServerPort, "0.0.0.0/0")
            println "Security RDS groups created!"

            println "Creating RDS parameter group..."
            awsTools.createRdsParameterGroup(omegaRdsParameterGroupName, params.rdsDatabaseFamily, "${params.stackName} (${uniqueId}) Parameter Group")
            def rdsParamGroupParameters = []
            rdsParamGroupParameters = genericTools.appendObjectVariable(rdsParamGroupParameters, "ParameterName", "'log_bin_trust_function_creators'")
            rdsParamGroupParameters = genericTools.appendObjectVariable(rdsParamGroupParameters, "ParameterValue", "1")
            rdsParamGroupParameters = genericTools.appendObjectVariable(rdsParamGroupParameters, "ApplyMethod", "immediate")
            awsTools.modifyRdsParameterGroup(omegaRdsParameterGroupName, rdsParamGroupParameters)
            println "RDS parameter group created!"

            println "Creating RDS Db..."
            rdsDbInstance = awsTools.createRdsDbInstance(uniqueId, "518606024901", region, dbName, params.rdsDatabaseInstanceClass, params.rdsDatabaseEngine, dbUser, dbPassword, 20, rdsSecurityGroupId, params.rdsSubnet1CidrBlockAvailabilityZone, omegaRdsSubnetGroupName, omegaRdsParameterGroupName, dbServerPort)
            println "RDS Db created!"
        } else {
            println "Creating database on (${dbServerAddress})..."
            genericTools.executeSql(dbServerAddress, dbServerPort, setupDbUser, setupDbPassword, "", "CREATE DATABASE ${dbName};")
            genericTools.executeSql(dbServerAddress, dbServerPort, setupDbUser, setupDbPassword, "", "CREATE USER '${dbUser}'@'%' IDENTIFIED BY '${dbPassword}';")
            genericTools.executeSql(dbServerAddress, dbServerPort, setupDbUser, setupDbPassword, "", "GRANT ALL PRIVILEGES ON ${dbName}.* TO '${dbUser}'@'%';")
            genericTools.executeSql(dbServerAddress, dbServerPort, setupDbUser, setupDbPassword, "", "FLUSH PRIVILEGES;")
            println "Database created on (${dbServerAddress})..."
        }

        println "Creating database data..."
        genericTools.executeSql(dbServerAddress, dbServerPort, dbUser, dbPassword, dbName, "md_cms.sql", true)
        genericTools.executeSql(dbServerAddress, dbServerPort, dbUser, dbPassword, dbName, "md_cms_functions.sql", true)
        genericTools.executeSql(dbServerAddress, dbServerPort, dbUser, dbPassword, dbName, "md_cms_views.sql", true)
        genericTools.executeSql(dbServerAddress, dbServerPort, dbUser, dbPassword, dbName, "md_cms_procedures.sql", true)
        genericTools.executeSql(dbServerAddress, dbServerPort, dbUser, dbPassword, dbName, "md_cms_data.sql", true)
        println "Database data created!"

    } catch (Exception e) {
        println "Error occured during aws infrastructure creation!"
        rollbackRequired = true
        rollbackException = e
        println e.toString()
    }
}

def stage_projectSetup(){
    try {
        dir(projectDirectory) {
            cmsTools.buildDependencies(buildProfile, projectDirectory)

            cmsTools.buildAndPublishLibs(buildProfile, projectDirectory, publishDirectory, baseArtifactDirectory, nugetKey)
                        
            dir(projectDirectory){
                adminVersion = genericTools.getCommandOutput(".\\get-version.bat MD.CMS.Administration.Core.Hosted\\MD.CMS.Administration.Core.Hosted.csproj")
                webApiVersion = genericTools.getCommandOutput(".\\get-version.bat MD.CMS.WebApi.Core.Hosted\\MD.CMS.WebApi.Core.Hosted.csproj")
            }

            cmsTools.buildAdminHosted(buildProfile, projectDirectory, publishDirectory, adminVersion)

            cmsTools.buildWebApiHosted(buildProfile, projectDirectory, publishDirectory, webApiVersion)

            dir("MD.CMS.Administration.Core.AwsLambda") {
                try {
                    //bat "create-links.bat"
                    //bat "build-project.bat ${buildProfile}"
                    //bat "build-scripts.${buildProfile}.bat"
                    genericTools.mkdir("\"wwwroot\\scripts\"", true)
                    bat "xcopy /E /S /I /Q /Y /F \"..\\MD.CMS.Administration\\MD.CMS.Administration.Core.Web\\scripts\" \"wwwroot\\scripts\""
                    genericTools.rmdir("\"wwwroot\\scripts\\.scannerwork\"", true)
                    //genericTools.robocopy("\"..\\MD.CMS.Administration\\MD.CMS.Administration.Core.Web\\scripts\" \"wwwroot\\scripts\" /MIR")
                } catch (Exception e) {
                    println "Error while creating AWS lambda scripts file"
                    println e.toString()
                }
            }
        } 
    } catch (Exception e) {
        println "Error occured during project setup!"
        rollbackRequired = true
        rollbackException = e
        println e.toString()
    }
}

def stage_deployS3(params){
    if (rollbackException == null) {
        try {
            dir(projectDirectory) {
                println "Deploying CMS plugins to S3 bucket..."
                try {
                    bat "aws s3 sync \"AwsLambdaPlugins\" s3://${omegaPluginsS3BucketName}"
                } catch (Exception e) {
                    println "Error AWS S3 Plugin Sync"
                    println e.toString()
                }
                println "CMS plugins deployed to S3 bucket..."
                                
                println "Deploying CMS static assets to S3 bucket..."
                try {
                    bat "aws s3 sync \"${baseArtifactDirectory}\\admin.awslambda\\assets.${params.version}\" s3://${omegaAdminAssetsS3BucketName} --quiet"
                } catch (Exception e) {
                    println "Error AWS S3 Admin Asset Sync"
                    println e.toString()
                }
                println "CMS static assets deployed to S3 bucket..."
            }
        } catch (Exception e) {
            println "Error occured during lambda s3 deployment!"
            rollbackRequired = true
            rollbackException = e
            println e.toString()
        }
    }
}

def deploy_containerLambda(stackName, s3BucketName, templateParameters, serverlessTemplateFileName, protocol){
    def apiGatewayUrl = ""

    println "Creating IAM Role..."
    try {
        awsTools.createIamRole(stackName, "..\\Assets\\aws\\rolepolicy.json")
        awsTools.assignPolicyToIamRole(stackName, "arn:aws:iam::aws:policy/AWSLambda_FullAccess")
        awsTools.assignPolicyToIamRole(stackName, "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole")
    } catch (Exception e) {
        println "Error occured while creating IAM Role!"
        println e.toString()
        println "Continuing..."
    }
    println "IAM Role created!"
                                    
    println "Creating Lambda Options..."
    def publish_opts = [
        "lambdaFunctionName": stackName,
        "region": stackRegion,
        "configuration": buildProfile,
        "s3bucketname": s3BucketName,
        "iamRole": stackName,
        "lambdaFunctionMemorySize": 512,
        "lambdaFunctionTimeout": 30,
        "templateParameters": templateParameters,
        "configFile": "aws-lambda-tools-defaults.json",
        "templateFile": serverlessTemplateFileName,
        "package": "\"${baseArtifactDirectory}\\container.awslambda\\container.awslambda.1.0.1\""
    ]
    println "Lambda Options created!"

    println "Creating Lambda Function and API Gateway..."
    apiGatewayUrl = cmsTools.publishDotnetLambda(publish_opts, protocol)
    println "Lambda Function and API Gateway created!"

    return apiGatewayUrl
}

def stage_deployLambdaAdmin(params){
    if (rollbackException == null) {
        try {
            dir(projectDirectory) {
                dir("MD.CMS.AwsLambda.Container.Core") {
                    println "Deploying CMS Admin lambda..."

                    println "Creating Environment Variables..."
                    def stageName = cloudFrontAdminPath.replace("/", "")
                    if(cloudFrontAdminPath == ""){
                        stageName = "Prod"
                    }
                    def productArn = genericTools.getCommandOutputLastLine("powershell -File \"..\\Jenkinsfile-AwsLambda-GetCmsLayers.ps1\" -framework dotnetcore3.1 -product \"Administration\" -version ${params.version}")
                    def templateParameters = []
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "StageName", stageName.replace("/", ""), true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "ProductLayer", "${productArn}:1", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "BasePluginsLayer", "arn:aws:lambda:us-east-1:518606024901:layer:AwsLambdaPlugins:1", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "AppReferencePath", "MD.CMS.Administration.Core.AwsLambda.dll.MD.CMS.Administration.Core.AwsLambda.AwsStartup", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "WebAppPath", "/opt/MD.CMS.Administration.Core.AwsLambda/", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "BillingGroup", uniqueId, true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "GatewayName", omegaAdminStackName, true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreDefaultLcid", cms_lcid, true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreGoogleMapsJsKey", "${cms_googleApiKey}", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDToolsHelpersCoreProviderOptions", "{ \"AWSS3FileProvider\": \"{ \\\"BucketName\\\": \\\"${omegaPluginsS3BucketName}\\\", \\\"AccessKey\\\": \\\"${params.awsAccessKey}\\\", \\\"SecretKey\\\": \\\"${params.awsSecret}\\\", \\\"RegionDisplayName\\\": \\\"${stackRegion}\\\"}\" }", true)
                    println "Environment Variables created!"

                    omegaAdminApiGatewayUrl = deploy_containerLambda(omegaAdminStackName, omegaAdminS3BucketName, templateParameters, "admin-serverless.template", "https")

                    println "CMS Admin lambda deployed..."
                }
            }
        } catch (Exception e) {
            println "Error occured during lambda admin instance deployment!"
            rollbackRequired = true
            rollbackException = e
            println e.toString()
        }
    }
}

def stage_deployLambdaWs(params){
    if (rollbackException == null) {
        try {
            dir(projectDirectory) {
                println "Deploying CMS ws lambda..."
                dir("MD.CMS.AwsLambda.Container.Core") {
                    getRdsDetailsFromAws()

                    awsTools.createIamRole(omegaWsStackName, "..\\Assets\\aws\\rolepolicy.json")
                    awsTools.assignPolicyToIamRole(omegaWsStackName, "arn:aws:iam::aws:policy/AWSLambda_FullAccess")
                    awsTools.assignPolicyToIamRole(omegaWsStackName, "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole")
                    
                    def productArn = genericTools.getCommandOutputLastLine("powershell -File \"..\\Jenkinsfile-AwsLambda-GetCmsLayers.ps1\" -framework dotnetcore3.1 -product \"WebApi\" -version ${params.version}")
                    def templateParameters = []
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "StageName", cloudFrontWsPath.replace("/", ""), true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "ProductLayer", "${productArn}:1", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "BasePluginsLayer", "arn:aws:lambda:us-east-1:518606024901:layer:AwsLambdaPlugins:1", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "AppReferencePath", "MD.CMS.WebApi.Core.AwsLambda.dll.MD.CMS.WebApi.Core.AwsLambda.AwsStartup", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "WebAppPath", "/opt/MD.CMS.WebApi.Core.AwsLambda/", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "BillingGroup", uniqueId, true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "VpcSecurityGroupId", vpcSecurityGroupId, true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "VpcSubnet1Id", subnet1VpcId, true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "VpcSubnet2Id", subnet2VpcId, true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "GatewayName", omegaWsStackName, true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreDefaultLcid", cms_lcid, true)
                    if(cms_emailEnabled){
                        templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreEmailHost", cms_emailHost, true)
                        templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreEmailPort", cms_emailPort, true)
                        templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreEmailEnableSsl", cms_emailUseSsl, true)
                        templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreUsername", cms_emailUsername, true)
                        templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCorePassword", cms_emailPassword, true)
                    }
                    if(cms_rootAccountEnabled){
                        templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreRootAdminAccount", "{ \"Username\": \"${cms_rootAccountUsername}\", \"Password\": \"${cms_rootAccountPassword}\" }", true)
                    }
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreSessionDomain", "${cloudFrontDistributionName.toLowerCase()}.client.omegacms.run", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreSessionTimeout", "01:00:00", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSWebApiCoreCorsOrigins", "[ \"${cloudFrontDistributionName.toLowerCase()}.client.omegacms.run\" ]", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSWebApiCorePluginsDirectory", "AwsLambdaPlugins", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSWebApiCorePluginsFileProviderType", "1", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDToolsBaseDataAccessPluginsCoreDataAccessPlugins", "[ \"MD.Tools.BaseDataAccess.Plugins.WebService.Core\", \"MD.Tools.BaseDataAccess.Plugins.MySql.Core\" ]", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDToolsBaseDataAccessPluginsCoreDataAccessPluginSettings", "{\"MD.Tools.BaseDataAccess.Plugins.MySql.Core\": \"DataSource=${dbServerAddress}%%3BDatabase=${dbName}%%3BUser=${dbUser}%%3BPassword=${dbPassword}%%3BCharSet=utf8%%3Bconvert zero datetime=True%%3BDefault Command Timeout=60\",\"MD.Tools.BaseDataAccess.Plugins.WebService.Core\": \"\"}", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDToolsBaseDataAccessPluginsCoreBaseDataAccessPluginsDirectory", "AwsLambdaPlugins", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDToolsBaseDataAccessPluginsCoreBaseDataAccessPluginsFileProviderType", "1", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDToolsHelpersCoreProviderOptions", "", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDToolsHelpersCoreDefaultFileProvider", "1", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSWebApiCoreBaseApiPath", "/", true)
                                 
                    omegaWsApiGatewayUrl = deploy_containerLambda(omegaWsStackName, omegaWsS3BucketName, templateParameters, "webapi-serverless.template", "https")
                }
                println "CMS ws lambda deployed..."
            }
        } catch (Exception e) {
            println "Error occured during lambda ws instance deployment!"
            rollbackRequired = true
            rollbackException = e
            println e.toString()
        }
    }
}

def stage_deployLambdaWsSocket(params){
    if (rollbackException == null) {
        try {
            dir(projectDirectory) {
                println "Deploying CMS ws sockets lambda..."
                dir("MD.CMS.AwsLambda.Container.Core") {
                    getRdsDetailsFromAws()    

                    awsTools.createIamRole(omegaWsSocketStackName, "..\\Assets\\aws\\rolepolicy.json")
                    awsTools.assignPolicyToIamRole(omegaWsSocketStackName, "arn:aws:iam::aws:policy/AWSLambda_FullAccess")
                    awsTools.assignPolicyToIamRole(omegaWsSocketStackName, "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole")

                    def productArn = genericTools.getCommandOutputLastLine("powershell -File \"..\\Jenkinsfile-AwsLambda-GetCmsLayers.ps1\" -framework dotnetcore3.1 -product \"WebApi-Sockets\" -version ${params.version}")
                    def templateParameters = []
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "StageName", cloudFrontWsSocketPath.replace("/", ""), true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "ProductLayer", "${productArn}:1", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "BasePluginsLayer", "arn:aws:lambda:us-east-1:518606024901:layer:AwsLambdaPlugins:1", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "AppReferencePath", "MD.CMS.WebApi.Sockets.Core.AwsLambda.dll.MD.CMS.WebApi.Sockets.Core.AwsLambda.Functions", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "WebAppPath", "/opt/MD.CMS.WebApi.Sockets.Core.AwsLambda/", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "BillingGroup", uniqueId, true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "VpcSecurityGroupId", vpcSecurityGroupId, true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "VpcSubnet1Id", subnet1VpcId, true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "VpcSubnet2Id", subnet2VpcId, true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "GatewayName", omegaWsStackName, true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreDefaultLcid", cms_lcid, true)
                    if(cms_emailEnabled){
                        templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreEmailHost", cms_emailHost, true)
                        templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreEmailPort", cms_emailPort, true)
                        templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreEmailEnableSsl", cms_emailUseSsl, true)
                        templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreUsername", cms_emailUsername, true)
                        templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCorePassword", cms_emailPassword, true)
                    }
                    if(cms_rootAccountEnabled){
                        templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreRootAdminAccount", "{ \"Username\": \"${cms_rootAccountUsername}\", \"Password\": \"${cms_rootAccountPassword}\" }", true)
                    }
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreSessionDomain", "${cloudFrontDistributionName.toLowerCase()}.client.omegacms.run", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSBusinessLogicCoreSessionTimeout", "01:00:00", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSWebApiCoreCorsOrigins", "[ \"${cloudFrontDistributionName.toLowerCase()}.client.omegacms.run\" ]", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSWebApiCorePluginsDirectory", "AwsLambdaPlugins", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSWebApiCorePluginsFileProviderType", "1", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDToolsBaseDataAccessPluginsCoreDataAccessPlugins", "[ \"MD.Tools.BaseDataAccess.Plugins.WebService.Core\", \"MD.Tools.BaseDataAccess.Plugins.MySql.Core\" ]", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDToolsBaseDataAccessPluginsCoreDataAccessPluginSettings", "{\"MD.Tools.BaseDataAccess.Plugins.MySql.Core\": \"DataSource=${dbServerAddress}%%3BDatabase=${dbName}%%3BUser=${dbUser}%%3BPassword=${dbPassword}%%3BCharSet=utf8%%3Bconvert zero datetime=True%%3BDefault Command Timeout=60\",\"MD.Tools.BaseDataAccess.Plugins.WebService.Core\": \"\"}", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDToolsBaseDataAccessPluginsCoreBaseDataAccessPluginsDirectory", "AwsLambdaPlugins", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDToolsBaseDataAccessPluginsCoreBaseDataAccessPluginsFileProviderType", "1", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDToolsHelpersCoreProviderOptions", "{ \"AWSS3FileProvider\": \"{ \\\"BucketName\\\": \\\"${omegaPluginsS3BucketName}\\\", \\\"AccessKey\\\": \\\"${params.awsAccessKey}\\\", \\\"SecretKey\\\": \\\"${params.awsSecret}\\\", \\\"RegionDisplayName\\\": \\\"${stackRegion}\\\"}\" }", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDToolsHelpersCoreDefaultFileProvider", "1", true)
                    templateParameters = genericTools.appendObjectVariable(templateParameters, "MDCMSWebApiCoreBaseApiPath", cloudFrontWsPath, true)
                                 
                    omegaWsSocketApiGatewayUrl = deploy_containerLambda(omegaWsSocketStackName, omegaWsSocketS3BucketName, templateParameters, "websockets-serverless.template", "wss")
                }
                println "CMS ws sockets lambda deployed..."
            }
        } catch (Exception e) {
            println "Error occured during lambda ws-socket instance deployment!"
            rollbackRequired = true
            rollbackException = e
            println e.toString()
        }
    }
}

pipeline {
    agent any
    parameters {
        choice(name: "buildProfile", choices: ["Release", "Debug"], description: "Build profile")
        string(name: "artifactDirectory", defaultValue: "\\\\10.0.10.3\\IIS_Shares\\CMS\\Artifacts", description: "Artifact Directory")
        password(name: "nugetKey", defaultValue: "", description: "Nuget Key")
        choice(name: "version", choices: getCmsVersions(), description: "Omega Version")
        string(name: "clientName", description: "Client Name", defaultValue: "Omega Test Client")
        string(name: "clientId", description: "Client Id", defaultValue: "0")
        password(name: "awsAccessKey", description: "AWS Access Key", defaultValue: "")
        password(name: "awsSecret", description: "AWS Secret", defaultValue: "")
        password(name: "vpcId", description: "AWS VPC Id", defaultValue: "vpc-00c8ef7a66ae66c01")
        string(name: "subnet1VpcCidrBlock", description: "AWS VPC Lambda Subnet 1 CIDR Block", defaultValue: "10.90.152.0/24")
        string(name: "subnet2VpcCidrBlock", description: "AWS VPC Lambda Subnet 2 CIDR Block", defaultValue: "10.90.153.0/24")
        string(name: "subnet1VpcCidrBlockAvailabilityZone", description: "AWS Lambda Omega VPC Subnet 1 CIDR Block Availability Zone", defaultValue: "us-east-1a")
        string(name: "subnet2VpcCidrBlockAvailabilityZone", description: "AWS Lambda Omega VPC Subnet 2 CIDR Block Availability Zone", defaultValue: "us-east-1b")
        password(name: "igwId", description: "AWS VPC IGW Id", defaultValue: "igw-083fd50e3f65388ca")
        string(name: "stackName", description: "AWS Lambda Omega Stack Name", defaultValue: "Omega-CMS")
        password(name: "googleApiKey", description: "Google Api Key", defaultValue: "")
        string(name: "lcid", description: "Default Omega LCID", defaultValue: "2057")
        booleanParam(name: "emailEnabled", description: "Omega Email Enabled", defaultValue: true)
        string(name: "emailHost", description: "Omega Email Host (or set OMEGA_SMTP_HOST — see .env.example)", defaultValue: "")
        string(name: "emailPort", description: "Omega Email Port", defaultValue: "587")
        booleanParam(name: "emailSslEnabled", description: "Omega Email SSL Enabled", defaultValue: true)
        password(name: "emailUsername", description: "Omega Email Username", defaultValue: "")
        password(name: "emailPassword", description: "Omega Email Password", defaultValue: "")
        booleanParam(name: "rootAccountEnabled", description: "Omega RootAccount Enabled", defaultValue: true)
        password(name: "rootAccountUsername", description: "Omega Email Username", defaultValue: "root")
        password(name: "rootAccountPassword", description: "Omega Email Password", defaultValue: "")
        string(name: "dbName", description: "CMS Database Name", defaultValue: "")
        password(name: "setupDbUser", description: "CMS Database User", defaultValue: "")
        password(name: "setupDbPassword", description: "CMS Database Password", defaultValue: "")
        string(name: "dbServerAddress", description: "Database Server Address", defaultValue: "")
        string(name: "dbServerPort", description: "Database Server Port", defaultValue: "3306")
        booleanParam(name: "createSingleS3LambdaBucket", description: "Create single S3 lambda deploy bucket", defaultValue: true)
        booleanParam(name: "createNewRds", description: "Create New Rds", defaultValue: true)
        string(name: "rdsSubnet1CidrBlock", description: "AWS Lambda Omega RDS Subnet 1 CIDR Block", defaultValue: "10.90.150.0/24")
        string(name: "rdsSubnet2CidrBlock", description: "AWS Lambda Omega RDS Subnet 1 CIDR Block", defaultValue: "10.90.151.0/24")
        string(name: "rdsSubnetRouteId", description: "AWS Lambda Omega RDS Route Table Id", defaultValue: "rtb-0cbb736237b76a3a6")
        string(name: "rdsSubnetAclId", description: "AWS Lambda Omega RDS Subnet ACL Id", defaultValue: "acl-0a3e874e5a67c787f")
        string(name: "rdsSubnet1CidrBlockAvailabilityZone", description: "AWS Lambda Omega RDS Subnet 1 CIDR Block Availability Zone", defaultValue: "us-east-1a")
        string(name: "rdsSubnet2CidrBlockAvailabilityZone", description: "AWS Lambda Omega RDS Subnet 1 CIDR Block Availability Zone", defaultValue: "us-east-1b")
        string(name: "region", description: "AWS region name", defaultValue: "us-east-1")
        string(name: "rdsDatabaseType", description: "AWS RDS Database Type", defaultValue: "MariaDB")
        string(name: "rdsDatabaseFamily", description: "AWS RDS Database Family", defaultValue: "mariadb10.4")
        string(name: "rdsDatabaseEngine", description: "AWS RDS Database Engine", defaultValue: "mariadb")
        string(name: "rdsDatabaseInstanceClass", description: "AWS RDS Database Instance Class", defaultValue: "db.t2.micro")
        string(name: "cloudFrontDistributionName", description: "CloudFront OmegaCMS Distribution Name", defaultValue: "")
        string(name: "cloudFrontAdminPath", description: "CloudFront OmegaCMS Admin Path", defaultValue: "")
        string(name: "cloudFrontWsPath", description: "CloudFront OmegaCMS Web Services Path", defaultValue: "/ws")
        string(name: "cloudFrontWsSocketPath", description: "CloudFront OmegaCMS Web Services Socket Path", defaultValue: "/ws-web-sockets")
        string(name: "cloudFrontSslArn", description: "CloudFront OmegaCMS Subdomain SslId", defaultValue: "arn:aws:acm:us-east-1:518606024901:certificate/847f077a-b3b5-4ab0-90ae-27c705f9812d")
        booleanParam(name: "forceRollback", description: "Force the rollback", defaultValue: false)
    }
    stages {
        stage("Load Modules"){
            steps {
                script{
                    try {
                        genericTools = load "Jenkins\\Tools\\GeneralTools.groovy"
                        dotNetTools = load "Jenkins\\Tools\\DotNetTools.groovy"
                        awsTools = load "Jenkins\\Tools\\AWSTools.groovy"
                        cmsTools = load "Jenkins\\Tools\\CMSTools.groovy"
                    } catch (Exception e){
                        error("Error happened during module load phase! The error is: ${e.toString()}")
                    }
                        
                    try {
                        dotNetTools.init()
                        awsTools.init()
                        cmsTools.init()
                    } catch (Exception e){
                        error("Error happened during module init phase! The error is: ${e.toString()}")
                    }
                }
            }
        }
        stage("Variable Setup"){
            steps{
                script {
                    stage_variableSetup(params)
                }
            }
        }
        stage("Initial Project Setup"){
            steps {
                script {
                    stage_initialProjectSetup()
                }
            }
        }
        stage("Creating AWS Infrastructure"){
            steps {
                script {
                    dir(projectDirectory) {
                        stage_createAwsInfrastructure(params)
                    }
                }
            }
        }
        stage("Deploying Lambda Instances") {
            steps {
                parallel(
                    "deploy-s3": {
                        script {
                            stage_deployS3(params)
                        }
                    },
                    "deploy-lambda-admin": {
                        script {
                            stage_deployLambdaAdmin(params)
                        }
                    },
                    "deploy-lambda-ws": {
                        script {
                            stage_deployLambdaWs(params)
                        }
                    },
                    "deploy-lambda-ws-socket": {
                        script {
                            stage_deployLambdaWsSocket(params)
                        }
                    }
                )
            }
        }
        stage("Deploying Lambda to CloudFront") {
            steps {
                script {
                    if (rollbackException == null) {
                        try {
                            dir(projectDirectory) {

                                def opts = [
                                    "distributionname": cloudFrontDistributionName,
                                    "adminlambdaurl": omegaAdminApiGatewayUrl,
                                    "wslambdaurl": omegaWsApiGatewayUrl,
                                    "wssocketlambdaurl": omegaWsSocketApiGatewayUrl,
                                    "s3adminassetsurl": "${omegaAdminAssetsS3BucketName}.s3.amazonaws.com", 
                                    "s3uploadsurl": "${omegaUploadsS3BucketName}.s3.amazonaws.com", 
                                    "adminpath": cloudFrontAdminPath,
                                    "wspath": cloudFrontWsPath,
                                    "wssocketpath": cloudFrontWsSocketPath,
                                    "distributionalias": "${cloudFrontDistributionName.toLowerCase()}.client.omegacms.run",
                                    "distributionsslarn": cloudFrontSslArn,
                                    "billingGroup": uniqueId
                                ]
                                
                                awsTools.createCloudFrontInstance(opts, projectDirectory)

                                cloudFrontDistributionUrl = awsTools.getCloudFrontDistributionUrl(cloudFrontDistributionName, projectDirectory)

                                if(cloudFrontDistributionUrl.trim() == ""){
                                    error("An error occured while creating the cloudfront distribution")
                                }

                                def postFields = []
                                postFields = genericTools.appendObjectVariable(postFields, "auth-id", genericTools.getEnvString("CLOUDNS_AUTH_ID", "5159"), false, true)
                                postFields = genericTools.appendObjectVariable(postFields, "auth-password", genericTools.getEnvString("CLOUDNS_AUTH_PASSWORD", ""), false, true)
                                postFields = genericTools.appendObjectVariable(postFields, "domain-name", "omegacms.run", false, true)
                                postFields = genericTools.appendObjectVariable(postFields, "record-type", "CNAME", false, true)
                                postFields = genericTools.appendObjectVariable(postFields, "host", "${cloudFrontDistributionName.toLowerCase()}.client", false, true)
                                postFields = genericTools.appendObjectVariable(postFields, "record", cloudFrontDistributionUrl, false, true)
                                postFields = genericTools.appendObjectVariable(postFields, "ttl", "3600", false, true)

                                httpRequest "https://api.cloudns.net/dns/add-record.json?${postFields.join('&')}"
                            }
                        } catch (Exception e) {
                            println "Error occured during cloudfront deployment!"
                            rollbackRequired = true
                            rollbackException = e
                            println e.toString()
                        }
                    }
                }
            }
        }
        stage("Commit to Registry"){
            steps {
                script {
                    if (rollbackException == null) {
                        try {
                            def cms_httpBase = genericTools.getEnvString("OMEGA_CMS_INTERNAL_WS_BASE", "")
                            if (cms_httpBase == null || cms_httpBase == "") {
                                println "Skipping Commit to Registry: set OMEGA_CMS_INTERNAL_WS_BASE"
                            } else {
                            def regPwd = params.rootAccountPassword
                            def cms_authToken = cmsTools.cms_login(cms_httpBase, "Admin", regPwd)

                            def content = cmsTools.cms_constructContent(cmsTools.cms_getContentType(cms_httpBase, cms_authToken, 4))
                            content.Title = "Lambda Deploy Content ${clientName}"
                            content.FolderId = 12
                            def websiteUrl = "${cloudFrontDistributionName.toLowerCase()}.client.omegacms.run".inspect()
                            cmsTools.cms_contentSetFieldValue(content, "Omega Customer", "${clientId}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Unique Id", "${uniqueId}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Stack Name", "${stackName}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Database Host Address", "${dbServerAddress}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Database Host Port", "${dbServerPort}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Database Name", "${dbName}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Database User", "${dbUser}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Database Password", "${dbPassword}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Admin Lambda Gateway Api Url", "${omegaAdminApiGatewayUrl}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Web Service Lambda Gateway Api Url", "${omegaWsApiGatewayUrl}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Web Service Socket Lambda Gateway Api Url", "${omegaWsSocketApiGatewayUrl}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Plugins S3 Bucket", "${omegaPluginsS3BucketName}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Public S3 Bucket", "${singleS3PublicBucketName}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "CloudFront Distribution Name", "${cloudFrontDistributionName}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "CloudFront Distribution Url", "${cloudFrontDistributionUrl}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Omega web url", "${websiteUrl}".inspect())
                            cmsTools.cms_saveContent(cms_httpBase, cms_authToken, content)
                            }
                        } catch (Exception e) {
                            println "Error occured commiting to CMS registry!"
                            rollbackRequired = true
                            rollbackException = e
                            println e.toString()
                        }
                    }
                }
            }
        }
        stage("Send mail to aws creation"){
            steps {
                script {
                    if (rollbackException == null) {
                        try {
                            emailext(
                            to: '$DEFAULT_RECIPIENTS', 
                            body: """
                                    A website has been created at ${cloudFrontDistributionName.toLowerCase()}.client.omegacms.run.\n
                                    The username is Admin\n
                                    The password was set from the job parameters (root account — not included in this email).\n
                                    \n
                                    Omega Jenkins
                                  """,
                            subject: "Jenkins AWS Deployment Success - ${cloudFrontDistributionName.toLowerCase()}.client.omegacms.run")
                        } catch (Exception e) {
                            println "Error occured while sending email!"
                        }
                    }
                }
            }
        }
        stage("Cleanup"){
            steps{
                script {
                    if(!rollbackRequired){
                        println "Deleting project directory..."
                        bat "if exist \"${projectDirectory}\" (rmdir /Q/S \"${projectDirectory}\")"
                        bat "if exist \"${projectDirectory}@tmp\" (rmdir /Q/S \"${projectDirectory}@tmp\")"
                        bat "if exist \"${projectDirectory}-dist\" (rmdir /Q/S \"${projectDirectory}-dist\")"
                        bat "if exist \"${projectDirectory}-dist@tmp\" (rmdir /Q/S \"${projectDirectory}-dist@tmp\")"
                        println "Project directory deleted!"
                    } else {
                        println "Error occured, skipping cleanup process!"
                    }
                }
            }
        }
        stage("Rollback"){
            steps{
                script {
                    if (rollbackRequired || forceRollback) {
                        println "Deleting stacks..."
                        awsTools.deleteStack(omegaAdminStackName)
                        awsTools.deleteStack(omegaWsStackName)
                        awsTools.deleteStack(omegaWsSocketStackName)
                        println "Stacks deleted!"

                        println "Deleting S3 buckets..."
                        if(createSingleS3LambdaBucket){
                            awsTools.deleteS3Bucket(singleS3LambdaBucketName)
                            awsTools.deleteS3Bucket(singleS3PublicBucketName)
                        } else {
                            awsTools.deleteS3Bucket(omegaAdminS3BucketName)
                            awsTools.deleteS3Bucket(omegaWsS3BucketName)
                            awsTools.deleteS3Bucket(omegaWsSocketS3BucketName)
                            awsTools.deleteS3Bucket(omegaAdminAssetsS3BucketName)
                            awsTools.deleteS3Bucket(omegaUploadsS3BucketName)
                        }
                        awsTools.deleteS3Bucket(omegaPluginsS3BucketName)
                        println "S3 buckets deleted!"

                        println "Deleting RDS instance..."
                        if(rdsDbInstance != null){
                            awsTools.deleteRdsDbInstance(rdsDbInstance.DBInstanceIdentifier)
                        }
                        println "RDS instance deleted!"

                        println "Deleting subnets and subnet group..."
                        awsTools.deleteRdsSubnetGroup(omegaRdsSubnetGroupName)
                        awsTools.deleteSubnet(subnet1Id)
                        awsTools.deleteSubnet(subnet2Id)
                        println "Subnets and subnet group deleted!"

                        println "Deleting RDS parameter group..."
                        awsTools.deleteRdsParameterGroup(omegaRdsParameterGroupName)
                        println "RDS parameter group deleted!"

                        println "Deleting security group..."
                        awsTools.deleteSecurityGroup(rdsSecurityGroupId)
                        println "Security group deleted!"

                        println "Deleting IAM roles..."
                        awsTools.deleteIamRole(omegaAdminStackName)
                        awsTools.deleteIamRole(omegaWsStackName)
                        awsTools.deleteIamRole(omegaWsSocketStackName)
                        println "IAM roles deleted!"

                        if (rollbackException != null) {
                            error(rollbackException.toString())
                        }
                    }
                }
            }
        }
    }
}