using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Contacts.Api;

public class ContactRepository
{
    private const string collectionName = "contacts";
    private readonly IMongoCollection<Contact> _contacts;
    private readonly FilterDefinitionBuilder<Contact> _filterBuilder = Builders<Contact>.Filter;

    public ContactRepository(string connectionString)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

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

    public async Task<Contact> UpdateAsync(Guid id, Contact updatingEntity)
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