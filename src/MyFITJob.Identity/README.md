# MyFITJob.Identity - Serveur d'Authentification JWT

## 🚀 Démarrage rapide

### 1. Démarrer MongoDB
```bash
# Depuis la racine du projet MyFITJob
docker compose up mongodb -d
```

### 2. Démarrer l'API Identity

#### Option A (conseillé): Avec Docker Compose 
```bash
# Depuis la racine du projet
docker compose up identity -d
```
L'API sera accessible sur : `http://localhost:5001`

#### Option B : Développement local
```bash
cd src/MyFITJob.Identity
dotnet run
```
L'API sera accessible sur : `http://localhost:8080`

### 3. Tester l'authentification

Utilisez le fichier `test-identity.http` pour tester l'authentification JWT.

**Identifiants par défaut :**
- **Username** : `admin`
- **Password** : `admin123`

## 📋 Endpoints disponibles

- `POST /api/auth/login` - Connexion utilisateur
- `POST /api/auth/register` - Création de compte
- `GET /api/auth/me` - Informations utilisateur (protégé)
- `POST /api/auth/refresh` - Rafraîchissement de token

## 🔧 Configuration

Les paramètres JWT sont dans `appsettings.json` :
- **SecretKey** : Clé de signature
- **Issuer** : MyFITJob.Identity
- **Audience** : MyFITJob.API
- **Expiration** : 60 minutes

## 🎓 Objectif pédagogique

Ce service illustre l'authentification JWT dans une architecture microservices avec :
- ASP.NET Core Identity avec MongoDB
- Génération et validation de tokens JWT
- Règles de mot de passe simplifiées pour l'apprentissage
- Claims JWT modernes et standards 