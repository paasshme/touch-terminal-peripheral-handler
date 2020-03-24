FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
COPY ["ProjetS3/Config.xml","/app"]
COPY ["ProjetS3/PeripheralLibraries/TestDevices.dll","/app/PeripheralLibraries/"]
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ProjetS3/ProjetS3.csproj", "ProjetS3/"]
COPY ["IDeviceLib/IDeviceLib.csproj", "IDeviceLib/"]
RUN dotnet restore "ProjetS3/ProjetS3.csproj"
COPY . .
WORKDIR "/src/ProjetS3"
RUN dotnet build "ProjetS3.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "ProjetS3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjetS3.dll"]
