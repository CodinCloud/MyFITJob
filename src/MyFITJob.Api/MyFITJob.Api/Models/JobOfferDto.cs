using MyFITJob.Models;

namespace MyFITJob.Api.Models;

public class JobOfferDto
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
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastInteraction { get; set; }
    public int CommentsCount { get; set; }

    public static JobOfferDto FromDomain(JobOffer jobOffer)
    {
        return new JobOfferDto
        {
            Id = jobOffer.Id,
            Title = jobOffer.Title,
            Company = jobOffer.Company,
            Location = jobOffer.Location,
            Description = jobOffer.Description,
            ExperienceLevel = jobOffer.ExperienceLevel,
            ContractType = jobOffer.ContractType,
            Salary = jobOffer.Salary,
            Requirements = jobOffer.Requirements,
            Status = jobOffer.Status.Name,
            CreatedAt = jobOffer.CreatedAt,
            UpdatedAt = jobOffer.UpdatedAt,
            LastInteraction = jobOffer.LastInteraction,
            CommentsCount = jobOffer.CommentsCount
        };
    }
}
