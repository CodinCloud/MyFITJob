# DevContainer Configuration pour MyFITJob

Ce dossier contient la configuration DevContainer pour le projet MyFITJob, optimisée pour le développement .NET 9 avec MongoDB.

## 🚀 Démarrage rapide

1. **Prérequis**
   - Visual Studio Code
   - Extension "Dev Containers" installée
   - Docker Desktop en cours d'exécution

2. **Lancement**
   - Ouvrir le projet dans VS Code
   - Appuyer sur `F1` et taper "Dev Containers: Reopen in Container"
   - Ou cliquer sur la notification qui apparaît

## 📁 Structure des fichiers

- `devcontainer.json` : Configuration principale du DevContainer
- `docker-compose.yml` : Services Docker (app + MongoDB)
- `Dockerfile` : Image personnalisée pour le développement
- `post-create.sh` : Script d'initialisation post-création
- `init-mongo.js` : Script d'initialisation MongoDB

## 🛠️ Fonctionnalités incluses

### Extensions VS Code
- **C# Dev Kit** : Développement .NET complet
- **Docker** : Gestion des conteneurs
- **REST Client** : Test des APIs
- **MongoDB for VS Code** : Interface MongoDB
- **GitLens** : Outils Git avancés

### Outils de développement
- .NET SDK 9.0 (+ 8.0 en fallback)
- Entity Framework Core CLI
- ASP.NET Core Code Generator
- MongoDB Shell (mongosh)
- MongoDB Database Tools
- Docker-in-Docker
- GitHub CLI

### Services
- **Application .NET** : Port 5000/5001
- **MongoDB** : Port 27017
  - Base de données : `myfitjob`
  - Utilisateur : `myfitjob_user` / `myfitjob_password`
  - Données de test pré-chargées

## 🔧 Configuration automatique

Le script `post-create.sh` configure automatiquement :
- Certificats HTTPS de développement
- Restauration des packages NuGet
- Initialisation de MongoDB
- Données de test dans la collection `contacts`

## 📊 Base de données MongoDB

### Collection `contacts`
```javascript
{
  firstName: String (requis),
  lastName: String (requis),
  email: String (requis, unique),
  phone: String,
  createdAt: Date,
  updatedAt: Date
}
```

### Index créés
- `email` (unique)
- `lastName + firstName`
- `createdAt` (décroissant)

## 🌐 Accès aux services

Une fois le DevContainer démarré :
- **API .NET** : http://localhost:5000 ou https://localhost:5001
- **MongoDB** : mongodb://localhost:27017
- **Scalar API Docs** : http://localhost:5000/scalar/v1

## 🔍 Commandes utiles

```bash
# Démarrer l'API
dotnet run

# Démarrer avec rechargement automatique
dotnet watch

# Se connecter à MongoDB
mongosh

# Voir les conteneurs
docker ps

# Restaurer les packages
dotnet restore

# Créer une nouvelle migration EF
dotnet ef migrations add NomMigration

# Appliquer les migrations
dotnet ef database update
```

## 🐛 Dépannage

### MongoDB ne démarre pas
```bash
# Vérifier les logs
docker logs devcontainer-db-1

# Redémarrer MongoDB
docker restart devcontainer-db-1
```

### Problèmes de certificats HTTPS
```bash
# Régénérer les certificats
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### Problèmes de permissions
```bash
# Vérifier les permissions du script
ls -la .devcontainer/post-create.sh

# Rendre exécutable si nécessaire
chmod +x .devcontainer/post-create.sh
```

## 📝 Personnalisation

Pour modifier la configuration :
1. Éditer `devcontainer.json` pour les extensions et paramètres VS Code
2. Modifier `docker-compose.yml` pour les services
3. Adapter `Dockerfile` pour les outils système
4. Personnaliser `init-mongo.js` pour les données initiales

## 🔄 Mise à jour

Pour mettre à jour le DevContainer :
1. Modifier les fichiers de configuration
2. Reconstruire le conteneur : `F1` → "Dev Containers: Rebuild Container" 