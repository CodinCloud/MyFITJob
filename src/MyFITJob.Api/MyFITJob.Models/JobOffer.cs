namespace MyFITJob.Models;

public class JobOffer
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Company { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public string ExperienceLevel { get; set; }
    public string ContractType { get; set; }
    public string Salary { get; set; }
    public List<string> Requirements { get; set; }
    public JobOfferStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastInteraction { get; set; }
    public int CommentsCount { get; set; }
}