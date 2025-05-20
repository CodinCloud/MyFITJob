using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

public class Contact(Guid Id, string FirstName, string LastName, string Email, string Phone, string Address)
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Id;

    public string FirstName { get; set; } = FirstName;
    public string LastName { get; set; } = LastName;
    public string Email { get;set; } = Email;
    public string Phone { get;set; } = Phone;
    public string Address { get; set; } = Address;
}

