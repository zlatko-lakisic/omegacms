// Environment variables (set in Jenkins or copy .env on agents — see .env.example and Jenkins/omega-pipeline.env.example):
//   OMEGA_REMOTE_JENKINS_NAME — e.g. Jenkins instance id for "Parameterized Remote Trigger"
//   OMEGA_REMOTE_JENKINS_JOB_MASTER — remote job name for master branch
//   OMEGA_REMOTE_JENKINS_JOB_PRODUCTION — remote job name for production branch
//   OMEGA_REMOTE_JENKINS_TOKEN — remote build authentication token (never commit)
//   OMEGA_SLACK_CHANNEL, OMEGA_SLACK_CREDENTIAL_ID — optional; if unset, slackSend is skipped

def getEnvString(String k, String defaultValue = null) {
    def v = System.getenv(k)
    if (v != null && v != "")
        return v
    return defaultValue
}

pipeline {
    agent any

    stages {
        stage('Master Commit') {
            when {
                expression { env.BRANCH_NAME == 'master' }
            }
            steps {
                script {
                    def remoteJenkins = getEnvString("OMEGA_REMOTE_JENKINS_NAME")
                    def jobName = getEnvString("OMEGA_REMOTE_JENKINS_JOB_MASTER")
                    def jobToken = getEnvString("OMEGA_REMOTE_JENKINS_TOKEN")
                    if (!remoteJenkins || !jobName || !jobToken) {
                        error "Set OMEGA_REMOTE_JENKINS_NAME, OMEGA_REMOTE_JENKINS_JOB_MASTER, and OMEGA_REMOTE_JENKINS_TOKEN in Jenkins (see Jenkins/omega-pipeline.env.example)"
                    }
                    def handle = triggerRemoteJob(
                                    remoteJenkinsName: remoteJenkins,
                                    job: jobName,
                                    parameters: "token=${jobToken}",
                                    blockBuildUntilComplete: true)
                    echo 'Log: ' + handle.lastLog().toString()
                    echo 'Remote Status: ' + handle.getBuildStatus().toString()
                }
            }
        }
        stage('Production Deploy') {
            when {
                expression { env.BRANCH_NAME == 'production' }
            }
            steps {
                script {
                    def remoteJenkins = getEnvString("OMEGA_REMOTE_JENKINS_NAME")
                    def jobName = getEnvString("OMEGA_REMOTE_JENKINS_JOB_PRODUCTION")
                    def jobToken = getEnvString("OMEGA_REMOTE_JENKINS_TOKEN")
                    if (!remoteJenkins || !jobName || !jobToken) {
                        error "Set OMEGA_REMOTE_JENKINS_NAME, OMEGA_REMOTE_JENKINS_JOB_PRODUCTION, and OMEGA_REMOTE_JENKINS_TOKEN in Jenkins (see Jenkins/omega-pipeline.env.example)"
                    }
                    def handle = triggerRemoteJob(
                                    remoteJenkinsName: remoteJenkins,
                                    job: jobName,
                                    parameters: "token=${jobToken}",
                                    blockBuildUntilComplete: true)
                    echo 'Log: ' + handle.lastLog().toString()
                    echo 'Remote Status: ' + handle.getBuildStatus().toString()
                }
            }
        }
    }
    post {
        success {
            script {
                switch(env.BRANCH_NAME) {
                  case "production":
                    emailext(
                        to: '$DEFAULT_RECIPIENTS',
                        body: "${currentBuild.currentResult}: Job ${env.JOB_NAME} build ${env.BUILD_NUMBER}\n More info at: ${env.BUILD_URL}",
                        subject: "Jenkins Deployment Success ${currentBuild.currentResult}: Job ${env.JOB_NAME}")
                    def slCh = getEnvString("OMEGA_SLACK_CHANNEL")
                    def slCred = getEnvString("OMEGA_SLACK_CREDENTIAL_ID")
                    if (slCh && slCred) {
                        slackSend(channel: slCh, tokenCredentialId: slCred, color: "good", message: "Jenkins Deployment Success ${currentBuild.currentResult}: Job ${env.JOB_NAME}, ${currentBuild.currentResult}: Job ${env.JOB_NAME} build ${env.BUILD_NUMBER}\n More info at: ${env.BUILD_URL}")
                    }
                    break
                  case "master":
                    break
                }
            }

        }
        failure {
            script {
                switch(env.BRANCH_NAME) {
                  case "production":
                    emailext(
                        to: '$DEFAULT_RECIPIENTS',
                        body: "${currentBuild.currentResult}: Job ${env.JOB_NAME} build ${env.BUILD_NUMBER}\n More info at: ${env.BUILD_URL}",
                        subject: "Jenkins Deployment Failure ${currentBuild.currentResult}: Job ${env.JOB_NAME}")
                    def slCh = getEnvString("OMEGA_SLACK_CHANNEL")
                    def slCred = getEnvString("OMEGA_SLACK_CREDENTIAL_ID")
                    if (slCh && slCred) {
                        slackSend(channel: slCh, tokenCredentialId: slCred, color: "danger", message: "Jenkins Deployment Failure ${currentBuild.currentResult}: Job ${env.JOB_NAME}, ${currentBuild.currentResult}: Job ${env.JOB_NAME} build ${env.BUILD_NUMBER}\n More info at: ${env.BUILD_URL}")
                    }
                    break
                  case "master":
                    break
                }
            }
        }
    }
}
