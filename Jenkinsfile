properties([pipelineTriggers([githubPush()])])

pipeline {
    agent any

    stages {
        stage('hello') {
            steps {
                // Get some code from a GitHub repository
                echo 'hello1'
            }
        }
        stage('deleteWorkspace') {
            steps {
                deleteDir()
            }
        }
        stage('clone') {
            steps {
                // Get some code from a GitHub repository
                git branch: 'main',
                url: 'https://github.com/ProductivityTools-Journal/ProductivityTools.Journal.Api'
            }
        }
        stage('build') {
            steps {
                bat(script: "dotnet publish ProductivityTools.Journal.Api.sln -c Release ", returnStdout: true)
            }
        }
        stage('deleteDbMigratorDir') {
            steps {
                bat('if exist "C:\\Bin\\JournalApiDdbMigration" RMDIR /Q/S "C:\\Bin\\JournalApiDdbMigration"')
            }
        }
        stage('copyDbMigratdorFiles') {
            steps {
                bat('xcopy "src\\Server\\ProductivityTools.Journal.DbUp\\bin\\Release\\net6.0\\publish" "C:\\Bin\\JournalApiDdbMigration\\" /O /X /E /H /K')
            }
        }

        stage('runDbMigratorFiles') {
            steps {
                bat('C:\\Bin\\JournalApiDdbMigration\\ProductivityTools.Journal.DbUp.exe')
            }
        }

        stage('Stop PTJournal on IIS') {
            steps {
                bat('%windir%\\system32\\inetsrv\\appcmd stop site /site.name:PTJournal')
            }
        }
		
		stage('Stop PTJournal AppPool') {
            steps {
                bat('%windir%\\system32\\inetsrv\\appcmd stop apppool /apppool.name:"PTJournal"')
            }
        }
		

        stage('Delete IIS Journal Directory') {
            steps {
                retry(5) {
                    bat('if exist "C:\\Bin\\IIS\\Journal" RMDIR /Q/S "C:\\Bin\\IIS\\PTJournal"')
                }

            }
        }
        stage('Copy IIS files') {
            steps {         
                bat('xcopy "src\\Server\\ProductivityTools.Journal.WebApi\\bin\\Release\\net6.0\\publish" "C:\\Bin\\IIS\\PTJournal\\" /O /X /E /H /K')
            }
        }
		
		stage('Start PTJournal AppPool') {
            steps {
                bat('%windir%\\system32\\inetsrv\\appcmd start apppool /apppool.name:"PTJournal"')
            }
        }

        stage('Start PTJournal on IIS') {
            steps {
                bat('%windir%\\system32\\inetsrv\\appcmd start site /site.name:PTJournal')
            }
        }
        stage('byebye') {
            steps {
                // Get some code from a GitHub repository
                echo 'byebye1'
            }
        }
    }
	post {
		always {
            emailext body: "${currentBuild.currentResult}: Job ${env.JOB_NAME} build ${env.BUILD_NUMBER}\n More info at: ${env.BUILD_URL}",
                recipientProviders: [[$class: 'DevelopersRecipientProvider'], [$class: 'RequesterRecipientProvider']],
                subject: "Jenkins Build ${currentBuild.currentResult}: Job ${env.JOB_NAME}"
		}
	}
}
