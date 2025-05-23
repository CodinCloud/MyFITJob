using MyFITJob.Models;

namespace MyFITJob.BusinessLogic.DTOs;

public record JobOfferDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Company { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ExperienceLevel { get; init; } = string.Empty;
    public string ContractType { get; init; } = string.Empty;
    public string Salary { get; init; } = string.Empty;
    public JobOfferStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public int CommentsCount { get; init; }
    public ICollection<SkillDto> Skills { get; init; } = new List<SkillDto>();

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
            Status = jobOffer.Status,
            CreatedAt = jobOffer.CreatedAt,
            UpdatedAt = jobOffer.UpdatedAt,
            CommentsCount = jobOffer.CommentsCount,
            Skills = jobOffer.Skills.Select(s => new SkillDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            }).ToList()
        };
    }
}

public record CreateJobOfferDto
{
    public string Title { get; init; } = string.Empty;
    public string Company { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ExperienceLevel { get; init; } = string.Empty;
    public string ContractType { get; init; } = string.Empty;
    public string Salary { get; init; } = string.Empty;
    public ICollection<CreateSkillDto> Skills { get; init; } = new List<CreateSkillDto>();
} 