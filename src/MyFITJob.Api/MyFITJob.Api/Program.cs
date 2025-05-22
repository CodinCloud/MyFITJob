using Microsoft.EntityFrameworkCore;
using MyFITJob.Api.Models;
using MyFITJob.BusinessLogic;
using MyFITJob.BusinessLogic.Services;
using MyFITJob.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<MyFITJobContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<MyFITJobContextInitializer>();
builder.Services.AddScoped<IJobOfferService, JobOfferService>();
builder.Services.AddScoped<IJobOfferRepository, JobOfferRepository>();
builder.Services.AddScoped<ISkillExtractorService, SkillExtractorService>();

builder.Services.AddCors((options) =>
{
    options.AddPolicy(name: "Development",
        builder =>
        {
            builder.WithOrigins("https://localhost:8080", "http://localhost:3000")
                .AllowAnyMethod()
                .AllowCredentials()
                .AllowAnyHeader();
        });
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.Services.SeedDatabaseAsync();
    app.MapOpenApi();
    app.UseCors("Development");
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();