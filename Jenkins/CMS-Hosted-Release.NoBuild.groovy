import groovy.json.*
import groovy.xml.*
import groovy.util.Node

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
projectDirectory = '\\\\stagingserver2019standard\\e$\\Sites\\Demo'
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
cms_uploadsRoot = ""
clientName = ""
clientId = 0
customDomain = ""

rollbackRequired = false
rollbackException = null

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
    
    adminVersion = params.version
    webApiVersion = params.version

    buildProfile = params.buildProfile

    clientName = params.clientName
    clientId = params.clientId

    forceRollback = params.forceRollback
    
    if (params.uniqueId == null || params.uniqueId == "") {
        uniqueId = genericTools.getUniqueId()
    } else {
        uniqueId = params.uniqueId
    }
    
    stackName = "${params.stackName}-${uniqueId}"
    dbUser = genericTools.trim("${params.stackName}${genericTools.getUniqueId()}".replace(".", "").replace("-", "").toLowerCase(), 15)
    dbPassword = genericTools.getUniqueId()

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
}

@NonCPS
def writeToFile(filePath, arguments) {
    def configXml = new XmlSlurper().parse("${filePath}")
    configXml.configuration.'system.webServer'.aspNetCore.'@processPath' = "dotnet"
    configXml.configuration.'system.webServer'.aspNetCore.'@arguments' = "${arguments}".inspect()
    configXml.configuration.'system.webServer'.aspNetCore.environmentVariables.replaceNode { }
    def configOutWriter = new StringWriter()
    XmlUtil.serialize( configXml, configOutWriter )
    return configOutWriter.toString()
}

def stage_siteDeployment(params) {
    try {
        println "Creating database on (${dbServerAddress})..."
        genericTools.executeSql(dbServerAddress, dbServerPort, setupDbUser, setupDbPassword, "", "CREATE DATABASE ${dbName};")
        genericTools.executeSql(dbServerAddress, dbServerPort, setupDbUser, setupDbPassword, "", "CREATE USER '${dbUser}'@'%' IDENTIFIED BY '${dbPassword}';")
        genericTools.executeSql(dbServerAddress, dbServerPort, setupDbUser, setupDbPassword, "", "GRANT ALL PRIVILEGES ON ${dbName}.* TO '${dbUser}'@'%';")
        genericTools.executeSql(dbServerAddress, dbServerPort, setupDbUser, setupDbPassword, "", "FLUSH PRIVILEGES;")
        println "Database created on (${dbServerAddress})..."

        println "Creating database data..."
        genericTools.executeSql(dbServerAddress, dbServerPort, dbUser, dbPassword, dbName, "md_cms.sql", true)
        genericTools.executeSql(dbServerAddress, dbServerPort, dbUser, dbPassword, dbName, "md_cms_functions.sql", true)
        genericTools.executeSql(dbServerAddress, dbServerPort, dbUser, dbPassword, dbName, "md_cms_views.sql", true)
        genericTools.executeSql(dbServerAddress, dbServerPort, dbUser, dbPassword, dbName, "md_cms_procedures.sql", true)
        genericTools.executeSql(dbServerAddress, dbServerPort, dbUser, dbPassword, dbName, "md_cms_data.sql", true)
        println "Database data created!"

        cms_uploadsRoot = "\\\\10.0.10.3\\IIS_Shares\\demo.omegacms.run\\${stackName}"
        genericTools.mkdir(cms_uploadsRoot, true);
        cms_uploadsRoot = "${cms_uploadsRoot}\\uploads"
        genericTools.mkdir(cms_uploadsRoot, true);
        
        def cms_pluginsPath = "${projectDirectory}\\${stackName}\\plugins"
        def cms_adminPath = "${projectDirectory}\\${stackName}\\admin"
        def cms_wsPath = "${projectDirectory}\\${stackName}\\ws"
        def cms_loacalPluginsPath = "E:\\Sites\\Demo\\${stackName}\\plugins"
        def cms_loacalAdminPath = "E:\\Sites\\Demo\\${stackName}\\admin"
        def cms_loacalWsPath = "E:\\Sites\\Demo\\${stackName}\\ws"
        def cms_loacalTempAssembliesAdminFolder = "E:\\Sites\\Demo\\${stackName}\\temp-assemblies-folder-admin"
        def cms_loacalTempAssembliesWsFolder = "E:\\Sites\\Demo\\${stackName}\\temp-assemblies-folder-admin"

        genericTools.mkdir("${projectDirectory}\\${stackName}", true);
        genericTools.mkdir(cms_pluginsPath, true);
        genericTools.mkdir(cms_adminPath, true);
        genericTools.mkdir(cms_wsPath, true);
        
        bat "xcopy /E /S /I /Q /Y /F \"E:\\plugins-directory-hosted\" \"${cms_pluginsPath}\""
        bat "\"C:\\Program Files\\7-Zip\\7z.exe\" x -mx1 -mmt=16 \"\\\\10.0.10.3\\IIS_Shares\\CMS\\Artifacts\\administration.hosted\\administration.hosted.${buildProfile}.win-x64.${adminVersion}.7z\" -o\"${cms_adminPath}\""
        
        dir(cms_adminPath){
            genericTools.del("${cms_adminPath}\\web.config");
            genericTools.del("${cms_adminPath}\\appsettings.json");
            bat "powershell -command \"Get-Content '${cms_adminPath}\\web.default.config' | Out-File -encoding UTF8 -filepath '${cms_adminPath}\\web.config'\""
            bat "powershell -command \"Get-Content '${cms_adminPath}\\appsettings.default.json' | Out-File -encoding ASCII -filepath '${cms_adminPath}\\appsettings.json'\""
            def adminAppSettingsObject = readJSON file: "appsettings.json", returnPojo: true
            adminAppSettingsObject["Config"]["MD.Tools.Helpers.Core"]["TempAssembliesFolder"] = cms_loacalTempAssembliesAdminFolder.inspect()
            adminAppSettingsObject["Config"]["MD.CMS.Administration.Core"]["UploadsRootPath"] = cms_uploadsRoot.inspect()
            adminAppSettingsObject["Config"]["MD.CMS.Administration.Core"]["PluginsDirectory"] = cms_loacalPluginsPath.inspect()
            adminAppSettingsObject["Config"]["MD.CMS.Administration.Core"]["UiDebugMode"] = "false"
            adminAppSettingsObject["Config"]["MD.CMS.BusinessLogic.Core"]["ProductionMode"] = "true"
            def adminAppSettingsFile = new File("${cms_adminPath}\\appsettings.json")
            adminAppSettingsFile.write(new JsonBuilder(adminAppSettingsObject).toPrettyString())
        }

        bat "\"C:\\Program Files\\7-Zip\\7z.exe\" x -mx1 -mmt=16 \"\\\\10.0.10.3\\IIS_Shares\\CMS\\Artifacts\\webapi.hosted\\webapi.hosted.${buildProfile}.win-x64.${webApiVersion}.7z\" -o\"${cms_wsPath}\""
        
        dir(cms_wsPath){
            genericTools.del("${cms_wsPath}\\web.config");
            genericTools.del("${cms_wsPath}\\appsettings.json");
            bat "powershell -command \"Get-Content '${cms_wsPath}\\web.default.config' | Out-File -encoding UTF8 -filepath '${cms_wsPath}\\web.config'\""
            bat "powershell -command \"Get-Content '${cms_wsPath}\\appsettings.default.json' | Out-File -encoding ASCII -filepath '${cms_wsPath}\\appsettings.json'\""
            def wsAppSettingsObject = readJSON file: "appsettings.json", returnPojo: true
            wsAppSettingsObject["Config"]["MD.Tools.Helpers.Core"]["TempAssembliesFolder"] = cms_loacalTempAssembliesWsFolder.inspect()
            wsAppSettingsObject["Config"]["MD.CMS.BusinessLogic.Core"]["SessionDomain"] = "${stackName}.demo.omegacms.run".inspect()
            wsAppSettingsObject["Config"]["MD.CMS.BusinessLogic.Core"]["FileUploadPath"] = cms_uploadsRoot.inspect()
            wsAppSettingsObject["Config"]["MD.CMS.WebApi.Core"]["PluginsDirectory"] = cms_loacalPluginsPath.inspect()
            wsAppSettingsObject["Config"]["MD.Tools.BaseDataAccess.Plugins.Core"]["BaseDataAccessPluginsDirectory"] = cms_loacalPluginsPath.inspect()
            wsAppSettingsObject["Config"]["MD.Tools.BaseDataAccess.Plugins.Core"]["DataAccessPlugins"] = ["MD.Tools.BaseDataAccess.Plugins.MySql.Core"]
            wsAppSettingsObject["Config"]["MD.Tools.BaseDataAccess.Plugins.Core"]["DataAccessPluginSettings"]["MD.Tools.BaseDataAccess.Plugins.MySql.Core"] = "DataSource=${dbServerAddress};Database=${dbName};User=${dbUser};Password=${dbPassword};CharSet=utf8;convert zero datetime=True;Default Command Timeout=60".inspect()
            def wAppSettingsFile = new File("${cms_wsPath}\\appsettings.json")
            wAppSettingsFile.write(new JsonBuilder(wsAppSettingsObject).toPrettyString())
        }

        powershell ".\\Jenkins-Local-Site-Setup.ps1 -username Administrator -password ${genericTools.getEnvString("STAGING_WINRM_PASSWORD", "")} -remotecomputer stagingserver2019standard -stackName \"${stackName}\" -projectDirectory \"E:\\Sites\\Demo\""

        def postFields = []
        postFields = genericTools.appendObjectVariable(postFields, "auth-id", genericTools.getEnvString("CLOUDNS_AUTH_ID", "5159"), false, true)
        postFields = genericTools.appendObjectVariable(postFields, "auth-password", genericTools.getEnvString("CLOUDNS_AUTH_PASSWORD", ""), false, true)
        postFields = genericTools.appendObjectVariable(postFields, "domain-name", "omegacms.run", false, true)
        postFields = genericTools.appendObjectVariable(postFields, "record-type", "CNAME", false, true)
        postFields = genericTools.appendObjectVariable(postFields, "host", "${stackName}.demo", false, true)
        postFields = genericTools.appendObjectVariable(postFields, "record", "d3cw2b5uipkpw5.cloudfront.net", false, true)
        postFields = genericTools.appendObjectVariable(postFields, "ttl", "3600", false, true)

        httpRequest "https://api.cloudns.net/dns/add-record.json?${postFields.join('&')}"
    } catch (Exception e) {
        println "Error occured during site deployment!"
        rollbackRequired = true
        rollbackException = e
        println e.toString()
    }
}

pipeline {
    agent any
    parameters {
        string(name: "uniqueId", description: "Unique Id", defaultValue: "")
        choice(name: "buildProfile", choices: ["Release", "Debug"], description: "Build profile")
        choice(name: "version", choices: getCmsVersions(), description: "Omega Version")
        string(name: "clientName", description: "Client Name", defaultValue: "Omega Test Client")
        string(name: "clientId", description: "Client Id", defaultValue: "0")
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
        password(name: "setupDbUser", description: "CMS Database User", defaultValue: "root")
        password(name: "setupDbPassword", description: "CMS Database Password", defaultValue: "")
        string(name: "dbServerAddress", description: "Database Server Address", defaultValue: "127.0.0.1")
        string(name: "dbServerPort", description: "Database Server Port", defaultValue: "3306")
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
        stage("Site Deployment"){
            steps {
                script {
                    stage_siteDeployment(params)
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
                                println "Skipping Commit to Registry: set OMEGA_CMS_INTERNAL_WS_BASE (see Jenkins/omega-pipeline.env.example)"
                            } else {
                            def regPwd = params.rootAccountPassword
                            def cms_authToken = cmsTools.cms_login(cms_httpBase, "Admin", regPwd)

                            def content = cmsTools.cms_constructContent(cmsTools.cms_getContentType(cms_httpBase, cms_authToken, 5))
                            content.Title = "Omega CMS Demo ${clientName}"
                            content.FolderId = 14
                            def websiteUrl = "${stackName}.demo.omegacms.run".inspect()
                            cmsTools.cms_contentSetFieldValue(content, "Omega Customer", "${clientId}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Unique Id", "${uniqueId}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Stack Name", "${stackName}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Database Host Address", "${dbServerAddress}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Database Port", "${dbServerPort}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Database Name", "${dbName}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Database User", "${dbUser}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Database Password", "${dbPassword}".inspect())
                            cmsTools.cms_contentSetFieldValue(content, "Url", "${websiteUrl}".inspect())
                            cmsTools.cms_saveContent(cms_httpBase, cms_authToken, content)

                            cmsTools.cms_postToCms(cms_httpBase, "DemoSetup/NotifyDone", cms_authToken, [
                                "Version": "${adminVersion}",
                                "UniqueId": "${uniqueId}",
                                "BuildId": "0",
                                "Id": "${clientId}",
                                "Building": "false",
                                "ClientName": "${stackName}",
                                "Domain": "${websiteUrl}",
                                "Url": "https://${websiteUrl}",
                                "Username": "Admin",
                                "Password": "${regPwd}",
                                "Email": ""
                            ], false)
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
        stage("Rollback"){
            steps{
                script {
                    if (rollbackRequired || forceRollback) {
                        
                        try {
                            genericTools.rmdir("${projectDirectory}\\${stackName}")
                        } catch (Exception e) {
                        }
    
                        println "Dropping database on (${dbServerAddress})..."
                        try {
                            genericTools.executeSql(dbServerAddress, dbServerPort, setupDbUser, setupDbPassword, "", "DROP DATABASE ${dbName};")
                        } catch (Exception e) {
                        }

                        try {
                            genericTools.executeSql(dbServerAddress, dbServerPort, setupDbUser, setupDbPassword, "", "DROP USER '${dbUser}'@'%';")
                        } catch (Exception e) {
                        }

                        try {
                            genericTools.executeSql(dbServerAddress, dbServerPort, setupDbUser, setupDbPassword, "", "FLUSH PRIVILEGES;")
                        } catch (Exception e) {
                        }
                        println "Dropping created on (${dbServerAddress})..."

                        if (rollbackException != null) {
                            error(rollbackException.toString())
                        }
                    }
                }
            }
        }
    }
}