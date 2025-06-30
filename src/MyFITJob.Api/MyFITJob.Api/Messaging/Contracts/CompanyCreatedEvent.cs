namespace MyFITJob.Api.Messaging.Contracts;

/// <summary>
/// Événement déclenché quand une entreprise est créée ou trouvée dans le Contact API
/// Suite à la réception d'un événement JobOfferCreated
/// </summary>
public record CompanyCreatedEvent
{
    /// <summary>
    /// Identifiant unique de l'entreprise dans MongoDB (ObjectId)
    /// </summary>
    public string CompanyId { get; init; }
    
    /// <summary>
    /// Identifiant de l'offre d'emploi qui a provoqué la création/récupération de l'entreprise
    /// </summary>
    public required int JobOfferId { get; init; }
    
    /// <summary>
    /// Nom de l'entreprise
    /// </summary>
    public required string CompanyName { get; init; }
    
    /// <summary>
    /// Secteur d'activité de l'entreprise
    /// </summary>
    public string? Industry { get; init; }
    
    /// <summary>
    /// Taille de l'entreprise (nombre d'employés)
    /// </summary>
    public string? Size { get; init; }
    
    /// <summary>
    /// Date de création de l'entreprise
    /// </summary>
    public DateTime CreatedAt { get; init; }
} 