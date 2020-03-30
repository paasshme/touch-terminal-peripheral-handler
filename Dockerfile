FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
COPY ["InteractiveTerminalCrossPlatformMicroservice/Config.xml","/app"]
COPY ["InteractiveTerminalCrossPlatformMicroservice/PeripheralLibraries/TestDevices.dll","/PeripheralLibraries/"]
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["InteractiveTerminalCrossPlatformMicroservice/InteractiveTerminalCrossPlatformMicroservice.csproj", "InteractiveTerminalCrossPlatformMicroservice/"]
COPY ["PeripheralTools/PeripheralTools.csproj", "PeripheralTools/"]
RUN dotnet restore "InteractiveTerminalCrossPlatformMicroservice/InteractiveTerminalCrossPlatformMicroservice.csproj"
COPY . .
WORKDIR "/src/InteractiveTerminalCrossPlatformMicroservice"
RUN dotnet build "InteractiveTerminalCrossPlatformMicroservice.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "InteractiveTerminalCrossPlatformMicroservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InteractiveTerminalCrossPlatformMicroservice.dll"]
