@echo off
set projectPath="..\.."
dotnet restore "%projectPath%\ProjetS3.sln"
dotnet build "%projectPath%\ProjetS3.sln"
copy /Y "%projectPath%\TestDevices\bin\Debug\netcoreapp3.1\TestDevices.dll" "%projectPath%\ProjetS3\PeripheralLibraries"
start chrome --kiosk https://localhost:5001/swagger --noerrdialogs --disable-infobars --no-default-browser-check --no-experiments --no-pings --silent-debugger-extension-api

::With the terminal up
::start %~dp0\%projectPath%\ProjetS3\bin\Debug\netcoreapp3.1\ProjetS3.exe

:: Minimize the terminal (Deployment purpose)
start /min "%~dp0\%projectPath%\ProjetS3\bin\Debug\netcoreapp3.1\ProjetS3.exe"
