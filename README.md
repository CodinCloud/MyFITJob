# MyFITJob - Cours 2: Contextualize

## Environnement de travail

- Docker et Docker Compose
- Visual Studio Code avec extensions pour C# 
- .NET SDK

## Objectifs du TD

- Définir les layers d'une application N-Tiers
- Mesurer les limites d'un Monolithe avec K6

## Getting Started 

1. Lancer l'environnement : `docker-compose -up -d --build`
2. Installer l'extension [PostgreSQL](https://marketplace.visualstudio.com/items/?itemName=cweijan.vscode-postgresql-client2)
3. Se connecter sur l'adresse : http://localhost:3000

## ORM - Commandes EF pour modifier le modèle de données :  

1. Lister les migrations 
`dotnet ef migrations list --project MyFITJob.DAL --startup-project MyFITJob.Api`

2. Créer une migration 
`dotnet ef migrations list --project MyFITJob.DAL --startup-project MyFITJob.Api`

3. Supprimer une migration

```
dotnet ef database update NomDeLaMigrationPrécédente --project MyFITJob.DAL --startup-project MyFITJob.Api
dotnet ef migrations remove --project MyFITJob.DAL --startup-project MyFITJob.Api
```

## Steps 

1. Installez K6: `choco install k6` ou `winget install k6 --source winget`
2. Nettoyer bien l'environnement précédent: 
`docker-compose down -v --rmi all --remove-orphans`
3. Monter l'infrastructure avec le docker-compose de montée en charge : 
`docker-compose -f .\compose.workload.yml up -d --build`  
4. Lancer les scripts de montée en charge : `k6 ./tests/workload.e2e.js` puis `./tests/e2e.js` 




