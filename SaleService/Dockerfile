# Vedere https://aka.ms/customizecontainer per informazioni su come personalizzare il contenitore di debug e su come Visual Studio usa questo Dockerfile per compilare le immagini per un debug più rapido.

# A seconda del sistema operativo dei computer host che compilano o eseguono i contenitori, potrebbe essere necessario modificare l'immagine specificata nell'istruzione FROM.
# Per ulteriori informazioni, vedere https://aka.ms/containercompat

# Questa fase viene usata durante l'esecuzione da Visual Studio in modalità rapida (impostazione predefinita per la configurazione di debug)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Questa fase viene usata per compilare il progetto di servizio
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SaleService/SaleService.csproj", "SaleService/"]
RUN dotnet restore "./SaleService/SaleService.csproj"
COPY . .
WORKDIR "/src/SaleService"
RUN dotnet build "./SaleService.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# Questa fase viene usata per pubblicare il progetto di servizio da copiare nella fase finale
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SaleService.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# Questa fase viene usata nell'ambiente di produzione o durante l'esecuzione da Visual Studio in modalità normale (impostazione predefinita quando non si usa la configurazione di debug)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SaleService.dll"]