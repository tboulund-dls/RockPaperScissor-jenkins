pipeline {
    agent any
    triggers {
        pollSCM("* * * * *")
    }
    stages {
        stage("Build") {
            steps {
                sh "dotnet build"
            }
        }
        // stage("Deliver") {
            
        // }
        // stage("Deploy") {

        // }
    }
}