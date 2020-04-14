FROM ubuntu:bionic

#Install dotnet core sdk 3.1 & Chromium
RUN apt-get update; \ 
	apt-get install -y  chromium-browser; \
	apt-get install apt-transport-https; \
	apt-get install -y wget; \
	apt-get install -y gpg; \
	apt-get install -y git; \
	wget https://packages.microsoft.com/config/ubuntu/19.10/packages-microsoft-prod.deb O- packages-microsoft-prod.deb; \
	dpkg -i packages-microsoft-prod.deb; \
	wget O- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o microsoft.asc.gpg; \
	mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/; \
	wget https://packages.microsoft.com/config/ubuntu/19.04/prod.list; \
	mv prod.list /etc/apt/sources.list.d/microsoft-prod.list; \
	chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg; \
	chown root:root /etc/apt/sources.list.d/microsoft-prod.list; \
	apt-get install -y apt-transport-https; \
	apt-get update -y; \
	apt-get install -y dotnet-sdk-3.1

#Install project and use dotnet utils to set up it properly
RUN git clone https://github.com/PashmiDev/Cross-platformMicroservice.git /home

#Get configuration (DLL & Config file
COPY InteractiveTerminalCrossPlatformMicroservice/PeripheralLibraries/ /home/InteractiveTerminalCrossPlatformMicroservice/PeripheralLibraries/
COPY InteractiveTerminalCrossPlatformMicroservice/Config.xml /home/InteractiveTerminalCrossPlatformMicroservice/

#Restore and build the project
RUN dotnet restore /home; \
	dotnet build /home

#CMD dotnet run --project /home/InteractiveTerminalCrossPlatformMicroservice/InteractiveTerminalCrossPlatformMicroservice.csproj
CMD bash /home/Scripts/Linux/dockerSetup.sh

