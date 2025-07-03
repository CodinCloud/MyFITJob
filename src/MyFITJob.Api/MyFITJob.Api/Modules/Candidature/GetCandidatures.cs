namespace MyFITJob.Api.Modules.Candidature;

public static class GetCandidatures
{
    public static void MapGetCandidatures(this WebApplication app)
    {
        app.MapGet("/api/candidatures", async (
            ILogger<Program> logger) =>
        {
                return Results.Ok(new []
                {
                    
                   new {
                        Candidate = "JDoe",    
                        OfferId = 1,
                        Notes = "Notes sur l'offre d'emploi", 
                        State = "En attente"
                    },
                   new {
                        Candidate = "Another Student",    
                        OfferId = 2,
                        Notes = "Notes sur l'offre d'emploi", 
                        State = "Entretien planifié"
                    } 
                });
        })
        .WithName("GetCandidatures")
        .RequireAuthorization()
        .WithOpenApi();
    }
} 
