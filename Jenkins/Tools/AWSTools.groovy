import java.time.*
import java.time.format.DateTimeFormatter
import groovy.json.*

def genericTools = null

def init(){
    genericTools = load "Jenkins\\Tools\\GeneralTools.groovy"
}

def createIamRole(roleName, rolePolicyFilePath) {
    bat "aws iam create-role --role-name ${roleName} --assume-role-policy-document file://${rolePolicyFilePath}"
}

def deleteIamRole(roleName) {
    try {
        bat "aws iam delete-role --role-name ${roleName}"
    } catch (Exception e) {
        //Silent Fail
    }
}

def assignPolicyToIamRole(roleName, polictyArn) {
    bat "aws iam attach-role-policy --role-name ${roleName} --policy-arn ${polictyArn}"
}

def createS3Bucket(uniqueId, bucketName, region, isPublic = false) {
    def execString = "aws s3api create-bucket --bucket ${bucketName} --region ${region}"
    if (isPublic) {
        execString = "${execString} --acl public-read"
    }
    bat execString
    bat "aws s3api put-bucket-tagging --bucket ${bucketName} --tagging TagSet=[{Key=BillingGroup,Value=${uniqueId}}]"
}


def deleteS3Bucket(bucketName) {
    try {
        bat "aws s3api delete-bucket --bucket ${bucketName} --force"
    } catch (Exception e) {
        //Silent Fail
    }
}

def makeS3BucketPublic(bucketName, projectDirectory) {
    bat "powershell -File \"${projectDirectory}\\Jenkinsfile-AwsLambda-MakeS3Public.ps1\" --s3bucketname ${bucketName}"
}

def getLambdaGatewayApiId(lambdaFunctionName, projectDirectory) {
    def apiIdReturned = false
    def apiId = null
    while (!apiIdReturned) {
        try {
            apiId = genericTools.getCommandOutput("powershell -File \"${projectDirectory}\\Jenkinsfile-AwsLambda-GetApiGateway.ps1\" ${lambdaFunctionName}")
        } catch (Exception e) {
            apiId = null
        }

        apiIdReturned = apiId != null
        if (!apiIdReturned) {
            sleep 10
        }
    }
    return apiId
}

def getLambdaGatewayApiUrl(lambdaFunctionName, region, projectDirectory) {
    def apiId = getLambdaGatewayApiId(lambdaFunctionName, projectDirectory)
    return "${apiId}.execute-api.${region}.amazonaws.com"
}

def createSecurityGroup(vpcid, name, desription) {
    def response = genericTools.getCommandOutput("aws ec2 create-security-group --vpc-id ${vpcid} --group-name ${name} --description \"${desription}\"", true).join('').trim().replace("  ", "")
    def jsonResponse = readJSON text: response
    return jsonResponse.GroupId
}

def deleteSecurityGroup(securityGroupId) {
    try {
        bat "aws ec2 delete-security-group --group-id ${securityGroupId}"
    } catch (Exception e) {
        //Silent Fail
    }
}

def addSecurityGroupIngress(groupId, protocol, port, cidr) {
    bat "aws ec2 authorize-security-group-ingress --group-id ${groupId} --protocol ${protocol} --port ${port} --cidr ${cidr}"
}

def createSubnet(name, vpcId, cidrBlock, availabilityZone) {
    try {
        def response = genericTools.getCommandOutput("aws ec2 create-subnet --vpc-id ${vpcId} --cidr-block ${cidrBlock} --availability-zone ${availabilityZone} --tag-specifications ResourceType=subnet,Tags=[{Key=Name,Value=\"${name}\"}]", true).join('').trim().replace("  ", "")
        def jsonResponse = readJSON text: response
        return jsonResponse.Subnet.SubnetId
    } catch (Exception e) {
        return false
    }
}

def findSubnet(cidrBlock) {
    def response = genericTools.getCommandOutput("aws ec2 describe-subnets --filters Name=cidr-block,Values=\"${cidrBlock}\" --output json", true).join('').trim().replace("  ", "")
    def jsonResponse = readJSON text: response
    return jsonResponse.Subnets[0].SubnetId
}

def deleteSubnet(subnetId) {
    try {
        bat "aws ec2 delete-subnet --subnet-id ${subnetId}"
    } catch (Exception e) {
        //Silent Fail
    }
}

def createRdsSubnetGroup(name, description, subnet1Id, subnet2Id) {
    bat "aws rds create-db-subnet-group --db-subnet-group-name ${name} --db-subnet-group-description \"${description}\" --subnet-ids \"${subnet1Id}\" \"${subnet2Id}\""
}

def deleteRdsSubnetGroup(name) {
    try {
        bat "aws rds delete-db-subnet-group --db-subnet-group-name ${name}"
    } catch (Exception e) {
        //Silent Fail
    }
}

def publishLambdaLayer(name, description, s3bucket, filename, runtimes = "dotnetcore3.1") {
    name = name.replace(".", "-");
    bat "aws lambda publish-layer-version --layer-name \"${name}\" --description \"${description}\" --content S3Bucket=${s3bucket},S3Key=${filename} --compatible-runtimes ${runtimes}"
}


def uploadFileToS3(filename, s3bucket, options = "") {
    bat "aws s3 cp \"${filename}\" s3://${s3bucket} ${options}"
}

def createRdsParameterGroup(name, family, description) {
    bat "aws rds create-db-parameter-group --db-parameter-group-name ${name} --db-parameter-group-family ${family} --description \"${description}\""
}

def modifyRdsParameterGroup(name, parameters) {
    bat "aws rds modify-db-parameter-group --db-parameter-group-name ${name} --parameters ${parameters.join(',')}"
}

def deleteRdsParameterGroup(name) {
    bat "aws rds delete-db-parameter-group --db-parameter-group-name ${name}"
}

def createRdsDbInstance(uniqueId, accountId, region, name, dbClass, engine, username, password, allocatedStorage, securityGroupIds, availabilityZone, subnetGroupName, parameterGroupName, port) {
    def command = "aws rds create-db-instance --db-name ${name} --db-instance-identifier ${name.toLowerCase()} --db-instance-class ${dbClass} --engine ${engine} --allocated-storage ${allocatedStorage} --vpc-security-group-ids \"${securityGroupIds}\" --availability-zone ${availabilityZone} --db-subnet-group-name ${subnetGroupName} --db-parameter-group-name ${parameterGroupName} --port ${port} --publicly-accessible"
    if (password != "") {
        command = "${command} --master-username \"${username}\""
    }
    if (username != "") {
        command = "${command} --master-user-password \"${password}\""
    }
    def response = genericTools.getCommandOutput(command, true).join('').trim().replace("  ", "")
    def jsonResponse = readJSON text: response
    bat "aws rds add-tags-to-resource --resource-name arn:aws:rds:${region}:${accountId}:db:${jsonResponse.DBInstance.DBInstanceIdentifier} --tags \"[{\\\"Key\\\": \\\"BillingGroup\\\",\\\"Value\\\": \\\"${uniqueId}\\\"}]\""
    return jsonResponse.DBInstance
}

def deleteRdsDbInstance(dbInstanceId) {
    try {
        bat "aws rds delete-db-instance --db-instance-identifier ${dbInstanceId} --skip-final-snapshot --delete-automated-backups"
    } catch (Exception e) {
        //Silent Fail
    }

    def rdsInstanceDeleted = false
    def rdsInstance = null
    while (!rdsInstanceDeleted) {
        rdsInstance = getRdsDbInstanceInfo(dbInstanceId)
        rdsInstanceDeleted = rdsInstance == null
        if (!rdsInstanceDeleted) {
            sleep 10
        }
    }
}

def getRdsDbInstanceInfo(instanceId) {
    try {
        def response = genericTools.getCommandOutput("aws rds describe-db-instances --db-instance-identifier ${instanceId}", true).join('').trim().replace("  ", "")
        def jsonResponse = readJSON text: response
        return jsonResponse.DBInstances[0]
    } catch (Exception e) {
        return null
    }
}

def deleteLambdaInstance(name) {
    try {
        bat "aws lambda delete-function --function-name ${name}"
    } catch (Exception e) {
        //Silent Fail
    }
}

def deleteStack(name) {
    try {
        bat "aws cloudformation delete-stack --stack-name ${name}"
    } catch (Exception e) {
        //Silent Fail
    }
}

def createCloudFrontInstance(opt, projectDirectory) {
    def command = "powershell -File \"${projectDirectory}\\Jenkinsfile-AwsLambda-CreateCloudFrontDistribution.ps1\""

    if(opt.billingGroup != null && opt.billingGroup != ""){
        command = "${command} -billingGroup ${opt.billingGroup}"
    } else {
        error("Parameter billingGroup required!")
    }

    if(opt.distributionname != null && opt.distributionname != ""){
        command = "${command} -distributionname ${opt.distributionname}"
    } else {
        error("Parameter distributionname required!")
    }

    if(opt.adminlambdaurl != null && opt.adminlambdaurl != ""){
        command = "${command} -adminlambdaurl ${opt.adminlambdaurl}"
    } else {
        error("Parameter adminlambdaurl required!")
    }
    
    if(opt.wslambdaurl != null && opt.wslambdaurl != ""){
        command = "${command} -wslambdaurl ${opt.wslambdaurl}"
    } else {
        error("Parameter wslambdaurl required!")
    }
    
    if(opt.wssocketlambdaurl != null && opt.wssocketlambdaurl != ""){
        command = "${command} -wssocketlambdaurl ${opt.wssocketlambdaurl}"
    } else {
        error("Parameter wssocketlambdaurl required!")
    }
    
    if(opt.s3adminassetsurl != null && opt.s3adminassetsurl != ""){
        command = "${command} -s3adminassetsurl ${opt.s3adminassetsurl}"
    } else {
        error("Parameter s3adminassetsurl required!")
    }
    
    if(opt.s3uploadsurl != null && opt.s3uploadsurl != ""){
        command = "${command} -s3uploadsurl ${opt.s3uploadsurl}"
    } else {
        error("Parameter s3uploadsurl required!")
    }
    
    if(opt.adminpath != null && opt.adminpath != ""){
        command = "${command} -adminpath ${opt.adminpath}"
    }
    
    if(opt.wspath != null && opt.wspath != ""){
        command = "${command} -wspath ${opt.wspath}"
    } else {
        error("Parameter wspath required!")
    }
    
    if(opt.wssocketpath != null && opt.wssocketpath != ""){
        command = "${command} -wssocketpath ${opt.wssocketpath}"
    } else {
        error("Parameter wssocketpath required!")
    }
    
    if(opt.adminoriginpath != null && opt.adminoriginpath != ""){
        command = "${command} -adminoriginpath ${opt.adminoriginpath}"
    }

    if(opt.wsoriginpath != null && opt.wsoriginpath != ""){
        command = "${command} -wsoriginpath ${opt.wsoriginpath}"
    }

    if(opt.wssocketoriginpath != null && opt.wssocketoriginpath != ""){
        command = "${command} -wssocketoriginpath ${opt.wssocketoriginpath}"
    }

    if(opt.distributionalias != null && opt.distributionalias != ""){
        command = "${command} -distributionalias ${opt.distributionalias}"
    }

    if(opt.distributionsslarn != null && opt.distributionsslarn != ""){
        command = "${command} -distributionsslarn ${opt.distributionsslarn}"
    }

    bat command
}

def getCloudFrontDistributionUrl(cloudFrontName, projectDirectory) {
    return genericTools.getCommandOutput("powershell -File \"${projectDirectory}\\Jenkinsfile-AwsLambda-GetCloudFrontDistributionUrl.ps1\" ${cloudFrontName}")
}

def associateSubnetWithRouteTable(subnetId, routeTableId) {
    bat "aws ec2 associate-route-table --route-table-id ${routeTableId} --subnet-id ${subnetId}"
}

def associateSubnetWithAcl(associationId, networkAclId) {
    bat "aws ec2 replace-network-acl-association --association-id ${associationId} --network-acl-id ${networkAclId}"
}

return this