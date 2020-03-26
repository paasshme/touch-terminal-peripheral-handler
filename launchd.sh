#!/bin/bash

#Todo: test if docker is installed


re='^[0-9]+$'
a=$(sudo docker ps | grep ProjetS3.dll)
port=5001

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


dotnet restore .
dotnet build .
cp -f TestDevices/bin/Debug/netcoreapp3.1/TestDevices.dll ProjetS3/PeripheralLibraries

if [ -n "$a" ]; then
    echo "[STATUS] The project is already running"
    docker stop $(docker ps | grep ProjetS3.dll | cut -d ' ' -f 1) 
    echo "[STATUS] Old project stopped !"
fi

echo "Building docker image..."
docker build -t test:projets3 .


echo "[STATUS] Docker image ready"
echo "[STATUS] Launching project..."
docker run -it --privileged -device=/dev/ttyACM1 -p $port:80 test:projets3

echo "[STATUS] Project successfully launch"
echo "[STATUS] http://localhost:$port/swagger/index.html"

chromium --kiosk http://localhost:$port/swagger/index.html --sandbox
