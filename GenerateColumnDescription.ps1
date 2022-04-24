clear
cd $PSScriptRoot
#Import-Module .\ProductivityTools.SQLServerColumnDescription.psm1 -Force 

#Install-Module sqlserver

#Get-ColumnsDescription -Verbose -ServerInstance ".\SQL2019" -Database PTMeetings
Get-ColumnsDescription -Verbose -ServerInstance ".\SQL2019" -Database PTMeetings -OutMd
move-Item ColumnDescription.MD docs