using MyFITJob.Models;

namespace MyFITJob.BusinessLogic;

public interface IJobOfferService
{
    Task<IEnumerable<JobOffer>> GetJobOffersAsync(string searchTerm, int? skillId = null);
}
