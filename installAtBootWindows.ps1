#Add the startup file in the windows correct location
Copy-Item -Path startup.cmd -Destination "$env:APPDATA\Microsoft\Windows\Start Menu\Programs\Startup" 