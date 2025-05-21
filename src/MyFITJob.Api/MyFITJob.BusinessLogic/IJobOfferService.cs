using MyFITJob.Models;

namespace MyFITJob.BusinessLogic;

public interface IJobOfferService
{
    Task<List<JobOffer>> GetJobOffersAsync(string filter);
}
