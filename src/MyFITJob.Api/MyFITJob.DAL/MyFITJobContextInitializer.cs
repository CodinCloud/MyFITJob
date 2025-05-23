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

        // Création des offres d'emploi si nécessaire
        if (!context.JobOffers.Any())
        {
            var companies = new[] { "TechNova", "CloudSolutions", "NetSecure", "DataCorp", "Innovatech", 
                                  "HelpDeskPro", "AppCreators", "DataInsights", "QualitySoft", "NetConnect" };
            var locations = new[] { "Paris", "Lyon", "Marseille", "Toulouse", "Nantes", 
                                  "Strasbourg", "Bordeaux", "Lille", "Rennes", "Grenoble" };

            for (int i = 0; i < 10; i++)
            {
                var template = JobTemplates.ElementAt(i);
                var jobOffer = new JobOffer
                {
                    Id = i + 1,
                    Title = $"{template.Key} Junior",
                    Company = companies[i],
                    Location = $"{locations[i]}, France",
                    Description = template.Value.Description,
                    ExperienceLevel = "Débutant accepté",
                    ContractType = "CDI",
                    Salary = $"{28000 + (i * 1000)} € - {33000 + (i * 1000)} € par an",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Récupération des skills existants pour cette offre
                var skills = await context.Skills
                    .Where(s => template.Value.Skills.Contains(s.Name))
                    .ToListAsync();

                // Ajout direct des skills à l'offre
                jobOffer.Skills = skills;

                context.JobOffers.Add(jobOffer);
            }

            await context.SaveChangesAsync();
        }
    }
}
