using MyFITJob.Api.JobOffers.Domain;
using MyFITJob.Api.JobOffers.DTOs;

namespace MyFITJob.Api.JobOffers.Application;

public interface IJobOfferService
{
    Task<List<JobOfferDto>> GetJobOffersAsync(string searchTerm);
    Task<JobOfferDto> CreateJobOfferAsync(CreateJobOfferDto dto);
} 