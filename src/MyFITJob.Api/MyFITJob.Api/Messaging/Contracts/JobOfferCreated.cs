namespace MyFITJob.Api.Messaging.Contracts;

public record JobOfferCreated(int JobOfferId, string CompanyName, string Industry, string Size);
