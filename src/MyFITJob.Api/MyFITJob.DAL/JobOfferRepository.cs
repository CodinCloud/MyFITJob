using Microsoft.EntityFrameworkCore;
using MyFITJob.Models;

namespace MyFITJob.DAL;

public class JobOfferRepository : IJobOfferRepository
{
    private readonly MyFITJobContext _context;

    public JobOfferRepository(MyFITJobContext context)
    {
        _context = context;
    }

    public async Task<List<JobOffer>> GetJobOffersAsync(string filter)
    {
        return await _context.JobOffers
            .Where(j => string.IsNullOrEmpty(filter) || j.Title.Contains(filter))
            .ToListAsync();
    }

    public async Task<JobOffer> CreateJobOfferAsync(JobOffer jobOffer)
    {
        _context.JobOffers.Add(jobOffer);
        await _context.SaveChangesAsync();
        return jobOffer;
    }
}