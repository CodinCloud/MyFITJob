using MyFITJob.Api.JobOffers.Domain;
using MyFITJob.Api.JobOffers.DTOs;

namespace MyFITJob.Api.JobOffers.Application;

public interface IJobOfferService
{
    Task<List<JobOffer>> GetJobOffersAsync(string searchTerm);
    Task<JobOffer> CreateJobOfferAsync(CreateJobOfferDto dto);
} 