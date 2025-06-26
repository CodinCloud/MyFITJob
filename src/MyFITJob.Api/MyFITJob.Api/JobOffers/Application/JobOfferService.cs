using Microsoft.EntityFrameworkCore;
using MyFITJob.Api.Infrastructure.Data;
using MyFITJob.Api.Infrastructure.Integrations;
using MyFITJob.Api.JobOffers.Domain;
using MyFITJob.Api.JobOffers.DTOs;

namespace MyFITJob.Api.JobOffers.Application;

public class JobOfferService : IJobOfferService
{
    private readonly IJobOfferRepository _jobOfferRepository;
    private readonly MyFITJobContext _context;
    private readonly ILogger<JobOfferService> _logger;
    private readonly IContactsService _contactsService;

    public JobOfferService(
        IJobOfferRepository jobOfferRepository,
        MyFITJobContext context,
        ILogger<JobOfferService> logger,
        IContactsService contactsService)
    {
        _jobOfferRepository = jobOfferRepository;
        _context = context;
        _logger = logger;
        _contactsService = contactsService;
    }

    public async Task<List<JobOffer>> GetJobOffersAsync(string searchTerm)
    {
        _logger.LogInformation("Récupération des offres d'emploi avec le terme de recherche: {SearchTerm}", searchTerm);
        
        var jobOffers = await _jobOfferRepository.GetJobOffersAsync(searchTerm);
        
        _logger.LogInformation("Récupération de {Count} offres d'emploi", jobOffers.Count);
        
        return jobOffers;
    }

    public async Task<JobOffer?> GetJobOfferByIdAsync(int id)
    {
        return await _context.JobOffers
            .Include(j => j.Skills)
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<JobOffer> CreateJobOfferAsync(CreateJobOfferDto dto)
    {
        _logger.LogInformation("Création d'une nouvelle offre d'emploi: {Title}", dto.Title);

        // Créer ou récupérer l'entreprise via l'API Contacts
        CompanyInfo? companyInfo = null;
        try
        {
            var createCompanyDto = new CreateCompanyDto
            {
                Name = dto.Company,
                Industry = "Tech", // Valeur par défaut
                Size = "51-200",   // Valeur par défaut
                Description = $"Entreprise pour l'offre: {dto.Title}"
            };

            companyInfo = await _contactsService.CreateCompanyAsync(createCompanyDto);
            _logger.LogInformation("Entreprise créée/récupérée avec succès: {CompanyName}", companyInfo.Name);
        }
        catch (Exception ex)
        {
            // Gestion d'erreur gracieuse - on continue avec des valeurs par défaut
            _logger.LogWarning(ex, "Impossible de créer/récupérer l'entreprise {Company}", dto.Company);
        }

        // Créer l'offre d'emploi
        var jobOffer = new JobOffer
        {
            Title = dto.Title,
            Company = dto.Company,
            Location = dto.Location,
            Salary = dto.Salary,
            Description = dto.Description,
            Requirements = dto.Requirements,
            Status = JobOfferStatus.New,
            CreatedAt = DateTime.UtcNow
        };

        // Gérer les compétences existantes vs nouvelles
        if (dto.Skills.Any())
        {
            var existingSkills = await _jobOfferRepository.GetSkillsAsync();
            var newSkills = new List<Skill>();

            foreach (var skillDto in dto.Skills)
            {
                var existingSkill = existingSkills.FirstOrDefault(s => s.Name.Equals(skillDto.Name, StringComparison.OrdinalIgnoreCase));
                
                if (existingSkill != null)
                {
                    jobOffer.Skills.Add(existingSkill);
                }
                else
                {
                    // Créer une nouvelle compétence
                    var newSkill = new Skill
                    {
                        Name = skillDto.Name,
                        Description = skillDto.Description,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    newSkills.Add(newSkill);
                    jobOffer.Skills.Add(newSkill);
                }
            }

            // Ajouter les nouvelles compétences à la base de données
            if (newSkills.Any())
            {
                await _jobOfferRepository.AddSkillsAsync(newSkills);
            }
        }

        var createdJobOffer = await _jobOfferRepository.CreateJobOfferAsync(jobOffer);
        
        _logger.LogInformation("Offre d'emploi créée avec succès. ID: {JobOfferId}", createdJobOffer.Id);
        
        return createdJobOffer;
    }

    public async Task<JobOffer?> UpdateJobOfferAsync(int id, CreateJobOfferDto dto)
    {
        var jobOffer = await _context.JobOffers
            .Include(j => j.Skills)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (jobOffer == null)
            return null;

        jobOffer.Title = dto.Title;
        jobOffer.Company = dto.Company;
        jobOffer.Location = dto.Location;
        jobOffer.Description = dto.Description;
        jobOffer.ExperienceLevel = dto.ExperienceLevel;
        jobOffer.ContractType = dto.ContractType;
        jobOffer.Salary = dto.Salary;
        jobOffer.UpdatedAt = DateTime.UtcNow;

        // Mise à jour des Skills
        _context.Skills.RemoveRange(jobOffer.Skills);
        jobOffer.Skills = dto.Skills.Select(s => new Skill
        {
            Name = s.Name,
            Description = s.Description
        }).ToList();

        await _context.SaveChangesAsync();
        return jobOffer;
    }

    public async Task<bool> DeleteJobOfferAsync(int id)
    {
        var jobOffer = await _context.JobOffers.FindAsync(id);
        if (jobOffer == null)
            return false;

        _context.JobOffers.Remove(jobOffer);
        await _context.SaveChangesAsync();
        return true;
    }
} 