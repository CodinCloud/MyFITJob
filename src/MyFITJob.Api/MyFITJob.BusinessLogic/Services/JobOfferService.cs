using MyFITJob.DAL;
using MyFITJob.Models;

namespace MyFITJob.BusinessLogic;

public class JobOfferService : IJobOfferService
{
    private readonly IJobOfferRepository _repository;

    public JobOfferService(IJobOfferRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<JobOffer>> GetJobOffersAsync(string filter)
    {
        return await _repository.GetJobOffersAsync(filter);
    }
}