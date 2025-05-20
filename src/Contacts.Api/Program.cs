using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Contacts.Api;
using Contacts.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var contactRepository = new ContactRepository("contacts.json");

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

routes.MapGet("/contacts", () => contactRepository.GetAll())
    .WithName("GetContacts")
    .WithOpenApi();

// Endpoint pour récupérer un contact par son ID
routes.MapGet("/contacts/{id}", (Guid id) => 
{
    var contact = contactRepository.Get(id);
    return contact is null ? Results.NotFound() : Results.Ok(contact);
})
.WithName("GetContactById")
.WithOpenApi();

routes.MapPost("/contacts", ([FromBody] ContactDto contact) => 
{
    // TODO validation

    var addingContact = new Contact(contact.Id, contact.FirstName, contact.LastName, contact.Email, contact.Phone, contact.Address);
    var newContact = contactRepository.Create(addingContact);
    return Results.Created($"/contacts/{newContact.Id}", newContact);
})
.WithName("PostContact")
.WithOpenApi();

routes.MapPut("/contacts/{id}", (Guid id, [FromBody] ContactDto contact) => 
{
    // TODO validation
    var updating = new Contact(contact.Id, contact.FirstName, contact.LastName, contact.Email, contact.Phone, contact.Address);
    var updatedContact = contactRepository.Update(id, updating);

    return Results.NoContent();
})
.WithName("PutContact")
.WithOpenApi();

routes.MapDelete("/contacts/{id}", (Guid id) => 
{
    contactRepository.Delete(id); 
    return Results.NoContent();
})
.WithName("DeleteContact")
.WithOpenApi();

app.Run();
