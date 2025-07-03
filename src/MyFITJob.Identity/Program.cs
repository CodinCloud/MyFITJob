using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyFITJob.Identity.Data;
using MyFITJob.Identity.Infrastructure;
using MyFITJob.Identity.Seed;
using MyFITJob.Identity.Settings;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddOpenApi();

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

var isSettings = builder.Configuration.GetSection("IdentityServerSettings")
    .Get<IdentityServerSettings>();

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
        builder.Configuration.GetConnectionString("MongoDB")!,
        builder.Configuration.GetSection("MongoDbSettings:DatabaseName").Value!
        );

builder.Services.AddIdentityServer(option =>
    {
        option.Events.RaiseSuccessEvents = true;
        option.Events.RaiseFailureEvents = true;
        option.Events.RaiseErrorEvents = true;
    })
    .AddAspNetIdentity<ApplicationUser>() // Map l'authentification OIDC pour utiliser notre base utilisateur AspNetUser
    .AddInMemoryApiScopes(isSettings.ApiScopes)
    .AddInMemoryClients(isSettings.Clients)
    .AddInMemoryIdentityResources(isSettings.IdentityResources)
    .AddDeveloperSigningCredential();

builder.Services.AddControllers();

// API Documentation avec .NET 9 natif
// builder.Services.AddEndpointsApiExplorer();

// CORS
/*
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
*/

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

// Pour le HTTP pour les tests docker
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax, // ou None, mais ça ne marchera pas sans HTTPS
    Secure = CookieSecurePolicy.Always // ou None pour dev, mais ce n'est pas recommandé
});

app.UseRouting();
app.UseIdentityServer();

app.UseAuthorization();

// app.MapOpenApi();
// app.MapScalarApiReference();

app.MapControllers();
app.MapRazorPages();

// Seed des données initiales
using (var scope = app.Services.CreateScope())
{
    await RolesSeeder.SeedRolesAsync(app.Services);
    await RolesSeeder.SeedDefaultAdminAsync(app.Services);
}

app.Run();
