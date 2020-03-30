#!/bin/sh

#Launch script without docker for Linux
#Warning: the browser starts a few second before the project is totallly launched

#Prevent issues due to multi-user access on .NET NuGet ressources
rm -f /tmp/NuGetScratch/lock/*

path='./../..'
dotnet restore $path 
dotnet build $path 
cp -f "$path/TestDevices/bin/Debug/netcoreapp3.1/TestDevices.dll" "$path/InteractiveTerminalCrossPlatformMicroservice/PeripheralLibraries"

#Chromium require the "--no-sandbox" option if the script is launched as superuser
if [[ $EUID > 0 ]]; then    
    chromium --kiosk https://localhost:5001/swagger/index.html  --noerrdialogs --disable-infobars --no-default-browser-check --no-experiments --no-pings --silent-debugger-extension-api &
else
    chromium --kiosk https://localhost:5001/swagger/index.html --no-sandbox --noerrdialogs --disable-infobars --no-default-browser-check --no-experiments --no-pings --silent-debugger-extension-api &
fi
dotnet run --project "$path/InteractiveTerminalCrossPlatformMicroservice"