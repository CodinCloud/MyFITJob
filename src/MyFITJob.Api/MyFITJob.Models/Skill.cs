namespace MyFITJob.Models;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Clé étrangère et propriété de navigation
    public int JobOfferId { get; set; }
    public JobOffer JobOffer { get; set; } = null!;
} 