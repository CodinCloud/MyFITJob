using MyFITJob.Api.JobOffers.Domain;

namespace MyFITJob.Api.JobOffers.DTOs;

public record SkillDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }

    public static SkillDto FromDomain(Skill skill)
    {
        return new SkillDto
        {
            Id = skill.Id,
            Name = skill.Name,
            Description = skill.Description
        };
    }
} 