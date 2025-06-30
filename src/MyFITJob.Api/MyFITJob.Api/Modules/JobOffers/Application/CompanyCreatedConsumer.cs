using MassTransit;
using MyFITJob.Api.Infrastructure.Data;
using MyFITJob.Api.Messaging.Contracts;

namespace MyFITJob.Api.JobOffers.Application;

public class CompanyCreatedConsumer : IConsumer<CompanyCreatedEvent>
{
    private readonly ILogger<CompanyCreatedConsumer> _logger;
    private readonly IJobOfferRepository _jobOfferRepository;

    public CompanyCreatedConsumer(IJobOfferRepository jobOfferRepository, ILogger<CompanyCreatedConsumer> logger)
    {
        _jobOfferRepository = jobOfferRepository;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<CompanyCreatedEvent> context)
    {
        var jobOffer = await _jobOfferRepository.GetJobOfferAsync(context.Message.JobOfferId);

        if (jobOffer == null)
            throw new NullReferenceException($"JobOffer {context.Message.JobOfferId} doesn't exists");
        
        jobOffer!.CompanyId = context.Message.CompanyId;

        await _jobOfferRepository.UpdateJobOfferAsync(jobOffer);

        _logger.LogInformation($"Company updated: {jobOffer.CompanyId}");
    }
}