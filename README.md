# MyFITJob - TD MONGODB

## Environnement de travail

- Docker et Docker Compose
- Visual Studio Code avec extensions pour C# (dans un .devcontainer)
- .NET SDK

## Objectifs du TD

- Intégrer le code pour utiliser une base de données CosmosDB au lieu d'un fichier plat JSON 
- Ajouter la dépendance à Cosmos dans notre projet via `docker-compose`

## Steps 

1. **Création de notre couche Entité**

- Installer la dépendance NuGet pour gérer CosmosDB: `dotnet add package MongoDB.Driver`
- Modifier le `ContactRepository` actuellement basé sur du JSON pour qu'il utilise MongoDB 

API:

```csharp
public class ContactRepository
{
    private const string collectionName = "contacts";
    private readonly IMongoCollection<Contact> _contacts;
    private readonly FilterDefinitionBuilder<Contact> _filterBuilder = Builders<Contact>.Filter;

    public ContactRepository(string connectionString)
    {
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("Contacts");
        _contacts = database.GetCollection<Contact>(collectionName);
    }

    public async Task<List<Contact>> GetAllAsync()
    {
        return await _contacts.Find(_filterBuilder.Empty)
            .ToListAsync();
    }

    public Task<Contact> GetAsync(Guid id)
    {
        FilterDefinition<Contact> filter = _filterBuilder.Eq(entity => entity.Id, id);
        return _contacts.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<Contact> CreateAsync(Contact contact)
    {
        if (contact is null)
            throw new ArgumentNullException(nameof(contact));

        await _contacts.InsertOneAsync(contact);

        return contact;
    }

    public async Task<Contact> Update(Guid id, Contact updatingEntity)
    {
        if (updatingEntity is null)
            throw new ArgumentNullException(nameof(updatingEntity));

        FilterDefinition<Contact> filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, id);
        await _contacts.ReplaceOneAsync(filter, updatingEntity);

        return updatingEntity;
    }

    public async Task DeleteAsync(Guid id)
    {
        FilterDefinition<Contact> filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, id);
        await _contacts.DeleteOneAsync(filter);
    }
}
```
- Installer l'extension VSCode [MongoDB](https://marketplace.visualstudio.com/items/?itemName=mongodb.mongodb-vscode)
- Mettre à jour le Controller pour qu'il utilise les nouvelles méthodes async/await 
- Lancer l'application  
- Lancer l'image Docker : `docker run -d --rm --name contact-db -p 27017:27017 -v mongodbdata:/data/db mongo`