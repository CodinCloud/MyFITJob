using MassTransit;
using Microsoft.EntityFrameworkCore;
using MyFITJob.Api.Infrastructure.Data;
using MyFITJob.Api.Infrastructure.Integrations;
using MyFITJob.Api.JobOffers.Application;
using MyFITJob.Api.JobOffers.Endpoints;
using MyFITJob.Api.MarketAnalysis.Application;
using MyFITJob.Api.MarketAnalysis.Endpoints;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<MyFITJobContext>(options => options
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services .AddHttpClient();

builder.Services.AddScoped<MyFITJobContextInitializer>();
builder.Services.AddScoped<IJobOfferService, JobOfferService>();
builder.Services.AddScoped<IJobOfferRepository, JobOfferRepository>();
builder.Services.AddScoped<ISkillExtractorService, SkillExtractorService>();
builder.Services.AddScoped<IContactsService, ContactsService>();

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

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, cfg) =>
    {
       var rabbitMqHost = builder.Configuration["RabbitMQ:Host"]!;
       Console.WriteLine($"Connecting to RabbitMQ at: {rabbitMqHost}");
       
       cfg.Host(new Uri(rabbitMqHost));
       
       cfg.ConfigureEndpoints(context);
    });    
});

builder.Services.AddMassTransitHostedService();

// Ajout des métriques Prometheus via OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithMetrics(builder =>
    {
        builder.AddPrometheusExporter();

        builder.AddMeter("Microsoft.AspNetCore.Hosting",
                         "Microsoft.AspNetCore.Server.Kestrel");
        builder.AddView("http.server.request.duration",
            new ExplicitBucketHistogramConfiguration
            {
                Boundaries = new double[] { 0, 0.005, 0.01, 0.025, 0.05,
                       0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10 }
            });
    })
    .WithTracing(tracerProviderBuilder => {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("myfitjob-backend"))
            .SetSampler(new AlwaysOnSampler())
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddJaegerExporter(options =>
            {
                options.AgentHost = "localhost";
                options.AgentPort = 6831;
            });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.Services.SeedDatabaseAsync();
    app.MapOpenApi();
    app.UseCors("Development");
}

app.UseHttpsRedirection();

// Activation des métriques Prometheus
app.MapPrometheusScrapingEndpoint();

// Configuration des Minimal API Endpoints
app.MapGetJobOffers();
app.MapCreateJobOffer();
app.MapGetMostSoughtSkills();

app.Run();