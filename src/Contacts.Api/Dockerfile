FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
# Note: Les ports exposés sont indicatifs. L'application .NET déterminera le port réel
# en fonction de la configuration (Program.cs, appsettings.json, ou variables d'environnement)
EXPOSE 80
EXPOSE 443
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Contacts.Api/Contacts.Api.csproj", "Contacts.Api/"]
RUN dotnet restore "Contacts.Api/Contacts.Api.csproj"
COPY ["src/Contacts.Api", "Contacts.Api/"]
WORKDIR "/src/Contacts.Api"
RUN dotnet build "Contacts.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Contacts.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Contacts.Api.dll"]
