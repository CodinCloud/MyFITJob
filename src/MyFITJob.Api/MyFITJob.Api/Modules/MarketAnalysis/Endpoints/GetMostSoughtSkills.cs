using MyFITJob.Api.Infrastructure.Data;
using MyFITJob.Api.MarketAnalysis.Application;

namespace MyFITJob.Api.MarketAnalysis.Endpoints;

public static class GetMostSoughtSkills
{
    public static void MapGetMostSoughtSkills(this WebApplication app)
    {
        app.MapGet("/api/market/skills", async (
            IJobOfferRepository jobOfferRepository,
            ISkillExtractorService skillExtractorService,
            ILogger<Program> logger,
            int? top = 5) =>
        {
            try
            {
                var jobOffers = await jobOfferRepository.GetJobOffersAsync(string.Empty);
                
                // Concaténer tous les requirements
                var allRequirements = string.Join(" ", jobOffers.SelectMany(o => o.Requirements));
                
                // Extraire et compter les compétences
                var skillsCount = await skillExtractorService.ExtractSkillsAsync(allRequirements);

                // Trier et prendre le top N
                var topSkills = skillsCount
                    .OrderByDescending(x => x.Value)
                    .Take(top ?? 5)
                    .Select(x => new
                    {
                        Skill = x.Key,
                        Count = x.Value
                    });

                return Results.Ok(topSkills);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching most sought skills");
                return Results.Problem("An error occurred while processing your request", statusCode: 500);
            }
        })
        .WithName("GetMostSoughtSkills")
        .WithOpenApi();
    }
} 