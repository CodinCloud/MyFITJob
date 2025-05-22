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
}