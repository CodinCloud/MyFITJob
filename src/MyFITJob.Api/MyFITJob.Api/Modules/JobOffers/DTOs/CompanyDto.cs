namespace MyFITJob.Api.JobOffers.DTOs;

public record CompanyInfo
{
    public static CompanyInfo NullCompanyInfo = new CompanyInfo()
    {
        Id = 0,
        Name = "Inconnue",
        Industry = "Inconnue",
        Description = "",
        Rating = 0,
        Size = "Inconnue"
    }; 
    
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Industry { get; init; } = string.Empty;
    public string Size { get; init; } = string.Empty;
    public double Rating { get; init; }
    public string Description { get; init; } = string.Empty;
}

public record CreateCompanyDto
{
    public string Name { get; init; } = string.Empty;
    public string Industry { get; init; } = string.Empty;
    public string Size { get; init; } = string.Empty;
    public double? Rating { get; init; }
    public string Description { get; init; } = string.Empty;
} 