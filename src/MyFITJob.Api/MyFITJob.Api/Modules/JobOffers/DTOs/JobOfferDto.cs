using MyFITJob.Api.JobOffers.Domain;

namespace MyFITJob.Api.JobOffers.DTOs;

public record JobOfferDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Company { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string Salary { get; init; }
    public string Description { get; init; } = string.Empty;
    public List<string> Requirements { get; init; } = new();
    public JobOfferStatusDto Status { get; init; } 
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public List<SkillDto> Skills { get; init; } = new();
    
    // Informations enrichies de l'entreprise
    public CompanyInfo CompanyInfo { get; set; }
    
    public static JobOfferDto FromDomain(JobOffer jobOffer, CompanyInfo? companyInfo = null)
    {
        return new JobOfferDto
        {
            Id = jobOffer.Id,
            Title = jobOffer.Title,
            Location = jobOffer.Location,
            Salary = jobOffer.Salary,
            Description = jobOffer.Description,
            Requirements = jobOffer.Requirements,
            Status = new JobOfferStatusDto()
            {
               Name = jobOffer.Status.Name,  
               DisplayName = jobOffer.Status.DisplayName,
            },
            CreatedAt = jobOffer.CreatedAt,
            UpdatedAt = jobOffer.UpdatedAt,
            Skills = jobOffer.Skills.Select(s => SkillDto.FromDomain(s)).ToList(),
            
            CompanyInfo = companyInfo ?? CompanyInfo.NullCompanyInfo
        };
    }
}

public class JobOfferStatusDto
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
}