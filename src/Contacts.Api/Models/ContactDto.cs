using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.Models;

public record ContactDto
{
    public ContactDto(Guid id, string firstName, string lastName, string email, string phone, string address)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Address = address;
    }

    public Guid Id { get; set; }

    [Required(ErrorMessage = "Le prénom est obligatoire")]
    [StringLength(50, ErrorMessage = "Le prénom ne peut pas dépasser 50 caractères")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le numéro de téléphone est obligatoire")]
    [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'adresse est obligatoire")]
    [StringLength(200, ErrorMessage = "L'adresse ne peut pas dépasser 200 caractères")]
    public string Address { get; set; } = string.Empty;
}

public record CreateContactDto(string FirstName, string LastName, string Email, string Phone, string Address);
public record UpdateContactDto(string FirstName, string LastName, string Email, string Phone, string Address);

public static class ContactExtensions
{
    public static ContactDto AsDto(this Contact contact)
    {
        return new ContactDto(contact.Id, contact.FirstName, contact.LastName, contact.Email, contact.Phone, contact.Address);
    }
}
