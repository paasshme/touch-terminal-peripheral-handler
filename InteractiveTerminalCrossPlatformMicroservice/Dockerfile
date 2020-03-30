#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["ProjetS3/ProjetS3.csproj", "ProjetS3/"]
COPY ["PeripheralTools/PeripheralTools.csproj", "PeripheralTools/"]
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