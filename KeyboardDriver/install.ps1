$taskName = "Custom Keyboard Utility Driver"
$taskFolder = "Z:\ProductivityTools\KeyboardDriver\"
$buildFolder = "~/source\repos\KeyboardDriver\KeyboardDriver\bin\Debug\net6.0-windows10.0.19041.0/"

Stop-ScheduledTask -TaskName $taskName

sleep 1

Remove-Item $(Join-Path $taskFolder "*") -Recurse

Copy-Item -Path $(Join-Path $buildFolder "*") -Destination $taskFolder -Recurse

Start-ScheduledTask -TaskName $taskName
