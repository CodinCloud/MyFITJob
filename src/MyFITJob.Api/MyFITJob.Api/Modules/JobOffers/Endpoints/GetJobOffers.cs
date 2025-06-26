using Microsoft.AspNetCore.Mvc;
using MyFITJob.Api.JobOffers.Application;
using MyFITJob.Api.JobOffers.DTOs;

namespace MyFITJob.Api.JobOffers.Endpoints;

public static class GetJobOffers
{
    public static void MapGetJobOffers(this WebApplication app)
    {
        app.MapGet("/api/joboffers", async (
            IJobOfferService jobOfferService,
            ILogger<Program> logger) =>
        {
            try
            {
                var allJobOffers = await jobOfferService.GetJobOffersAsync(String.Empty);
                
                // Pour l'instant, on retourne les offres sans enrichissement
                // Dans une vraie implémentation, on enrichirait avec les infos d'entreprise
                var jobOfferDtos = allJobOffers.Select(jobOffer => JobOfferDto.FromDomain(jobOffer)).ToList();
                
                return Results.Ok(jobOfferDtos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la récupération des offres d'emploi");
                return Results.Problem( new ProblemDetails()
                {
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError,
                });
            }
        })
        .WithName("GetJobOffers")
        .WithOpenApi();
    }
} 