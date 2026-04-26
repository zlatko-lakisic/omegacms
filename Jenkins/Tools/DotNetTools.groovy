import java.time.*
import java.time.format.DateTimeFormatter
import groovy.json.*

def genericTools = null

def init(){
    genericTools = load "Jenkins\\Tools\\GeneralTools.groovy"
}

def publish(projectDirectory, publishDirectory, isLibs, project, projectShort, version, mode, platform, postPublishRoboCopyCommand = "") {
    def libsPrefix = ""
    if(isLibs){
        libsPrefix = "lib\\"
    }
    dir(projectDirectory) {
        echo("Working with version v${version}")
        dir(project) {
            if(isLibs){
                try { genericTools.mkdir("${publishDirectory}\\${libsPrefix}") } finally {}
            }
            publishDirectory = "${publishDirectory}\\${libsPrefix}${projectShort}"
            try { genericTools.mkdir("${publishDirectory}") } finally {}
            try { genericTools.mkdir("${publishDirectory}\\source") } finally {}
            try { genericTools.mkdir("${publishDirectory}\\source\\${mode}") } finally {}
            try { genericTools.mkdir("${publishDirectory}\\source\\${mode}\\${platform}", true) } finally {}
            bat "publish-project.bat \"${mode}\" \"${platform}\" \"${publishDirectory}\\source\\${mode}\\${platform}\""
            if(!isLibs){
                try {
                    dir("${publishDirectory}\\source\\${mode}\\${platform}") {
                        genericTools.del("web.config");
                    }
                } finally {
                }
                switch(mode.toLowerCase()){
                    case "debug":
                        if (postPublishRoboCopyCommand != "") {
                            try {
                                genericTools.robocopy(postPublishRoboCopyCommand)
                                genericTools.del("output.log");
                            } finally {
                            }
                        }
                    break;
                    case "release":
                        try {
                            dir("${publishDirectory}\\source\\${mode}\\${platform}") {
                                dir("wwwroot") {
                                    dir("scripts") {
                                        genericTools.rmdir("businessLogic_ts");
                                        genericTools.rmdir("businessLogicMinified");
                                        dir("app-tmp") {

                                        }
                                        dir("app") {
                                            if(project == "MD.CMS.Administration.Core.Hosted") {
                                                bat "xcopy /Y core\\directives\\md-cms-grid\\md-cms-grid-canvas.min.css ..\\app-tmp\\"
                                                bat "xcopy /Y core\\directives\\md-generictype-designer\\form\\md-generictype-designer-form-canvas.min.css ..\\app-tmp\\"
                                            }
                                            bat "del /s *.js"
                                            bat "del /s *.css"
                                            bat "del /s *.scss"
                                            if(project == "MD.CMS.Administration.Core.Hosted") {
                                                bat "xcopy /Y ..\\app-tmp\\md-cms-grid-canvas.min.css core\\directives\\md-cms-grid\\"
                                                bat "xcopy /Y ..\\app-tmp\\md-generictype-designer-form-canvas.min.css core\\directives\\md-generictype-designer\\form\\"
                                            }
                                        }
                                        genericTools.rmdir("app-tmp");
                                        genericTools.rmdir("app@tmp");
                                    }
                                    genericTools.rmdir("scripts@tmp");
                                }
                                genericTools.del("appsettings.json");
                                genericTools.del("appSettings.json");
                            }
                        } finally {
                        }
                    break;
                }
            }
        }
    }
}

def ensureArtifactDirectoryExists(baseArtifactDirectory, isLibs, projectShort) {
    def libsPrefix = ""
    if(isLibs){
        libsPrefix = "lib\\"
    }
    script {
        if(isLibs){
            try { genericTools.mkdir("${baseArtifactDirectory}\\${libsPrefix}") } finally {}
        }
        try { genericTools.mkdir("${baseArtifactDirectory}\\${libsPrefix}${projectShort}") } finally {}
    }
    return "${baseArtifactDirectory}\\${libsPrefix}${projectShort}"
}

def publishArtifact(publishDirectory, baseArtifactDirectory, isLibs, projectShort, version, mode, platform){
    def libsPrefix = ""
    if(isLibs){
        libsPrefix = "lib\\"
    }
    def artifactsDirectory = ensureArtifactDirectoryExists(baseArtifactDirectory, isLibs, projectShort)
    script {
        try {
            bat "if exist ${artifactsDirectory}\\${projectShort}.${mode}.${platform}.${version}.7z del ${artifactsDirectory}\\${projectShort}.${mode}.${platform}.${version}.7z"
        } catch(e) {
            echo "File not found for deletion! Continuing..."
        }
    }
    dir("${publishDirectory}\\${libsPrefix}${projectShort}"){
        bat "\"C:\\Program Files\\7-Zip\\7z.exe\" a -r -t7z -m0=LZMA2 -mx1 -mmt=16 \"${artifactsDirectory}\\${projectShort}.${mode}.${platform}.${version}.7z\" \".\\source\\${mode}\\${platform}\\*\""
    }
}

def publishArtifactFile(publishDirectory, baseArtifactDirectory, isLibs, projectShort, fileName){
    def artifactsDirectory = ensureArtifactDirectoryExists(baseArtifactDirectory, isLibs, projectShort)
    script {
        try {
            bat "xcopy /I /Q /Y /F \"${publishDirectory}\\${fileName}\" \"${artifactsDirectory}\""
        } catch(e) {
            echo "Cannot copy file, result ${e.toString()}"
        }
    }
}

def publishNuget(projectDirectory, project, projectShort, version, mode, platform, nugetKey){
    def feed = System.getenv("NUGET_SOURCE_URL")
    if (feed == null || feed == "") {
        error "Set NUGET_SOURCE_URL in the agent environment (see .env.example / Jenkins/omega-pipeline.env.example)"
    }
    def libsPrefix = "lib\\"
    dir("${publishDirectory}\\${libsPrefix}${projectShort}\\source\\${mode}\\${platform}\\"){
        bat "dotnet nuget push -s ${feed} -k ${nugetKey} ${project}.${version}.nupkg"
    }
}

def publishAll(projectDirectory, publishDirectory, isLibs, project, projectShort, version, mode, platforms = [], postPublishRoboCopyCommand = "") {
    for (platform in platforms) {
        publish(projectDirectory, publishDirectory, isLibs, project, projectShort, version, mode, platform, postPublishRoboCopyCommand)
    }
}

def buildAndPublishLib(projectDirectory, publishDirectory, baseArtifactDirectory, project, projectShort, mode, nugetKey) {
    script {
        def version = 1
        dir(projectDirectory) {
            version = genericTools.getCommandOutput(".\\get-version.bat ${project}\\${project}.csproj")
        }
        publish(projectDirectory, publishDirectory, true, project, projectShort, version, mode, "portable")
        publishArtifact(publishDirectory, baseArtifactDirectory, true, projectShort, version, mode, "portable")
        publishNuget(projectDirectory, project, projectShort, version, mode, "portable", nugetKey)
    }
}

return this