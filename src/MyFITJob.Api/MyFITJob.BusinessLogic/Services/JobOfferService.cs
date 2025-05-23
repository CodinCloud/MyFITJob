using Microsoft.EntityFrameworkCore;
using MyFITJob.BusinessLogic.DTOs;
using MyFITJob.DAL;
using MyFITJob.Models;

namespace MyFITJob.BusinessLogic.Services;

public class JobOfferService : IJobOfferService
{
    private readonly MyFITJobContext _context;

    public JobOfferService(MyFITJobContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JobOffer>> GetJobOffersAsync(string searchTerm, int? skillId = null)
    {
        var query = _context.JobOffers
            .Include(j => j.Skills)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(j => 
                j.Title.Contains(searchTerm) || 
                j.Description.Contains(searchTerm) ||
                j.Company.Contains(searchTerm));
        }

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
            Skills = dto.Skills.Select(s => new Skill
            {
                Name = s.Name,
                Description = s.Description
            }).ToList()
        };

        _context.JobOffers.Add(jobOffer);
        await _context.SaveChangesAsync();
        return jobOffer;
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