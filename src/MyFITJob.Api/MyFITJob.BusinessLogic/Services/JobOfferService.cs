using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFITJob.BusinessLogic.DTOs;
using MyFITJob.DAL;
using MyFITJob.Models;

namespace MyFITJob.BusinessLogic.Services;

public class JobOfferService : IJobOfferService
{
    private readonly IJobOfferRepository _jobOfferRepository;
    private readonly MyFITJobContext _context;
    private readonly ILogger<JobOfferService> _logger;

    public JobOfferService(IJobOfferRepository jobOfferRepository, MyFITJobContext context, ILogger<JobOfferService> logger)
    {
        _jobOfferRepository = jobOfferRepository;
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<JobOffer>> GetJobOffersAsync(string searchTerm, int? skillId = null)
    {
        var query = _context.JobOffers
            .Include(j => j.Skills)
            .OrderBy(j => j.Description)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(j => 
                j.Title.Contains(searchTerm) || 
                j.Description.Contains(searchTerm) ||
                j.Company.Contains(searchTerm));
        }

        var offers = query.ToList();
        
        if (skillId.HasValue)
        {
            query = query.Where(j => j.Skills.Any(s => s.Id == skillId.Value));
        }

        return await query.ToListAsync();
    }

    public async Task<JobOffer?> GetJobOfferByIdAsync(int id)
    {
        return await _context.JobOffers
            .Include(j => j.Skills)
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<JobOffer> CreateJobOfferAsync(CreateJobOfferDto dto)
    {
        _logger.LogInformation("Création d'une nouvelle offre d'emploi: {Title} chez {Company}", dto.Title, dto.Company);

        // Gestion des skills existants vs nouveaux
        var skills = new List<Skill>();
        
        foreach (var skillDto in dto.Skills)
        {
            // Rechercher un skill existant par nom (non-case sensitive)
            var existingSkill = await _context.Skills
                .FirstOrDefaultAsync(s => s.Name.ToLower() == skillDto.Name.ToLower());

            if (existingSkill != null)
            {
                _logger.LogDebug("Skill existant trouvé: {SkillName}", existingSkill.Name);
                skills.Add(existingSkill);
            }
            else
            {
                _logger.LogDebug("Création d'un nouveau skill: {SkillName}", skillDto.Name);
                var newSkill = new Skill
                {
                    Name = skillDto.Name,
                    Description = skillDto.Description
                };
                _context.Skills.Add(newSkill);
                skills.Add(newSkill);
            }
        }

        var jobOffer = new JobOffer
        {
            Title = dto.Title,
            Company = dto.Company,
            Location = dto.Location,
            Description = dto.Description,
            ExperienceLevel = dto.ExperienceLevel,
            ContractType = dto.ContractType,
            Salary = dto.Salary,
            Status = JobOfferStatus.New,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Skills = skills
        };

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