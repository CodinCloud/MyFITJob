# MyFITJob - TD API Web

## Environnement de travail

- Docker et Docker Compose
- Visual Studio Code avec extensions pour C# (dans un .devcontainer)
- .NET SDK

## Prérequis

1. **Installer Docker**
   - Télécharger et installer [Docker Desktop](https://www.docker.com/products/docker-desktop)
   - Vérifier l'installation avec la commande `docker --version`

2. **Installer .NET SDK**
   - Télécharger et installer [.NET SDK](https://dotnet.microsoft.com/download)
   - Vérifier l'installation avec la commande `dotnet --version`

3. **Installer Visual Studio Code**
   - Télécharger et installer [Visual Studio Code](https://code.visualstudio.com/)
   - Installer les extensions recommandées pour C#:
     - C# Dev Kit
     - .NET Runtime Install Tool
     - Docker

## Objectifs du TD

- Créer une API Web simple avec .NET pour gérer une liste de contacts : `contacts.json`
- implémenter une api REST (GET, POST, PUT, DELETE) 
- Tester les APIs avec des fichiers .http et une UI OpenAPI
- Utiliser Docker pour conteneuriser l'application

## Steps 

1. **Création du projet API**
   - Utiliser la commande `dotnet new webapi` pour créer un nouveau projet API
   - Explorer la structure générée du projet
   - Lancer le projet `dotnet run` 
   - Créer un fichier `.http` (pris en charge par l'extension [REST Client](https://marketplace.visualstudio.com/items/?itemName=humao.rest-client))
   - Créer un appel GET ALL pour lire la base de contacts: 
   - Ajouter les instructions de build pour que le fichier JSON soit ajouté dans le livrable généré par .Net : 
   
   *Dans le fichier **.csproj** de votre projet*:
   ```xml 
   <ItemGroup>
    <None Update="contacts.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
   </ItemGroup>
   ``` 

   - Ajouter la logique de lecture/chargement du fichier JSON qui simulera notre BDD
   ``` cs 
      using System.Text.Json;

      // Ajouter le fichier  
      string contactsJsonPath = Path.Combine(AppContext.BaseDirectory, "./contacts.json");
      if (File.Exists(contactsJsonPath))
      {
         string json = File.ReadAllText(contactsJsonPath);
         _contacts = JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
      }
      else
      {
         Console.WriteLine($"Le fichier contacts.json n'a pas été trouvé à l'emplacement: {contactsJsonPath}");
      }
   ```

   > **Checkpoint**

   - Installer un package d'UI pour l'OpenAPI : `dotnet add package Scalar.AspNetCore` 
   - `app.MapScalarApiReference();`
   - Naviguer vers <votre-api>/scalar/v1

5. **Conteneuriser l'application**
   - Créer un *Dockerfile* dans ce projet 
   ```dockerfile
      FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
      WORKDIR /app
      EXPOSE 80
      EXPOSE 443

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
   ```

   - Configurer Docker Compose
   - Exécuter l'application dans un conteneur

## Commandes Docker

Pour construire et démarrer le conteneur Docker :

**Important** : Exécutez ces commandes depuis la racine du projet 
```bash

# Construire l'image Docker
docker build -t contacts-api -f src/Contacts.Api/Dockerfile .

# Démarrer le conteneur
docker run -d -p 80:8080 -e ASPNETCORE_ENVIRONMENT=Development --name contacts-api-container contacts-api
```

Pour arrêter et supprimer le conteneur :

```bash
# Arrêter le conteneur
docker stop contacts-api-container

# Supprimer le conteneur
docker rm contacts-api-container
```

## Commandes docker-compose 

Pour monter un environnement Docker via docker-compose : 

```bash
docker-compose up -d
```

Supprimer tous les artefacts Docker :
```bash
docker-compose down -v --rmi all --remove-orphans
```

## Ressources

- [Documentation .NET](https://docs.microsoft.com/fr-fr/dotnet/)
- [Documentation ASP.NET Core](https://docs.microsoft.com/fr-fr/aspnet/core/)
- [Documentation Docker](https://docs.docker.com/)
- [Guide des verbes HTTP](https://developer.mozilla.org/fr/docs/Web/HTTP/Methods)
