import java.time.*
import java.time.format.DateTimeFormatter
import groovy.json.*

def getUniqueId(removeHyphens = true){
    def id = UUID.randomUUID().toString()
    if(removeHyphens){
        id = id.replace("-", "")
    }
    return id
}

def getCommandOutput(cmd, fullResult = false) {
    try {
        if (isUnix()) {
            return sh(returnStdout: true, script: "#!/bin/sh -e\n" + cmd).trim()
        } else {
            if (fullResult) {
                return bat(returnStdout: true, script: cmd).trim().readLines().drop(1)
            }
            return bat(returnStdout: true, script: cmd).trim().readLines().drop(1).join(" ")
        }
    } catch (Exception e) {
        println "Error occured while executing the script (${cmd}) ${e.toString()}"
        throw e;
    }
    return null
}

def getCommandOutputLastLine(cmd, ignoreWhiteSpace = true) {
    try {
        if (isUnix()) {
            return sh(returnStdout: true, script: "#!/bin/sh -e\n" + cmd).trim()
        } else {
            def res = bat(returnStdout: true, script: cmd).trim().readLines()
            def len = res.size()

            if(!ignoreWhiteSpace) {
                return res.get(len-1).toString()
            }

            while(len > 0) {
                if(res.get(len-1).toString().trim() != "") {
                    return res.get(len-1).toString()
                }
                len = len - 1
            }

            return null
        }
    } catch (Exception e) {
        println "Error occured while executing the script (${cmd}) ${e.toString()}"
        throw e;
    }
    return null
}

def robocopy(cmd) {
    // robocopy uses non-zero exit code even on success, status below 3 is fine
    def status = bat returnStatus: true, script: "ROBOCOPY ${cmd}"
    println "ROBOCOPY returned ${status}"
    if (status < 0 || status > 3) {
        error("ROBOCOPY failed")
    }
}

def mkdir(path, delete = false) {
    if (delete) {
        rmdir(path, true)
    }
    bat "if not exist \"${path}\" (mkdir \"${path}\")"
}

def rmdir(path, checkIfExists = true) {
    def command = "rmdir /Q/S \"${path}\""
    if (checkIfExists) {
        command = "if exist \"${path}\" (${command})"
    }
    bat command
}

def del(path, checkIfExists = true) {
    try{
        def command = "del \"${path}\""
        if (checkIfExists) {
            command = "if exist \"${path}\" (${command})"
        }
        bat command
    } catch(Exception e){
        println "Error occured \"${e.toString()}\" while deleting file \"${path}\", continuing..."
    }
}

def trim(str, maxLength, offset = 0){
    return str.substring(offset, (str.length() > maxLength ? maxLength : str.length()))
}

def appendObjectVariable(arrayObject, key, value, includeQuotes = false, encodeValue = false) {
    if(includeQuotes) {
        key = key.replace('\\', '\\\\').replace('"', '\\"')
        value = value.replace('\\', '\\\\').replace('"', '\\"')
        if(encodeValue){
            arrayObject.add("\"${key}\"=\"${java.net.URLEncoder.encode(value, 'UTF-8')}\"")
        } else {
            arrayObject.add("\"${key}\"=\"${value}\"")
        }
    } else {
        if(encodeValue){
            arrayObject.add("${key}=${java.net.URLEncoder.encode(value, 'UTF-8')}")
        } else {
            arrayObject.add("${key}=${value}")
        }
    }
    return arrayObject
}

def executeSql(host, port, user, password, database, command, isFile = false) {
    def cmd = "\"E:\\mysql\\bin\\mysqlsh.exe\" --user=${user} --password=${password} --host ${host} --port=${port}"
    if(database != ""){
        cmd = "${cmd} --database ${database}"
    }

    if(isFile) {
        cmd = "${cmd} -f \"E:\\lambda-sql-setup\\${command}\""
    } else {
        cmd = "${cmd} --sql -e \"${command}\""
    }

    bat cmd
}

def escapeCharacter(str, character, escapeWith = "\\") {
    return str.replace(character, "${escapeWith}${character}")
}

// See repo-root .env.example and Jenkins/omega-pipeline.env.example
def getEnvString(String key, String defaultValue = null) {
    def v = System.getenv(key)
    if (v != null && v != "")
        return v
    return defaultValue
}

return this