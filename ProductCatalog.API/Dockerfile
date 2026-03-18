FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["ProductCatalog.API/ProductCatalog.API.csproj", "ProductCatalog.API/"]
RUN dotnet restore "ProductCatalog.API/ProductCatalog.API.csproj"
COPY ProductCatalog.API/ ProductCatalog.API/
WORKDIR "/src/ProductCatalog.API"
RUN dotnet build "ProductCatalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductCatalog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductCatalog.API.dll"]
