FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
# USER $APP_UID   # <-- Optionnel, à commenter en dev si besoin
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyFITJob.Api/MyFITJob.Api.csproj", "MyFITJob.Api/"]
RUN dotnet restore "MyFITJob.Api/MyFITJob.Api.csproj"
COPY . .
WORKDIR "/src/MyFITJob.Api"
RUN dotnet build "MyFITJob.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyFITJob.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS prod
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFITJob.Api.dll"]

# --- STAGE DEV ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
WORKDIR /src
COPY ["MyFITJob.Api/MyFITJob.Api.csproj", "MyFITJob.Api/"]
RUN dotnet restore "MyFITJob.Api/MyFITJob.Api.csproj"
COPY . .
WORKDIR "/src/MyFITJob.Api"
ENTRYPOINT ["dotnet", "watch", "run", "--project", "./MyFITJob.Api/MyFITJob.Api.csproj", "--urls=http://0.0.0.0:80"] 