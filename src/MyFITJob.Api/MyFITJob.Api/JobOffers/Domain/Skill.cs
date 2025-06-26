using System.ComponentModel.DataAnnotations;

namespace MyFITJob.Api.JobOffers.Domain;

public class Skill
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public List<JobOffer> JobOffers { get; set; } = new();
} 