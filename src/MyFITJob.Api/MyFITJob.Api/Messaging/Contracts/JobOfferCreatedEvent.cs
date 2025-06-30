namespace MyFITJob.Api.Messaging.Contracts;

public record JobOfferCreatedEvent(int JobOfferId, string CompanyName, string Industry, string Size);
