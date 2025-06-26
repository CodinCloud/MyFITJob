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
    private static readonly Dictionary<string, (string Description, string[] Skills)> JobTemplates = new()
    {
        ["Développeur Full Stack"] = (
            "Rejoignez notre équipe dynamique pour développer des applications web innovantes.",
            new[] { "JavaScript", "HTML", "CSS", "React", "Angular", "Node.js", "TypeScript" }
        ),
        ["DevOps"] = (
            "Participez à la mise en place de pipelines CI/CD et à la gestion de l'infrastructure cloud.",
            new[] { "Docker", "Kubernetes", "Bash", "Python", "CI/CD", "Terraform", "AWS" }
        ),
        ["Administrateur Systèmes"] = (
            "Assurez la maintenance et la supervision des systèmes informatiques de nos clients.",
            new[] { "Linux", "Windows", "TCP/IP", "Réseaux", "Active Directory", "Powershell" }
        ),
        ["Développeur Backend"] = (
            "Développez des API robustes et évolutives pour nos applications internes.",
            new[] { "Java", "Node.js", "SQL", "REST", "Microservices", "Spring Boot" }
        ),
        ["Ingénieur d'Études"] = (
            "Participez à la conception et au développement de solutions logicielles sur mesure.",
            new[] { "UML", "Agile", "Java", "Design Patterns", "Git", "Jira" }
        ),
        ["Support Informatique"] = (
            "Fournissez un support technique de premier niveau aux utilisateurs.",
            new[] { "Hardware", "Software", "Windows", "Office", "Résolution de problèmes" }
        ),
        ["Développeur Mobile"] = (
            "Contribuez au développement d'applications mobiles innovantes.",
            new[] { "Swift", "Kotlin", "React Native", "Flutter", "iOS", "Android" }
        ),
        ["Analyste Données"] = (
            "Analysez les données pour fournir des insights pertinents aux équipes métiers.",
            new[] { "SQL", "Python", "Excel", "Power BI", "Data Analysis", "Statistics" }
        ),
        ["Testeur QA"] = (
            "Testez les applications pour assurer leur qualité et leur fiabilité.",
            new[] { "Selenium", "JMeter", "Test Cases", "Bug Tracking", "Automation" }
        ),
        ["Ingénieur Réseaux"] = (
            "Assurez la configuration et la maintenance des infrastructures réseau.",
            new[] { "Cisco", "Juniper", "Sécurité réseau", "VPN", "Firewall", "LAN/WAN" }
        )
    };

    private static readonly string[] Companies = new[]
    {
        "TechNova", "CloudSolutions", "NetSecure", "DataCorp", "Innovatech",
        "HelpDeskPro", "AppCreators", "DataInsights", "QualitySoft", "NetConnect",
        "CyberTech", "WebMasters", "CodeCraft", "DataFlow", "SmartSystems",
        "FutureTech", "DigitalWave", "ByteForce", "TechVision", "CodeGenius"
    };

    private static readonly string[] Locations = new[]
    {
        "Paris", "Lyon", "Marseille", "Toulouse", "Nantes",
        "Strasbourg", "Bordeaux", "Lille", "Rennes", "Grenoble",
        "Nice", "Montpellier", "Lille", "Toulon", "Angers",
        "Brest", "Dijon", "Saint-Étienne", "Le Havre", "Reims"
    };

    private static readonly string[] ExperienceLevels = new[]
    {
        "Débutant accepté",
        "1-3 ans d'expérience",
        "3-5 ans d'expérience",
        "5-8 ans d'expérience",
        "8+ ans d'expérience"
    };

    private static readonly string[] ContractTypes = new[]
    {
        "CDI",
        "CDD",
        "Freelance",
        "Stage",
        "Alternance"
    };

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
        // Création des skills uniques
        var allSkills = JobTemplates.Values
            .SelectMany(t => t.Skills)
            .Distinct()
            .Select(skillName => new Skill
            {
                Name = skillName,
                Description = $"Compétence en {skillName}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            })
            .ToList();

        // Sauvegarde des skills et récupération des IDs
        foreach (var skill in allSkills)
        {
            var existingSkill = await context.Skills
                .FirstOrDefaultAsync(s => s.Name == skill.Name);

            if (existingSkill == null)
            {
                context.Skills.Add(skill);
            }
        }
        await context.SaveChangesAsync();

        // Récupération de tous les skills pour les utiliser dans les offres
        var skills = await context.Skills.ToListAsync();

        // Création des offres d'emploi si nécessaire
        if (!context.JobOffers.Any())
        {
            var jobOffers = new List<JobOffer>();
            var batchSize = 100; // Taille des lots pour l'insertion
            var builder = new JobOfferBuilder(JobTemplates, Companies, Locations, ExperienceLevels, ContractTypes);

            for (int i = 0; i < 0; i++)
            {
                var template = JobTemplates.ElementAt(new Random().Next(JobTemplates.Count));
                var jobOffer = builder
                    .New()
                    .WithRandomTemplate()
                    .WithRandomCompany()
                    .WithRandomLocation()
                    .WithRandomExperienceLevel()
                    .WithRandomContractType()
                    .WithRandomSalary()
                    .WithRandomStatus()
                    .WithRandomDates()
                    .WithRandomCommentsCount()
                    .WithRandomSkills(skills, template.Value.Skills)
                    .Build();

                jobOffers.Add(jobOffer);

                // Sauvegarde par lots pour optimiser les performances
                if (jobOffers.Count >= batchSize)
                {
                    context.JobOffers.AddRange(jobOffers);
                    await context.SaveChangesAsync();
                    jobOffers.Clear();
                }
            }

            // Sauvegarde des offres restantes
            if (jobOffers.Any())
            {
                context.JobOffers.AddRange(jobOffers);
                await context.SaveChangesAsync();
            }
        }
    }
}
