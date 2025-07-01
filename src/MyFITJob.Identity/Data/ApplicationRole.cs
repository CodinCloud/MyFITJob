using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace MyFITJob.Identity.Data;

[CollectionName("Roles")]
public class ApplicationRole : MongoIdentityRole<Guid>
{
}