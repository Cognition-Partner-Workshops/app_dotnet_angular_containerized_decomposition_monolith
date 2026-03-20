FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["ShopService.API/ShopService.API.csproj", "ShopService.API/"]
COPY ["ShopService.Core/ShopService.Core.csproj", "ShopService.Core/"]
RUN dotnet restore "ShopService.API/ShopService.API.csproj"
COPY . .
WORKDIR "/src/ShopService.API"
RUN dotnet build "ShopService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShopService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopService.API.dll"]
