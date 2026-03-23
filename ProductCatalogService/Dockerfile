FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ProductCatalogService/ProductCatalogService.csproj", "ProductCatalogService/"]
RUN dotnet restore "ProductCatalogService/ProductCatalogService.csproj"
COPY ProductCatalogService/ ProductCatalogService/
WORKDIR "/src/ProductCatalogService"
RUN dotnet build "ProductCatalogService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductCatalogService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductCatalogService.dll"]
