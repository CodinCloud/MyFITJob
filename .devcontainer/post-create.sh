#!/bin/bash

echo "🚀 Configuration post-création du DevContainer..."

# Ensure we're in the workspace directory
cd /workspace

# Trust the HTTPS development certificate
echo "📜 Configuration des certificats de développement..."
dotnet dev-certs https --trust

# Restore any existing .NET projects
echo "📦 Restauration des packages NuGet..."
if [ -f "*.sln" ]; then
    dotnet restore
elif find . -name "*.csproj" -type f | head -1 | grep -q .; then
    find . -name "*.csproj" -exec dotnet restore {} \;
fi

# Wait for MongoDB to be ready
echo "🍃 Attente de MongoDB..."
until mongosh --host localhost:27017 --eval "print('MongoDB is ready')" > /dev/null 2>&1; do
    echo "En attente de MongoDB..."
    sleep 2
done

echo "✅ MongoDB est prêt!"

# Create initial database and collections if needed
echo "🗄️ Initialisation de la base de données..."
mongosh --host localhost:27017 --eval "
use myfitjob;
if (!db.contacts.findOne()) {
    db.contacts.insertOne({
        id: '1',
        firstName: 'John',
        lastName: 'Doe',
        email: 'john.doe@example.com',
        phone: '+33123456789',
        createdAt: new Date()
    });
    print('Collection contacts initialisée avec des données de test');
}
"

# Set up git configuration if not already set
if [ -z "$(git config --global user.name)" ]; then
    echo "⚙️ Configuration Git recommandée..."
    echo "Pensez à configurer Git avec :"
    echo "  git config --global user.name 'Votre Nom'"
    echo "  git config --global user.email 'votre.email@example.com'"
fi

echo "🎉 Configuration terminée! Votre environnement de développement est prêt."
echo ""
echo "📋 Commandes utiles :"
echo "  - dotnet run                    : Démarrer l'API"
echo "  - dotnet watch                  : Démarrer avec rechargement automatique"
echo "  - mongosh                      : Se connecter à MongoDB"
echo "  - docker ps                    : Voir les conteneurs en cours"
echo ""
echo "🌐 Ports exposés :"
echo "  - 5000/5001 : API .NET"
echo "  - 27017     : MongoDB" 