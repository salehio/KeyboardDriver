. $(Join-Path $PSScriptRoot common.ps1)

Stop-ScheduledTask -TaskName $taskName

sleep 3

Remove-Item $(Join-Path $taskFolder "*") -Recurse

Copy-Item -Path $(Join-Path $buildFolder "*") -Destination $taskFolder -Recurse

Start-ScheduledTask -TaskName $taskName
