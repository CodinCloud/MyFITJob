using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFITJob.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MyFITJob.DAL;

public static class DatabaseInitializerExtensions
{
    public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetService<MyFITJobContext>();
        await context.Database.MigrateAsync();
         
        var initialiser = scope.ServiceProvider.GetRequiredService<MyFITJobContextInitializer>();
        await initialiser.SeedAsync();
    }
}

public class MyFITJobContextInitializer(ILogger<MyFITJobContextInitializer> logger, MyFITJobContext context)
{
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // Default data
        var sampleJobOffers = new List<JobOffer>
        {
            new JobOffer
            {
                Id = 1,
                Title = "Développeur Full Stack Junior",
                Company = "TechNova",
                Location = "Paris, France",
                Description =
                    "Rejoignez notre équipe dynamique pour développer des applications web innovantes.",
                Requirements = new List<string>
                    {
                        "Connaissances en JavaScript, HTML, CSS", "Notions de React ou Angular"
                    },
                ExperienceLevel = "Débutant accepté",
                ContractType = "CDI",
                Salary = "30 000 € - 35 000 € par an"
            },
            new JobOffer
            {
                Id = 2,
                Title = "Ingénieur DevOps Junior",
                Company = "CloudSolutions",
                Location = "Lyon, France",
                Description =
                    "Participez à la mise en place de pipelines CI/CD et à la gestion de l'infrastructure cloud.",
                Requirements =
                    new List<string>
                    {
                        "Bases en Docker et Kubernetes", "Connaissances en scripts Bash ou Python"
                    },
                ExperienceLevel = "Débutant accepté",
                ContractType = "CDI",
                Salary = "32 000 € - 36 000 € par an"
            },
            new JobOffer
            {
                Id = 3,
                Title = "Administrateur Systèmes Junior",
                Company = "NetSecure",
                Location = "Marseille, France",
                Description =
                    "Assurez la maintenance et la supervision des systèmes informatiques de nos clients.",
                Requirements =
                    new List<string> { "Connaissances en Linux/Windows", "Notions de réseaux TCP/IP" },
                ExperienceLevel = "Débutant accepté",
                ContractType = "CDI",
                Salary = "28 000 € - 33 000 € par an"
            },
            new JobOffer
            {
                Id = 4,
                Title = "Développeur Backend Junior",
                Company = "DataCorp",
                Location = "Toulouse, France",
                Description = "Développez des API robustes et évolutives pour nos applications internes.",
                Requirements =
                    new List<string> { "Maîtrise de Java ou Node.js", "Connaissances en bases de données SQL" },
                ExperienceLevel = "Débutant accepté",
                ContractType = "CDI",
                Salary = "31 000 € - 34 000 € par an"
            },
            new JobOffer
            {
                Id = 5,
                Title = "Ingénieur d'Études Junior",
                Company = "Innovatech",
                Location = "Nantes, France",
                Description =
                    "Participez à la conception et au développement de solutions logicielles sur mesure.",
                Requirements =
                    new List<string> { "Connaissances en UML", "Notions de gestion de projet Agile" },
                ExperienceLevel = "Débutant accepté",
                ContractType = "CDI",
                Salary = "30 000 € - 35 000 € par an"
            },
            new JobOffer
            {
                Id = 6,
                Title = "Technicien Support Informatique",
                Company = "HelpDeskPro",
                Location = "Strasbourg, France",
                Description = "Fournissez un support technique de premier niveau aux utilisateurs.",
                Requirements =
                    new List<string> { "Bonne communication", "Connaissances de base en hardware et software" },
                ExperienceLevel = "Débutant accepté",
                ContractType = "CDD",
                Salary = "25 000 € - 28 000 € par an"
            },
            new JobOffer
            {
                Id = 7,
                Title = "Développeur Mobile Junior",
                Company = "AppCreators",
                Location = "Bordeaux, France",
                Description = "Contribuez au développement d'applications mobiles innovantes.",
                Requirements =
                    new List<string> { "Connaissances en Swift ou Kotlin", "Notions de design mobile" },
                ExperienceLevel = "Débutant accepté",
                ContractType = "CDI",
                Salary = "30 000 € - 33 000 € par an"
            },
            new JobOffer
            {
                Id = 8,
                Title = "Analyste Données Junior",
                Company = "DataInsights",
                Location = "Lille, France",
                Description = "Analysez les données pour fournir des insights pertinents aux équipes métiers.",
                Requirements = new List<string> { "Maîtrise d'Excel", "Notions de SQL et Python" },
                ExperienceLevel = "Débutant accepté",
                ContractType = "CDI",
                Salary = "29 000 € - 32 000 € par an"
            },
            new JobOffer
            {
                Id = 9,
                Title = "Testeur QA Junior",
                Company = "QualitySoft",
                Location = "Rennes, France",
                Description = "Testez les applications pour assurer leur qualité et leur fiabilité.",
                Requirements =
                    new List<string> { "Connaissances en tests fonctionnels", "Notions de Selenium ou JMeter" },
                ExperienceLevel = "Débutant accepté",
                ContractType = "CDI",
                Salary = "28 000 € - 31 000 € par an"
            },
            new JobOffer
            {
                Id = 10,
                Title = "Ingénieur Réseaux Junior",
                Company = "NetConnect",
                Location = "Grenoble, France",
                Description = "Assurez la configuration et la maintenance des infrastructures réseau.",
                Requirements =
                    new List<string> { "Connaissances en Cisco ou Juniper", "Notions de sécurité réseau" },
                ExperienceLevel = "Débutant accepté",
                ContractType = "CDI",
                Salary = "30 000 € - 34 000 € par an"
            }
        };

        if (!context.JobOffers.Any())
        {
            context.JobOffers.AddRange(sampleJobOffers);
            await context.SaveChangesAsync();
        }
    }
}
