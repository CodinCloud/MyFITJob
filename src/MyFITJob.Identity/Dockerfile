# Multi-stage build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["MyFITJob.Identity.csproj", "./"]
RUN dotnet restore "MyFITJob.Identity.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "MyFITJob.Identity.csproj" -c Release -o /app/build

# Development stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
WORKDIR /src
COPY ["MyFITJob.Identity.csproj", "./"]
RUN dotnet restore "MyFITJob.Identity.csproj"
COPY . .
EXPOSE 80
EXPOSE 443
CMD ["dotnet", "run", "--urls", "http://0.0.0.0:80"]

# Publish
FROM build AS publish
RUN dotnet publish "MyFITJob.Identity.csproj" -c Release -o /app/publish

# Production image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "MyFITJob.Identity.dll"] 