$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition-Parent
Start-Process chrome -ArgumentList "--kiosk https://localhost:5001/swagger --noerrdialogs --disable-infobars --no-default-browser-check --no-experiments --no-pings --silent-debugger-extension-api"                                           
Start-Process -NoNewWindow $scriptDir\ProjetS3\bin\Debug\netcoreapp3.1\ProjetS3.exe 

