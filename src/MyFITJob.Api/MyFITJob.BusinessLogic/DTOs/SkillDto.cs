namespace MyFITJob.BusinessLogic.DTOs;

public record SkillDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}

public record CreateSkillDto
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
} 