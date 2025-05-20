using System.Text.Json;

namespace Contacts.Api;

public record Contact(Guid Id, string FirstName, string LastName, string Email, string Phone, string Address);

public class ContactRepository
{
    private readonly string _filePath;
    private List<Contact> _contacts;

    public ContactRepository(string filePath)
    {
        _filePath = Path.Combine(AppContext.BaseDirectory, filePath);
        LoadContacts();
    }

    private void LoadContacts()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            _contacts = JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
        }
        else
        {
            _contacts = new List<Contact>();
            Console.WriteLine($"Le fichier {_filePath} n'a pas été trouvé.");
        }
    }

    private void SaveContacts()
    {
        string json = JsonSerializer.Serialize(_contacts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public IEnumerable<Contact> GetAll()
    {
        return _contacts;
    }

    public Contact? Get(Guid id)
    {
        return _contacts.FirstOrDefault(c => c.Id == id);
    }

    public Contact Create(Contact contact)
    {
        // Si l'ID est vide, on en génère un nouveau
        if (contact.Id == Guid.Empty)
        {
            contact = contact with { Id = Guid.NewGuid() };
        }

        // Vérifier si un contact avec cet ID existe déjà
        var existingContact = _contacts.FirstOrDefault(c => c.Id == contact.Id);
        if (existingContact != null)
        {
            // Remplacer le contact existant
            _contacts.Remove(existingContact);
        }

        _contacts.Add(contact);
        SaveContacts();
        return contact;
    }

    public Contact? Update(Guid id, Contact updatedContact)
    {
        var existingContact = _contacts.FirstOrDefault(c => c.Id == id);

        var contact = updatedContact with { Id = id };
        var index = _contacts.IndexOf(existingContact);
        _contacts[index] = contact;

        SaveContacts();
        return contact;
    }

    public void Delete(Guid id)
    {
        var contact = _contacts.FirstOrDefault(c => c.Id == id);
        if (contact != null)
        {
            _contacts.Remove(contact);
            SaveContacts();
        }
    }
} 