using MyFITJob.Models;

namespace MyFITJob.DAL;

public interface IJobOfferRepository
{
    Task<List<JobOffer>> GetJobOffersAsync(string filter);
}