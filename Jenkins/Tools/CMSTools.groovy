import java.time.*
import java.time.format.DateTimeFormatter
import groovy.json.*

def genericTools = null
def dotNetTools = null

def init(){
    genericTools = load "Jenkins\\Tools\\GeneralTools.groovy"
    dotNetTools = load "Jenkins\\Tools\\DotNetTools.groovy"
    dotNetTools.init()
}

def buildDependencies(buildProfile, baseProjectDirectory){
    dir(baseProjectDirectory){
        dir("MD.Tools.Helpers.Core"){
            bat "build-project.bat ${buildProfile}"
        }
        dir("MD.Tools.Licensing"){
            bat "build-project.bat ${buildProfile}"
        }
        dir("MD.Tools.BaseDataAccess.Core"){
            bat "build-project.bat ${buildProfile}"
        }
        dir("MD.Tools.BaseDataAccess.Plugins.Core"){
            bat "build-project.bat ${buildProfile}"
        }
        dir("MD.Tools.BaseDataAccess.PluginMethods.Core"){
            bat "build-project.bat ${buildProfile}"
        }
        dir("MD.CMS.BusinessLogic.Core"){
            bat "build-project.bat ${buildProfile}"
        }
        dir("MD.CMS.BusinessLogic.WebApi.Core"){
            bat "build-project.bat ${buildProfile}"
        }
        dir("MD.CMS.BusinessLogic.Administration.Core"){
            bat "build-project.bat ${buildProfile}"
        }
    }
}

def buildAndPublishLibs(buildProfile, baseProjectDirectory, basePublishDirectory, baseArtifactDirectory, nugetKey){
    dotNetTools.buildAndPublishLib(baseProjectDirectory, basePublishDirectory, baseArtifactDirectory, "MD.Tools.Helpers.Core", "md-tools-helpers-core", buildProfile, nugetKey)
    dotNetTools.buildAndPublishLib(baseProjectDirectory, basePublishDirectory, baseArtifactDirectory, "MD.Tools.Licensing", "md-tools-licensing", buildProfile, nugetKey)
    dotNetTools.buildAndPublishLib(baseProjectDirectory, basePublishDirectory, baseArtifactDirectory, "MD.Tools.BaseDataAccess.Core", "md-tools-basedataaccess-core", buildProfile, nugetKey)
    dotNetTools.buildAndPublishLib(baseProjectDirectory, basePublishDirectory, baseArtifactDirectory, "MD.Tools.BaseDataAccess.Plugins.Core", "md-tools-basedataaccess-plugins-core", buildProfile, nugetKey)
    dotNetTools.buildAndPublishLib(baseProjectDirectory, basePublishDirectory, baseArtifactDirectory, "MD.Tools.BaseDataAccess.PluginMethods.Core", "md-tools-basedataaccess-pluginmethods-core", buildProfile, nugetKey)
    dotNetTools.buildAndPublishLib(baseProjectDirectory, basePublishDirectory, baseArtifactDirectory, "MD.CMS.BusinessLogic.Core", "md-cms-businesslogic-core", buildProfile, nugetKey)
    dotNetTools.buildAndPublishLib(baseProjectDirectory, basePublishDirectory, baseArtifactDirectory, "MD.CMS.BusinessLogic.WebApi.Core", "md-cms-businesslogic-webapi-core", buildProfile, nugetKey)
    dotNetTools.buildAndPublishLib(baseProjectDirectory, basePublishDirectory, baseArtifactDirectory, "MD.CMS.BusinessLogic.Administration.Core", "md-cms-businesslogic-administration-core", buildProfile, nugetKey)
}


def buildAdminHosted(buildProfile, baseProjectDirectory, basePublishDirectory, adminVersion){
    def nodeCopyDirectory = "wwwroot\\"
    dir(baseProjectDirectory) {
        dir("MD.CMS.Administration"){
            bat "yarn install"
        }
        dir("MD.CMS.BusinessLogic.Administration.Core"){
            bat "build-project.bat ${buildProfile}"
        }
        dir("MD.CMS.Administration.Core.Hosted"){
            bat "create-links.bat"
            bat "build-project.bat ${buildProfile}"
        }

        if(buildProfile == "Debug"){
            nodeCopyDirectory = ""
        }
        dir("MD.CMS.Administration\\MD.CMS.Administration.Core"){
            bat "build-project.bat ${buildProfile}"
        }
        dir("MD.CMS.Administration"){
            bat "yarn run build-jsdoc"
            bat "yarn run build-typedoc"
        }
        dir("MD.CMS.Administration\\MD.CMS.Administration.Core.Web\\scripts\\businessLogic"){
            bat "tsc"
        }
        dir("MD.CMS.Administration.Core.Hosted"){
            bat "build-scripts.${buildProfile}.bat"
        }
    }
    dotNetTools.publish(baseProjectDirectory, basePublishDirectory, false, "MD.CMS.Administration.Core.Hosted", "administration.hosted", adminVersion, buildProfile, "win-x64", ".\\node_modules\\ ${basePublishDirectory}\\administration.hosted\\source\\${buildProfile.toLowerCase()}\\win-x64\\${nodeCopyDirectory}node_modules /S /E /Z /ZB /R:5 /W:5 /TBD /NP /V /MT:32")
    dotNetTools.publish(baseProjectDirectory, basePublishDirectory, false, "MD.CMS.Administration.Core.Hosted", "administration.hosted", adminVersion, buildProfile, "linux-x64", ".\node_modules\\ ${basePublishDirectory}\\administration.hosted\\source\\${buildProfile.toLowerCase()}\\linux-x64\\${nodeCopyDirectory}node_modules /S /E /Z /ZB /R:5 /W:5 /TBD /NP /V /MT:32")
    dotNetTools.publish(baseProjectDirectory, basePublishDirectory, false, "MD.CMS.Administration.Core.Hosted", "administration.hosted", adminVersion, buildProfile, "osx-x64", ".\\node_modules\\ ${basePublishDirectory}\\administration.hosted\\source\\${buildProfile.toLowerCase()}\\osx-x64\\${nodeCopyDirectory}node_modules /S /E /Z /ZB /R:5 /W:5 /TBD /NP /V /MT:32")
}

def getAdminAwsLambdaPackageName(adminVersion){
    return "admin.awslambda.${adminVersion}"
}

def getWsAwsLambdaPackageName(wsVersion){
    return "ws.awslambda.${wsVersion}"
}

def getWsSocketAwsLambdaPackageName(wsSocketVersion){
    return "wssocket.awslambda.${wsSocketVersion}"
}

def getContainerAwsLambdaPackageName(wsSocketVersion){
    return "container.awslambda.${wsSocketVersion}"
}

def buildAwsLambdaPackage(buildProfile, packageName, baseProjectDirectory, postAddCommands = []){
    def package_opts = [
        "lambdaFunctionName": packageName,
        "region": "us-east-1",
        "configuration": buildProfile,
        "configFile": "aws-lambda-tools-defaults.json",
        "projectDirectory": baseProjectDirectory,
        "packageFileName": packageName
    ]
    return packageDotnetLambda(package_opts, postAddCommands)
}

def buildWebApiHosted(buildProfile, baseProjectDirectory, basePublishDirectory, webApiVersion){
    dir(baseProjectDirectory){
        script {
            dir("MD.CMS.BusinessLogic.WebApi.Core"){
                bat "build-project.bat ${buildProfile}"
            }
            dir("MD.CMS.WebApi.Core.Hosted"){
                bat "build-project.bat ${buildProfile}"
            }
            dotNetTools.publishAll(baseProjectDirectory, basePublishDirectory, false, "MD.CMS.WebApi.Core.Hosted", "webapi.hosted", webApiVersion, buildProfile, ["win-x64", "linux-x64", "osx-x64"])
        }
    }
}

def buildAsyncTaskProcessor(buildProfile, baseProjectDirectory, basePublishDirectory, asyncTaskProcessorVersion){
    dir(baseProjectDirectory){
        script {
            dir("MD.Tools.AsyncTask.Processor"){
                bat "build-project.bat ${buildProfile}"
            }
            dotNetTools.publishAll(baseProjectDirectory, basePublishDirectory, false, "MD.Tools.AsyncTask.Processor", "asyncTaskProcessor", asyncTaskProcessorVersion, buildProfile, ["win-x64", "linux-x64", "osx-x64"])
        }
    }
}

def buildInstallerHosted(buildProfile, baseProjectDirectory, basePublishDirectory, installerVersion){
    dir(baseProjectDirectory){
        script {
            dir("MD.CMS.Installer.Hosted.Core"){
                bat "build-project.bat ${buildProfile}"
            }
            dotNetTools.publishAll(baseProjectDirectory, basePublishDirectory, false, "MD.CMS.Installer.Hosted.Core", "installer.hosted", installerVersion, buildProfile, ["win-x64", "linux-x64", "osx-x64"])
        }
    }
}

def publishDotnetLambda(opt, type) {
    def command = "dotnet lambda deploy-serverless ${opt.lambdaFunctionName} --function-name ${opt.lambdaFunctionName}"
    if(opt.region != null && opt.region != ""){
        command = "${command} --region ${opt.region}"
    }
    if(opt.lambdaFunctionHandler != null && opt.lambdaFunctionHandler != ""){
        command = "${command} --function-handler ${opt.lambdaFunctionHandler}"
    }
    if(opt.configuration != null && opt.configuration != ""){
        command = "${command} --configuration ${opt.configuration}"
    }
    if(opt.s3bucketname != null && opt.s3bucketname != ""){
        command = "${command} --s3-bucket ${opt.s3bucketname}"
    }
    if(opt.iamRole != null && opt.iamRole != ""){
        command = "${command} --function-role ${opt.iamRole}"
    }
    if(opt.lambdaFunctionMemorySize != null && opt.lambdaFunctionMemorySize != ""){
        command = "${command} --function-memory-size ${opt.lambdaFunctionMemorySize}"
    }
    if(opt.lambdaFunctionTimeout != null && opt.lambdaFunctionTimeout != ""){
        command = "${command} --function-timeout ${opt.lambdaFunctionTimeout}"
    }
    if(opt.eventVariables != null){
        command = "${command} --ev '${opt.eventVariables.join(';')}'"
    }
    if(opt.configFile != null && opt.configFile != ""){
        command = "${command} --config-file ${opt.configFile}"
    }
    if(opt.templateFile != null && opt.templateFile != ""){
        command = "${command} -t ${opt.templateFile}"
    }
    if(opt.templateParameters != null){
        command = "${command} -tp ${opt.templateParameters.join(';')}"
    }
    if(opt.package != null && opt.package != ""){
        command = "${command} -pac \"${opt.package}\""
    }
    if(opt.vpc != null){
        command = "${command } --vpc-config SubnetIds=${opt.vpc.subnet1id},${opt.vpc.subnet2id},SecurityGroupIds=${opt.vpc.sgid}"
    }
    def res = genericTools.getCommandOutputLastLine(command, true)
    return res.split("${type}://")[1].split("/")[0]
}

def packageDotnetLambda(opt, postAddCommands = []) {
    try {
        genericTools.rmdir("${opt.projectDirectory}\\bin");
    } catch(e){
        println "Delete of bin folder failed."
    }
    try {
        genericTools.rmdir("${opt.projectDirectory}\\obj");
    } catch(e){
        println "Delete of obj folder failed."
    }

    def command = "dotnet lambda package ${opt.lambdaFunctionName} --function-name ${opt.lambdaFunctionName}"

    if(opt.region != null && opt.region != ""){
        command = "${command} --region ${opt.region}"
    }
    if(opt.configuration != null && opt.configuration != ""){
        command = "${command} --configuration ${opt.configuration}"
    }
    if(opt.configFile != null && opt.configFile != ""){
        command = "${command} --config-file ${opt.configFile}"
    }

    def res = genericTools.getCommandOutputLastLine(command, true)
    
    sleep 10

    def zipFilePath_original = res.replace("Lambda project successfully packaged:", "").replace("${opt.projectDirectory}\\", "").trim()
    def zipFilePath_new = "${zipFilePath_original}.zip".inspect().trim()

    try {
        bat "del ${zipFilePath_new}"
    } catch(e){
        println "Delete of existing file failed."
    }

    if(postAddCommands.size() > 0){
        bat "ren ${zipFilePath_original} ${zipFilePath_new}"
        for (postAddCommand in postAddCommands) {
            bat "\"C:\\Program Files\\7-Zip\\7z.exe\" a \"${zipFilePath_new}\" ${postAddCommand}"
        }
        bat "ren ${zipFilePath_new} ${opt.packageFileName}"
    } else {
        bat "ren ${zipFilePath_original} ${opt.packageFileName}"
    }

    return opt.packageFileName
}

def packageDotnetLambdaLayer(opt, postAddCommands = []) {
    try {
        genericTools.rmdir("${opt.projectDirectory}\\bin");
    } catch(e){
        println "Delete of bin folder failed."
    }
    try {
        genericTools.rmdir("${opt.projectDirectory}\\obj");
    } catch(e){
        println "Delete of obj folder failed."
    }

    genericTools.getCommandOutputLastLine("publish-project.bat ${opt.configuration} \"${opt.destination}\\${opt.projectShort}\"", true)

    dir(opt.destination){
        genericTools.getCommandOutputLastLine("\"C:\\Program Files\\7-Zip\\7z.exe\" a \"${opt.artifactsDirectory}\\${opt.projectShort}-v${opt.version}.zip\" \"${opt.projectShort}\"", true)
    }

    return "${opt.projectShort}-v${opt.version}"
}

def cms_postToCms(cms_httpBase, cms_path, cms_authToken, data, parseResponse = true) {
    def json = JsonOutput.toJson(data)
    println cms_httpBase
    println cms_path
    println data
    println json
    def headers = []
    if(cms_authToken != ""){
        headers = [[name: "authorization", value: cms_authToken]]
    }
    def response = httpRequest contentType: 'APPLICATION_JSON', customHeaders: headers, httpMode: 'POST', requestBody: json, url: "${cms_httpBase}/${cms_path}", validResponseCodes: '200'
    if(parseResponse){
        def responseObj = readJSON text: response.content
        return responseObj
    }
    return true
}

def cms_login(cms_httpBase, username, password) {
    def data = [
        "Values": [
            "username":username,
            "password":password,
            "token":genericTools.getUniqueId(false)
        ],
        "AuthenticationProviderName":"BuiltInAuthenticationProvider"
    ]
    def responseObj = cms_postToCms(cms_httpBase, "User/LoginAuthData", "", data)
    def authHeader = "${username}:${responseObj.SessionId}"
    return authHeader.bytes.encodeBase64().toString()
}

def cms_getContentType(cms_httpBase, cms_authToken, id) {
    def response = httpRequest contentType: 'APPLICATION_JSON', customHeaders: [[name: "authorization", value: cms_authToken]], httpMode: 'GET', url: "${cms_httpBase}/ContentTypeDefinition/GetById/${id}", validResponseCodes: '200'
    def responseObj = readJSON text: response.content
    return responseObj
}

def cms_constructContent(contentType = null) {
    def data = [
        "Id":0,
        "IsDeleted":false,
        "LCID":0,
        "DateCreated":new Date().format( "yyyy-MM-dd'T'HH:mm:ss" ),
        "AuthorId":0,
        "FolderId":"",
        "Title":"",
        "Path":"",
        "Html":null,
        "Author":null,
        "ContentType":contentType,
        "ContentTypeDefinitionId": 0,
        "IsNew":true,
        "IsPublished":true,
        "Taxonomy":null,
        "Menu":null,
        "MetaDataFieldValues":null,
        "Template":null,
        "IsPublished":false,
        "ApprovalPending":false
    ]
    if(contentType != null){
        data.ContentTypeDefinitionId = contentType.Id
    }
    return data
}

def cms_contentSetFieldValue(content, fieldName, fieldValue) {
    for (field in content.ContentType.Fields) {
        if(field.Name == fieldName){
            field.Value = fieldValue
        }
    }
    return content
}

def cms_saveContent(cms_httpBase, cms_authToken, contentObj) {
    def fileName = "request-${genericTools.getUniqueId()}.json"
    writeJSON file: fileName, json: contentObj
    def json = readFile "${env.WORKSPACE}/${fileName}"
    def response = httpRequest contentType: 'APPLICATION_JSON', customHeaders: [[name: "authorization", value: cms_authToken]], httpMode: 'POST', requestBody: json, url: "${cms_httpBase}/Content/Save/", validResponseCodes: '200'
    def responseObj = readJSON text: response.content
    return responseObj
}


def cms_getLambdaLayerArn(product, version){
    return genericTools.getCommandOutput("powershell -file \"Jenkinsfile-AwsLambda-GetCmsLayers.ps1\" --framework dotnetcore3.1 --product ${product} --version ${version}", true)
}

return this