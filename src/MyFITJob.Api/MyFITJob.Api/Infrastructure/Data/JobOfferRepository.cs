using Microsoft.EntityFrameworkCore;
using MyFITJob.Api.JobOffers.Domain;

namespace MyFITJob.Api.Infrastructure.Data;

public class JobOfferRepository : IJobOfferRepository
{
    private readonly MyFITJobContext _context;

    public JobOfferRepository(MyFITJobContext context)
    {
        _context = context;
    }

    public async Task<List<JobOffer>> GetJobOffersAsync(string searchTerm)
    {
        var query = _context.JobOffers
            .Include(j => j.Skills)
            .OrderBy(j => j.Title)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(j => 
                j.Title.Contains(searchTerm) || 
                j.Description.Contains(searchTerm));
        }

        return await query.ToListAsync();
    }

    public async Task<JobOffer> CreateJobOfferAsync(JobOffer jobOffer)
    {
        _context.JobOffers.Add(jobOffer);
        await _context.SaveChangesAsync();
        return jobOffer;
    }

    public async Task<List<Skill>> GetSkillsAsync()
    {
        return await _context.Skills.ToListAsync();
    }

    public async Task AddSkillsAsync(List<Skill> skills)
    {
        _context.Skills.AddRange(skills);
        await _context.SaveChangesAsync();
    }
} 