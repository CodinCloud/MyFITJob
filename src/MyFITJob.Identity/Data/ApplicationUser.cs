using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace MyFITJob.Identity.Data;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<Guid>
{

} 