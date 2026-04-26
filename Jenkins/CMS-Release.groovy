import groovy.json.*
import groovy.xml.*
import groovy.util.Node

def adminVersion = 0
def webApiVersion = 0
def asyncTaskProcessorVersion = 0
def installerVersion = 0
def buildProfile = ""
def baseProjectDirectory = ""
def basePublishDirectory = ""
def baseArtifactDirectory = ""
def nugetKey = ""
def genericTools = null
def dotNetTools = null
def cmsTools = null
def awsTools = null;

pipeline {
    agent any
    parameters {
        choice(name: "buildProfile", choices: ["Release", "Debug"], description: "Build profile")
        string(name: "projectDirectory", defaultValue: "E:\\Projects\\cms", description: "Project Directory")
        string(name: "publishDirectory", defaultValue: "E:\\Projects\\cms-dist", description: "Publish Directory")
        string(name: "artifactDirectory", defaultValue: "\\\\10.0.10.3\\IIS_Shares\\CMS\\Artifacts", description: "Artifact Directory")
        password(name: "nugetKey", defaultValue: "", description: "Nuget Key (set NUGET_SOURCE_URL for the feed — see .env.example)")
    }
    stages {
        stage("Load Modules"){
            steps {
                script{
                    try {
                        buildProfile = params.buildProfile
                        baseProjectDirectory = params.projectDirectory
                        basePublishDirectory = params.publishDirectory
                        baseArtifactDirectory = params.artifactDirectory
                        nugetKey = params.nugetKey
                    } catch (Exception e){
                        error("Error happened during parameter initiation phase! The error is: ${e.toString()}")
                    }
                        
                    try {
                        genericTools = load "Jenkins\\Tools\\GeneralTools.groovy"
                        dotNetTools = load "Jenkins\\Tools\\DotNetTools.groovy"
                        cmsTools = load "Jenkins\\Tools\\CMSTools.groovy"
                        awsTools = load "Jenkins\\Tools\\AWSTools.groovy"
                    } catch (Exception e){
                        error("Error happened during module load phase! The error is: ${e.toString()}")
                    }
                        
                    try {
                        dotNetTools.init()
                        cmsTools.init()
                    } catch (Exception e){
                        error("Error happened during module init phase! The error is: ${e.toString()}")
                    }
                }
            }
        }
        stage("Git Pull"){
            steps{
                dir(baseProjectDirectory){
                    bat "git reset --hard"
                    bat "git pull"
                    bat "git pull --recurse-submodules"
                }
            }
        }
        stage("Build Dependencies") {
            steps{
                script {
                    cmsTools.buildDependencies(buildProfile, baseProjectDirectory)
                }
            }
        }
        stage("Build and Publish Libs") {
            steps {
                script {
                    cmsTools.buildAndPublishLibs(buildProfile, baseProjectDirectory, basePublishDirectory, baseArtifactDirectory, nugetKey)
                }
            }
        }
        stage("Build Administration Hosted") {
            steps {
                script {
                    dir(baseProjectDirectory) {
                        adminVersion = genericTools.getCommandOutput(".\\get-version.bat MD.CMS.Administration.Core.Hosted\\MD.CMS.Administration.Core.Hosted.csproj")
                    }
                    cmsTools.buildAdminHosted(buildProfile, baseProjectDirectory, basePublishDirectory, adminVersion)
                }
            }
        }
        stage("Publish Administration Hosted") {
            steps {
                parallel(
                    "win-x64": {
                        script {
                            dotNetTools.publishArtifact(basePublishDirectory, baseArtifactDirectory, false, "administration.hosted", adminVersion, buildProfile, "win-x64")
                        }
                    },
                    "linux-x64": {
                        script {
                            dotNetTools.publishArtifact(basePublishDirectory, baseArtifactDirectory, false, "administration.hosted", adminVersion, buildProfile, "linux-x64")
                        }
                    },
                    "osx-x64": {
                        script {
                            dotNetTools.publishArtifact(basePublishDirectory, baseArtifactDirectory, false, "administration.hosted", adminVersion, buildProfile, "osx-x64")
                        }
                    }
                )
            }
        }
        stage("Publish Client Business Logic Library") {
            steps {
                dir("${baseProjectDirectory}\\MD.CMS.Administration\\public-repo"){
                    script {
                        def packageFileObject = null;
                        try {
                            dir("omega-cms-businesslogic") {
                                def npmVersion = readJSON file: "${baseProjectDirectory}\\MD.CMS.Administration\\public-repo\\omega-cms-businesslogic\\package.json", returnPojo: true
                                npmVersion["version"] = adminVersion
                                def npmVersionFile = new File("${baseProjectDirectory}\\MD.CMS.Administration\\public-repo\\omega-cms-businesslogic\\package.json")
                                npmVersionFile.write(new JsonBuilder(npmVersion).toPrettyString())
                                bat "git commit -am \"Automated publishing of business logic library version (${adminVersion}).\""
                                bat "git push"
                                bat "npm publish -access public"
                            }
                        } catch (Exception e) {
                            println "Error while updating and publishing external business logic library."
                        }
                    }
                }
            }
        }
        stage("Build WebApi Hosted") {
            steps {
                script {
                    dir(baseProjectDirectory){
                        webApiVersion = genericTools.getCommandOutput(".\\get-version.bat MD.CMS.WebApi.Core.Hosted\\MD.CMS.WebApi.Core.Hosted.csproj")
                    }
                    cmsTools.buildWebApiHosted(buildProfile, baseProjectDirectory, basePublishDirectory, webApiVersion)
                }
            }
        }
        stage("Publish WebApi Hosted") {
            steps {
                parallel(
                    "win-x64": {
                        script {
                            dotNetTools.publishArtifact(basePublishDirectory, baseArtifactDirectory, false, "webapi.hosted", webApiVersion, buildProfile, "win-x64")
                        }
                    },
                    "linux-x64": {
                        script {
                            dotNetTools.publishArtifact(basePublishDirectory, baseArtifactDirectory, false, "webapi.hosted", webApiVersion, buildProfile, "linux-x64")
                        }
                    },
                    "osx-x64": {
                        script {
                            dotNetTools.publishArtifact(basePublishDirectory, baseArtifactDirectory, false, "webapi.hosted", webApiVersion, buildProfile, "osx-x64")
                        }
                    }
                )
            }
        }
        stage("Build Async Task Processor") {
            steps {
                script {
                    dir(baseProjectDirectory){
                        asyncTaskProcessorVersion = genericTools.getCommandOutput(".\\get-version.bat MD.Tools.AsyncTask.Processor\\MD.Tools.AsyncTask.Processor.csproj")
                    }
                    cmsTools.buildAsyncTaskProcessor(buildProfile, baseProjectDirectory, basePublishDirectory, asyncTaskProcessorVersion)
                }
            }
        }
        stage("PublishAsync Task Processor") {
            steps {
                parallel(
                    "win-x64": {
                        script {
                            dotNetTools.publishArtifact(basePublishDirectory, baseArtifactDirectory, false, "asyncTaskProcessor", asyncTaskProcessorVersion, buildProfile, "win-x64")
                        }
                    },
                    "linux-x64": {
                        script {
                            dotNetTools.publishArtifact(basePublishDirectory, baseArtifactDirectory, false, "asyncTaskProcessor", asyncTaskProcessorVersion, buildProfile, "linux-x64")
                        }
                    },
                    "osx-x64": {
                        script {
                            dotNetTools.publishArtifact(basePublishDirectory, baseArtifactDirectory, false, "asyncTaskProcessor", asyncTaskProcessorVersion, buildProfile, "osx-x64")
                        }
                    }
                )
            }
        }
        stage("Build Installer Hosted") {
            steps {
                script {
                    dir(baseProjectDirectory){
                        installerVersion = genericTools.getCommandOutput(".\\get-version.bat MD.CMS.Installer.Hosted.Core\\MD.CMS.Installer.Hosted.Core.csproj")
                    }
                    cmsTools.buildInstallerHosted(buildProfile, baseProjectDirectory, basePublishDirectory, installerVersion)
                }
            }
        }
        stage("Publish Installer Hosted") {
            steps {
                parallel(
                    "win-x64": {
                        script {
                            dotNetTools.publishArtifact(basePublishDirectory, baseArtifactDirectory, false, "installer.hosted", installerVersion, buildProfile, "win-x64")
                        }
                    },
                    "linux-x64": {
                        script {
                            dotNetTools.publishArtifact(basePublishDirectory, baseArtifactDirectory, false, "installer.hosted", installerVersion, buildProfile, "linux-x64")
                        }
                    },
                    "osx-x64": {
                        script {
                            dotNetTools.publishArtifact(basePublishDirectory, baseArtifactDirectory, false, "installer.hosted", installerVersion, buildProfile, "osx-x64")
                        }
                    }
                )
            }
        }
            
        stage("Build and Publish Administration AWS Lambda") {
            steps {
                script {
                    dir(baseProjectDirectory) {
                        dir("MD.CMS.Administration.Core.AwsLambda") {
                            genericTools.mkdir("\"wwwroot\\scripts\"", true)
                            bat "xcopy /E /S /I /Q /Y /F \"..\\MD.CMS.Administration\\MD.CMS.Administration.Core.Web\\scripts\" \"wwwroot\\scripts\""
                            genericTools.rmdir("\"wwwroot\\scripts\\.scannerwork\"", true)

                            def packageName = cmsTools.buildAwsLambdaPackage(buildProfile, cmsTools.getAdminAwsLambdaPackageName(adminVersion), "${baseProjectDirectory}\\MD.CMS.Administration.Core.AwsLambda", [".\\bin\\Release\\netcoreapp3.1\\publish\\wwwroot", "..\\AwsLambdaPlugins"])
                            dotNetTools.publishArtifactFile("${baseProjectDirectory}\\MD.CMS.Administration.Core.AwsLambda", baseArtifactDirectory, false, "admin.awslambda", packageName)

                            def layerName = cmsTools.packageDotnetLambdaLayer([
                                projectDirectory: "${baseProjectDirectory}\\MD.CMS.Administration.Core.AwsLambda",
                                artifactsDirectory: "${baseArtifactDirectory}\\admin.awslambda\\",
                                configuration: buildProfile,
                                destination: "${baseProjectDirectory}\\admin.awslambda\\",
                                projectShort: "MD.CMS.Administration.Core.AwsLambda",
                                version: adminVersion
                            ])

                            awsTools.uploadFileToS3("${baseArtifactDirectory}\\admin.awslambda\\${layerName}.zip", "md-cms-administration-core-awslambda-layers")
                            awsTools.publishLambdaLayer("MD-CMS-Administration-Core-AwsLambda-v${adminVersion}", "MD.CMS.Administration.Core.AwsLambda version ${adminVersion}", "md-cms-administration-core-awslambda-layers", "${layerName}.zip")
                        }
                        
                        def artifactsDirectory = dotNetTools.ensureArtifactDirectoryExists(baseArtifactDirectory, false, "admin.awslambda")
                        bat "xcopy /E /S /I /Q /Y /F /R \"MD.CMS.Administration\\MD.CMS.Administration.Core.Web\" \"${artifactsDirectory}\\assets.${adminVersion}\""
                        genericTools.rmdir("\"${artifactsDirectory}\\assets.${adminVersion}\\output\"", true)
                        genericTools.rmdir("\"${artifactsDirectory}\\assets.${adminVersion}\\scripts\"", true)
                        genericTools.del("\"${artifactsDirectory}\\assets.${adminVersion}\\config.js\"", true)
                        genericTools.del("\"${artifactsDirectory}\\assets.${adminVersion}\\runtime.php\"", true)
                        awsTools.uploadFileToS3("${artifactsDirectory}\\assets.${adminVersion}", "omega-cms-admin/assets/assets.${adminVersion}", "--recursive")
                    }
                }
            }
        }
        stage("Build and Publish Web Api AWS Lambda") {
            steps {
                script {
                    dir(baseProjectDirectory) {
                        dir("MD.CMS.WebApi.Core.AwsLambda") {
                            def packageName = cmsTools.buildAwsLambdaPackage(buildProfile, cmsTools.getWsAwsLambdaPackageName(webApiVersion), "${baseProjectDirectory}\\MD.CMS.WebApi.Core.AwsLambda", ["..\\AwsLambdaPlugins"])
                            dotNetTools.publishArtifactFile("${baseProjectDirectory}\\MD.CMS.WebApi.Core.AwsLambda", baseArtifactDirectory, false, "webapi.awslambda", packageName)

                            def layerName = cmsTools.packageDotnetLambdaLayer([
                                projectDirectory: "${baseProjectDirectory}\\MD.CMS.WebApi.Core.AwsLambda",
                                artifactsDirectory: "${baseArtifactDirectory}\\webapi.awslambda\\",
                                configuration: buildProfile,
                                destination: "${baseProjectDirectory}\\webapi.awslambda\\",
                                projectShort: "MD.CMS.WebApi.Core.AwsLambda",
                                version: adminVersion
                            ])
                            
                            awsTools.uploadFileToS3("${baseArtifactDirectory}\\webapi.awslambda\\${layerName}.zip", "md-cms-webapi-core-awslambda-layers")
                            awsTools.publishLambdaLayer("MD-CMS-WebApi-Core-AwsLambda-v${adminVersion}", "MD.CMS.WebApi.Core.AwsLambda version ${adminVersion}", "md-cms-webapi-core-awslambda-layers", "${layerName}.zip")
                        }
                    }
                }
            }
        }
        stage("Build and Publish Web Api Sockets AWS Lambda") {
            steps {
                script {
                    dir(baseProjectDirectory) {
                        dir("MD.CMS.WebApi.Sockets.Core.AwsLambda") {
                            def packageName = cmsTools.buildAwsLambdaPackage(buildProfile, cmsTools.getWsSocketAwsLambdaPackageName(webApiVersion), "${baseProjectDirectory}\\MD.CMS.WebApi.Sockets.Core.AwsLambda", ["..\\AwsLambdaPlugins"])
                            dotNetTools.publishArtifactFile("${baseProjectDirectory}\\MD.CMS.WebApi.Sockets.Core.AwsLambda", baseArtifactDirectory, false, "webapisockets.awslambda", packageName)

                            def layerName = cmsTools.packageDotnetLambdaLayer([
                                projectDirectory: "${baseProjectDirectory}\\MD.CMS.WebApi.Sockets.Core.AwsLambda",
                                artifactsDirectory: "${baseArtifactDirectory}\\webapisockets.awslambda\\",
                                configuration: buildProfile,
                                destination: "${baseProjectDirectory}\\webapisockets.awslambda\\",
                                projectShort: "MD.CMS.WebApi.Sockets.Core.AwsLambda",
                                version: adminVersion
                            ])
                            
                            awsTools.uploadFileToS3("${baseArtifactDirectory}\\webapisockets.awslambda\\${layerName}.zip", "md-cms-webapi-sockets-core-awslambda-layers")
                            awsTools.publishLambdaLayer("MD-CMS-WebApi-Sockets-Core-AwsLambda-v${adminVersion}", "MD.CMS.WebApi.Sockets.Core.AwsLambda version ${adminVersion}", "md-cms-webapi-sockets-core-awslambda-layers", "${layerName}.zip")
                        }
                    }
                }
            }
        }
        stage("Build and Publish AWS Lambda Container") {
            steps {
                script {
                    dir(baseProjectDirectory) {
                        def containerVersion = genericTools.getCommandOutput(".\\get-version.bat MD.CMS.AwsLambda.Container.Core\\MD.CMS.AwsLambda.Container.Core.csproj")
                        dir("MD.CMS.AwsLambda.Container.Core") {
                            def packageName = cmsTools.buildAwsLambdaPackage(buildProfile, cmsTools.getContainerAwsLambdaPackageName(containerVersion), "${baseProjectDirectory}\\MD.CMS.AwsLambda.Container.Core")
                            dotNetTools.publishArtifactFile("${baseProjectDirectory}\\MD.CMS.AwsLambda.Container.Core", baseArtifactDirectory, false, "container.awslambda", packageName)
                            
                            awsTools.uploadFileToS3("${baseArtifactDirectory}\\container.awslambda\\container.awslambda.${containerVersion}", "md-cms-awslambda-container")
                        }
                    }
                }
            }
        }
        stage("Add new version to git"){
            steps{
                script {
                    try {
                        dir(baseProjectDirectory){
                            bat "git tag release-${adminVersion} production"
                            bat "git push --tags"
                            echo("Published new version of CMS release-${adminVersion}")
                        }
                    } catch (Exception e) {
                        println "An error occured while adding new version to git, the error is: ${e.toString()}"
                    }
                }
            }
        }
    }
}