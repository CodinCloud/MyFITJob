using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Contacts.Api;
using Contacts.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var contactRepository = new ContactRepository("mongodb://localhost:27017");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Endpoint pour récupérer tous les contacts
var routes = app.MapGroup("/api");

routes.MapGet("/contacts", async ()
    => (await contactRepository.GetAllAsync())
        .Select(contact => contact.AsDto()))
            .WithName("GetContacts")
            .WithOpenApi();

// Endpoint pour récupérer un contact par son ID
routes.MapGet("/contacts/{id}", async (Guid id) =>
{
    var contact = await contactRepository.GetAsync(id);
    return contact is null ? Results.NotFound() : Results.Ok(contact);
})
.WithName("GetContactById")
.WithOpenApi();

routes.MapPost("/contacts", async ([FromBody] CreateContactDto contact) =>
{
    // TODO validation
    var addingContact = new Contact(Guid.NewGuid(), contact.FirstName, contact.LastName, contact.Email, contact.Phone, contact.Address);
    var newContact = await contactRepository.CreateAsync(addingContact);
    return Results.Created($"/contacts/{newContact.Id}", newContact);
})
.WithName("PostContact")
.WithOpenApi();

routes.MapPut("/contacts/{id}", async (Guid id, [FromBody] UpdateContactDto contact) =>
{
    // TODO validation
    var updating = new Contact(id, contact.FirstName, contact.LastName, contact.Email, contact.Phone, contact.Address);
    var updatedContact = await contactRepository.UpdateAsync(id, updating);

    return Results.NoContent();
})
.WithName("PutContact")
.WithOpenApi();

routes.MapDelete("/contacts/{id}", async (Guid id) =>
{
    await contactRepository.DeleteAsync(id);
    return Results.NoContent();
})
.WithName("DeleteContact")
.WithOpenApi();

app.Run();
