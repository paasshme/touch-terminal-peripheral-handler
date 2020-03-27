#Add the startup file in the windows correct location
$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition-Parent
Set-Content -Path '.\path.txt' -Value $scriptDir
Copy-Item -Path '.\path.txt' -Destination "$env:APPDATA\Microsoft\Windows\Start Menu\Programs\Startup" -Force
Copy-Item -Path ..\startup.cmd -Destination "$env:APPDATA\Microsoft\Windows\Start Menu\Programs\Startup" -Force