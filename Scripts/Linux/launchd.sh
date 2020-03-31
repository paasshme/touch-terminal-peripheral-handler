#!/bin/sh

#This script get every dependencies, build and run the docker image for linux
#And then launch chromium
#Warning: the browser starts a few second before the project is totallly launched
re='^[0-9]+$'
a=$(sudo docker ps | grep InteractiveTerminalCrossPlatformMicroservice.dll)
port=5001
path='../..'

if [ $# -gt 1 ]; then
    echo "[ERROR] Too much argument supplied:"
    echo "Use 'launchd.sh' or 'launchd.sh [port]'"
    exit 1
fi

if [ $# -eq 1 ]; then

    if ! [[ $1 =~ $re ]] ; then
	echo "[ERROR] You need to provide a correct port or use the default one " ; exit 1
    elif [ $1 -gt 65535 ] || [ $1 -lt 1 ]; then
	echo "[WARNING] Wrong port number: the $port will be used"
    else
	port=$1
    fi
fi
dotnet restore $path 
dotnet build $path 
cp -f "$path/TestDevices/bin/Debug/netcoreapp3.1/TestDevices.dll" "$path/InteractiveTerminalCrossPlatformMicroservice/PeripheralLibraries"

if [ -n "$a" ]; then
    echo "[STATUS] The project is already running"
    docker stop $(docker ps | grep InteractiveTerminalCrossPlatformMicroservice.dll 
	| cut -d ' ' -f 1) 
    echo "[STATUS] Old project stopped !"
fi

echo "Building docker image..."
docker build -t test:projets3 $path


echo "[STATUS] Docker image ready"
echo "[STATUS] Launching project..."
docker run -it --privileged -device=/dev/ttyACM1 -p $port:80 test:projets3

echo "[STATUS] Project successfully launch"
echo "[STATUS] http://localhost:$port/swagger/index.html"

chromium --kiosk http://localhost:$port/swagger/index.html --no-sandbox --noerrdialogs --disable-infobars --no-default-browser-check --no-experiments --no-pings --silent-debugger-extension-api