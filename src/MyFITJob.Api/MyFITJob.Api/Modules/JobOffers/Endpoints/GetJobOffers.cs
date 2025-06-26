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
                var allJobOfferDtos = await jobOfferService.GetJobOffersAsync(String.Empty);
                
                return Results.Ok(allJobOfferDtos);
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