using MyFITJob.Api.JobOffers.Domain;

namespace MyFITJob.Api.Infrastructure.Data;

public interface IJobOfferRepository
{
    Task<JobOffer?> GetJobOfferAsync(int id);
    Task<List<JobOffer>> GetJobOffersAsync(string searchTerm);
    Task<JobOffer> CreateJobOfferAsync(JobOffer jobOffer);
    Task<JobOffer> UpdateJobOfferAsync(JobOffer jobOffer);
    Task<List<Skill>> GetSkillsAsync();
    Task AddSkillsAsync(List<Skill> skills);
} 