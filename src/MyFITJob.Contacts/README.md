# MyFITJob.Contacts

Microservice de gestion des contacts et informations d'entreprises pour MyFITJob.

## Installation

```bash
cd src/MyFITJob.Contacts
npm install
```

## Démarrage

### Mode développement (avec rechargement automatique)
```bash
npm run dev
```

### Mode production
```bash
npm start
```

Le service démarre sur le port **3002** par défaut.

## API Endpoints

### Health Check
```
GET /health
```

### Informations d'entreprise
```
GET /api/companies/:id
```

**Réponse :**
```json
{
  "industry": "Tech",
  "size": "51-200",
  "rating": 4.2
}
```

## Exemples d'utilisation

### Test avec curl
```bash
# Health check
curl http://localhost:3002/health

# Informations d'entreprise (ID 1)
curl http://localhost:3002/api/companies/1

# Informations d'entreprise (ID 5)
curl http://localhost:3002/api/companies/5
```

### Test avec le frontend
Le frontend peut maintenant récupérer les informations d'entreprise via :
```javascript
fetch('http://localhost:3002/api/companies/1')
  .then(response => response.json())
  .then(companyInfo => {
    console.log(companyInfo);
    // { industry: "Tech", size: "51-200", rating: 4.2 }
  });
```

## Données disponibles

Le service contient 10 entreprises fictives avec des IDs de 1 à 10. Les informations retournées (industry, size, rating) sont générées aléatoirement à chaque requête pour simuler la variabilité des données. 