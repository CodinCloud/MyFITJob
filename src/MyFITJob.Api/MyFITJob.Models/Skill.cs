namespace MyFITJob.Models;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public virtual ICollection<JobOffer> JobOffers { get; set; } = new List<JobOffer>();
} 