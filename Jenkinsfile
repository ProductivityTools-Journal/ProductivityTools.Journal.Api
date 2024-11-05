properties([pipelineTriggers([githubPush()])])

pipeline {
    agent any

    stages {
        stage('Hello') {
            steps {
                // Get some code from a GitHub repository
                echo 'hello21'
            }
        }
        stage('Print workpalce Path'){
			steps{
				echo "${env.WORKSPACE}"
			}
		}
        stage('Delete Workspace') {
            steps {
                deleteDir()
            }
        }
        stage('Git Clone') {
            steps {
                // Get some code from a GitHub repository
                git branch: 'main',
                url: 'https://github.com/ProductivityTools-Journal/ProductivityTools.Journal.Api'
            }
        }
        stage('Build solution') {
            steps {
                bat(script: "dotnet publish ProductivityTools.Journal.Api.sln -c Release ", returnStdout: true)
            }
        }
        stage('Delete databse migration directory') {
            steps {
                bat('if exist "C:\\Bin\\DbMigration\\PTJournal.Api" RMDIR /Q/S "C:\\Bin\\DbMigration\\PTJournal.Api"')
            }
        }
        stage('Copy database migration files') {
            steps {
                bat('xcopy "src\\Server\\ProductivityTools.Journal.DbUp\\bin\\Release\\net9.0\\publish" "C:\\Bin\\DbMigration\\PTJournal.Api" /O /X /E /H /K')
            }
        }

       stage('Run databse migration files') {
            steps {
                bat('C:\\Bin\\DbMigration\\PTJournal.Api\\ProductivityTools.Journal.DbUp.exe')
            }
        }

        stage('Create page on the IIS') {
            steps {
                powershell('''
                function CheckIfExist($Name){
                    cd $env:SystemRoot\\system32\\inetsrv
                    $exists = (.\\appcmd.exe list sites /name:$Name) -ne $null
                    Write-Host $exists
                    return  $exists
                }
                
                 function Create($Name,$HttpbBnding,$PhysicalPath){
                    $exists=CheckIfExist $Name
                    if ($exists){
                        write-host "Web page already existing"
                    }
                    else
                    {
                        write-host "Creating app pool"
                        .\\appcmd.exe add apppool /name:$Name /managedRuntimeVersion:"v4.0" /managedPipelineMode:"Integrated"
                        write-host "Creating webage"
                        .\\appcmd.exe add site /name:$Name /bindings:http://$HttpbBnding /physicalpath:$PhysicalPath
                        write-host "assign app pool to the website"
                        .\\appcmd.exe set app "$Name/" /applicationPool:"$Name"


                    }
                }
                Create "PTJournal" "*:8008"  "C:\\Bin\\IIS\\PTJournal"                
                ''')
            }
        }

        stage('Stop PTJournal on IIS') {
            steps {
                bat('%windir%\\system32\\inetsrv\\appcmd stop site /site.name:PTJournal')
            }
        }
		
        stage('Delete PTExpenses IIS directory') {
            steps {
              powershell('''
                if ( Test-Path "C:\\Bin\\IIS\\PTJournal")
                {
                    while($true) {
                        if ( (Remove-Item "C:\\Bin\\IIS\\PTJournal" -Recurse *>&1) -ne $null)
                        {  
                            write-output "Removing failed we should wait"
                        }
                        else 
                        {
                            break 
                        } 
                    }
                  }
              ''')

            }
        }
		
        stage('Copy web page to the IIS Bin directory') {
            steps {         
                bat('xcopy "src\\Server\\ProductivityTools.Journal.WebApi\\bin\\Release\\net6.0\\publish" "C:\\Bin\\IIS\\PTJournal\\" /O /X /E /H /K')
            }
        }

        stage('Start website on IIS') {
            steps {
                bat('%windir%\\system32\\inetsrv\\appcmd start site /site.name:PTJournal')
            }
        }

       stage('Create Login PTExpenses on SQL2022') {
             steps {
                 bat('sqlcmd -S ".\\SQL2022" -q "CREATE LOGIN [IIS APPPOOL\\PTJorunal] FROM WINDOWS WITH DEFAULT_DATABASE=[PTJournal];"')
             }
        }

        stage('Create User PTJournal on SQL2022') {
             steps {
                 bat('sqlcmd -S ".\\SQL2022" -q " USE PTJournal;  CREATE USER [IIS APPPOOL\\PTJournal]  FOR LOGIN [IIS APPPOOL\\PTJournal];"')
             }
        }

        stage('Give DBOwner permissions on SQL2022') {
             steps {
                 bat('sqlcmd -S ".\\SQL2022" -q "USE PTJournal;  ALTER ROLE [db_owner] ADD MEMBER [IIS APPPOOL\\PTJournal];"')
             }
        }
        stage('Bye bye') {
            steps {
                // Get some code from a GitHub repository
                echo 'Bye bye'
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
