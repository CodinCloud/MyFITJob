using MyFITJob.Models;
using MyFITJob.BusinessLogic.DTOs;

namespace MyFITJob.BusinessLogic;

public interface IJobOfferService
{
    Task<IEnumerable<JobOffer>> GetJobOffersAsync(string searchTerm, int? skillId = null);
    Task<JobOffer> CreateJobOfferAsync(CreateJobOfferDto dto);
}
