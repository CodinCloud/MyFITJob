using MyFITJob.Api.JobOffers.Domain;

namespace MyFITJob.Api.Infrastructure.Data;

public interface IJobOfferRepository
{
    Task<List<JobOffer>> GetJobOffersAsync(string searchTerm);
    Task<JobOffer> CreateJobOfferAsync(JobOffer jobOffer);
    Task<List<Skill>> GetSkillsAsync();
    Task AddSkillsAsync(List<Skill> skills);
} 