FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5225
EXPOSE 7085

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["QuickApp.Server/QuickApp.Server.csproj", "QuickApp.Server/"]
COPY ["QuickApp.Core/QuickApp.Core.csproj", "QuickApp.Core/"]
COPY ["quickapp.client/quickapp.client.esproj", "quickapp.client/"]
RUN dotnet restore "QuickApp.Server/QuickApp.Server.csproj"
COPY QuickApp.Core/ QuickApp.Core/
COPY QuickApp.Server/ QuickApp.Server/
COPY quickapp.client/ quickapp.client/
WORKDIR "/src/QuickApp.Server"
RUN dotnet build "QuickApp.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuickApp.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuickApp.Server.dll"]
