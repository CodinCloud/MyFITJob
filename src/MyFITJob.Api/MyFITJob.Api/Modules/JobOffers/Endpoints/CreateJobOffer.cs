using MyFITJob.Api.JobOffers.Application;
using MyFITJob.Api.JobOffers.DTOs;

namespace MyFITJob.Api.JobOffers.Endpoints;

public static class CreateJobOffer
{
    public static void MapCreateJobOffer(this WebApplication app)
    {
        app.MapPost("/api/joboffers", async (
            CreateJobOfferDto createJobOfferDto,
            IJobOfferService jobOfferService,
            ILogger<Program> logger) =>
        {
            try
            {
                logger.LogInformation("Réception d'une demande de création d'offre d'emploi: {Title}", createJobOfferDto.Title);

                var createdJobOffer = await jobOfferService.CreateJobOfferAsync(createJobOfferDto);
                
                // Pour l'instant, on retourne sans enrichissement
                // Dans une vraie implémentation, on récupérerait les infos d'entreprise
                var jobOfferDto = JobOfferDto.FromDomain(createdJobOffer);

                logger.LogInformation("Offre d'emploi créée avec succès. ID: {JobOfferId}", createdJobOffer.Id);

                return Results.CreatedAtRoute("GetJobOffers", new { id = createdJobOffer.Id }, jobOfferDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la création de l'offre d'emploi: {Title}", createJobOfferDto.Title);
                return Results.Problem("Une erreur interne s'est produite lors de la création de l'offre d'emploi.", statusCode: 500);
            }
        })
        .WithName("CreateJobOffer")
        .WithOpenApi();
    }
} 