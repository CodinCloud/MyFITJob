using System.ComponentModel.DataAnnotations;

namespace MyFITJob.Api.JobOffers.Domain;

public class JobOffer
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public int? CompanyId { get; set; } 

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public List<string> Requirements { get; set; } = new();

    public string ExperienceLevel { get; set; } = string.Empty;

    public string ContractType { get; set; } = string.Empty;

    public string Salary { get; set; } = string.Empty;

    public JobOfferStatus Status { get; set; } = JobOfferStatus.New;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? LastInteraction { get; set; }

    public int CommentsCount { get; set; }

    // Navigation property pour la relation many-to-many
    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
}
