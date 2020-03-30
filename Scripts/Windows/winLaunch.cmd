@echo off
set projectPath="..\.."
dotnet restore "%projectPath%\Projet_IUT_IPM.sln"
dotnet build "%projectPath%\Projet_IUT_IPM.sln"
copy /Y "%projectPath%\TestDevices\bin\Debug\netcoreapp3.1\TestDevices.dll" "%projectPath%\InteractiveTerminalCrossPlatformMicroservice\PeripheralLibraries"
start chrome --kiosk https://localhost:5001/swagger --noerrdialogs --disable-infobars --no-default-browser-check --no-experiments --no-pings --silent-debugger-extension-api

::With the terminal up
::"%~dp0%projectPath:"=%\InteractiveTerminalCrossPlatformMicroservice\bin\Debug\netcoreapp3.1\InteractiveTerminalCrossPlatformMicroservice.exe"

:: Minimize the terminal (Deployment purpose)
"%~dp0%projectPath:"=%\InteractiveTerminalCrossPlatformMicroservice\bin\Debug\netcoreapp3.1\InteractiveTerminalCrossPlatformMicroservice.exe"
