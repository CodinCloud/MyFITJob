using MyFITJob.Api.JobOffers.Domain;

namespace MyFITJob.Api.Infrastructure.Data;

public class JobOfferBuilder
{
    private readonly Random _random;
    private JobOffer _jobOffer;
    private readonly Dictionary<string, (string Description, string[] Skills)> _templates;
    private readonly string[] _companies;
    private readonly string[] _locations;
    private readonly string[] _experienceLevels;
    private readonly string[] _contractTypes;
    private readonly string[] _titleSuffixes;

    public JobOfferBuilder(
        Dictionary<string, (string Description, string[] Skills)> templates,
        string[] companies,
        string[] locations,
        string[] experienceLevels,
        string[] contractTypes)
    {
        _random = new Random();
        _templates = templates;
        _companies = companies;
        _locations = locations;
        _experienceLevels = experienceLevels;
        _contractTypes = contractTypes;
        _titleSuffixes = new[] { "Junior", "Senior", "Expert", "Lead", "Architect", "Consultant" };
    }

    public JobOfferBuilder New()
    {
       _jobOffer = new JobOffer(); 
       return this;
    }
    
    public JobOfferBuilder WithRandomTemplate()
    {
        var template = _templates.ElementAt(_random.Next(_templates.Count));
        _jobOffer.Title = $"{template.Key} {_titleSuffixes[_random.Next(_titleSuffixes.Length)]}";
        _jobOffer.Description = template.Value.Description;
        return this;
    }

    public JobOfferBuilder WithRandomCompany()
    {
        _jobOffer.Company = _companies[_random.Next(_companies.Length)];
        return this;
    }

    public JobOfferBuilder WithRandomLocation()
    {
        _jobOffer.Location = $"{_locations[_random.Next(_locations.Length)]}, France";
        return this;
    }

    public JobOfferBuilder WithRandomExperienceLevel()
    {
        _jobOffer.ExperienceLevel = _experienceLevels[_random.Next(_experienceLevels.Length)];
        return this;
    }

    public JobOfferBuilder WithRandomContractType()
    {
        _jobOffer.ContractType = _contractTypes[_random.Next(_contractTypes.Length)];
        return this;
    }

    public JobOfferBuilder WithRandomSalary()
    {
        var baseSalary = 28000 + (_random.Next(20) * 1000);
        _jobOffer.Salary = $"{baseSalary} € - {baseSalary + 5000} € par an";
        return this;
    }

    public JobOfferBuilder WithRandomStatus()
    {
        var statuses = JobOfferStatus.GetAll<JobOfferStatus>().ToList();
        _jobOffer.Status = statuses[_random.Next(statuses.Count)];
        return this;
    }

    public JobOfferBuilder WithRandomDates()
    {
        _jobOffer.CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(365));
        _jobOffer.UpdatedAt = DateTime.UtcNow.AddDays(-_random.Next(30));
        return this;
    }

    public JobOfferBuilder WithRandomCommentsCount()
    {
        _jobOffer.CommentsCount = _random.Next(10);
        return this;
    }

    public JobOfferBuilder WithRandomSkills(List<Skill> allSkills, string[] templateSkills)
    {
        // Sélectionne 2-5 skills aléatoires parmi les skills du template
        var selectedSkills = templateSkills
            .OrderBy(x => _random.Next())
            .Take(_random.Next(2, 6))
            .Select(skillName => allSkills.First(s => s.Name == skillName))
            .ToList();

        // Ajoute 0-2 skills aléatoires supplémentaires
        var additionalSkills = allSkills
            .Where(s => !templateSkills.Contains(s.Name))
            .OrderBy(x => _random.Next())
            .Take(_random.Next(3))
            .ToList();

        _jobOffer.Skills = selectedSkills.Concat(additionalSkills).ToList();
        return this;
    }

    public JobOffer Build()
    {
        return _jobOffer;
    }
} 