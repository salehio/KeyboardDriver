. $(Join-Path $PSScriptRoot common.ps1)

while($true){
	Write-Host "Launching..."
	. $exePath | Out-Null
	Write-Host "Execution ended."
}
