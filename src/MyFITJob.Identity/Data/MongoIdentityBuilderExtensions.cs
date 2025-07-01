using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace MyFITJob.Identity.Data;

public static class MongoIdentityBuilderExtensions
{
    public static IdentityBuilder AddMongoDbStores(this IdentityBuilder builder, string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        builder.Services.AddSingleton<IMongoDatabase>(database);
        
        return builder;
    }
} 