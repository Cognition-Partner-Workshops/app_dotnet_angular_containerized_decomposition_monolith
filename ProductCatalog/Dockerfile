FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ProductCatalog.Core/ProductCatalog.Core.csproj ProductCatalog.Core/
COPY ProductCatalog.Api/ProductCatalog.Api.csproj ProductCatalog.Api/
RUN dotnet restore ProductCatalog.Api/ProductCatalog.Api.csproj

COPY . .
WORKDIR /src/ProductCatalog.Api
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductCatalog.Api.dll"]
