using System.ComponentModel.DataAnnotations;
using MyFITJob.Api.MarketAnalysis.DTOs;

namespace MyFITJob.Api.JobOffers.DTOs;

public record CreateJobOfferDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; init; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Company { get; init; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Location { get; init; } = string.Empty;
    
    [Required]
    public string Salary { get; init; }
    
    [Required]
    public string ContractType { get; init; }
    
    [Required]
    public string ExperienceLevel { get; init; }
    
    [Required]
    [MaxLength(1000)]
    public string Description { get; init; } = string.Empty;
    
    [Required]
    public List<string> Requirements { get; init; } = new();
    
    public List<CreateSkillDto> Skills { get; init; } = new();
} 