// Script d'initialisation MongoDB pour MyFITJob Contacts
// Ce script est exécuté automatiquement au démarrage du container MongoDB

print('🚀 Initialisation de la base de données contactsdb...');

// Créer la base de données contactsdb
db = db.getSiblingDB('contactsdb');

// Créer la collection companies avec des index
db.createCollection('companies');

// Index unique sur le nom de l'entreprise
db.companies.createIndex({ "name": 1 }, { unique: true });

// Index sur l'industrie pour les requêtes fréquentes
db.companies.createIndex({ "industry": 1 });

print('✅ Base de données contactsdb initialisée avec succès !');
print('🔍 Collections disponibles:', db.getCollectionNames());