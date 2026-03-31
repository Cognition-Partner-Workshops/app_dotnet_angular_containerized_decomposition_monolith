FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5002
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["ProductService.Server/ProductService.Server.csproj", "ProductService.Server/"]
COPY ["ProductService.Core/ProductService.Core.csproj", "ProductService.Core/"]
RUN dotnet restore "ProductService.Server/ProductService.Server.csproj"
COPY ProductService.Core/ ProductService.Core/
COPY ProductService.Server/ ProductService.Server/
WORKDIR "/src/ProductService.Server"
RUN dotnet build "ProductService.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductService.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductService.Server.dll"]
