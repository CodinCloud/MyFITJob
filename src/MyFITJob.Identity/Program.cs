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

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

// Configuration des règles de mot de passe plus souples pour l'apprentissage
builder.Services.Configure<IdentityOptions>(options =>
{
    // Règles de mot de passe simplifiées pour les étudiants
    options.Password.RequireDigit = false;           // Pas besoin de chiffres
    options.Password.RequireLowercase = false;       // Pas besoin de minuscules
    options.Password.RequireNonAlphanumeric = false; // Pas besoin de caractères spéciaux
    options.Password.RequireUppercase = false;       // Pas besoin de majuscules
    options.Password.RequiredLength = 3;             // Minimum 3 caractères seulement
    options.Password.RequiredUniqueChars = 0;        // Pas de caractères uniques requis

    // Règles de nom d'utilisateur
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
});

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
        builder.Configuration.GetConnectionString("MongoDB")!,
        builder.Configuration.GetSection("MongoDbSettings:DatabaseName").Value!
        );

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings!.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        // Remapping des claims legacy vers des noms plus modernes
        NameClaimType = "name",
        RoleClaimType = "role"
    };
    
    // Configuration des claims pour éviter les URLs XML longues
    options.MapInboundClaims = false;
});

// Services
builder.Services.AddScoped<JwtTokenGenerator>();

// Controllers
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// API Documentation avec .NET 9 natif
builder.Services.AddEndpointsApiExplorer();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();

// Seed des données initiales
using (var scope = app.Services.CreateScope())
{
    await RolesSeeder.SeedRolesAsync(app.Services);
    await RolesSeeder.SeedDefaultAdminAsync(app.Services);
}

app.Run();
