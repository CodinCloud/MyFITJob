using MyFITJob.Api.Kernel.Domain;

namespace MyFITJob.Api.JobOffers.Domain;

public class JobOfferStatus : Enumeration
{
    public static readonly JobOfferStatus New = new("new", "Nouvelle");
    public static readonly JobOfferStatus Applied = new("applied", "Candidature envoyée");
    public static readonly JobOfferStatus Interviewing = new("interviewing", "En entretien");
    public static readonly JobOfferStatus Offered = new("offered", "Offre reçue");
    public static readonly JobOfferStatus Rejected = new("rejected", "Refusée");
    public static readonly JobOfferStatus Accepted = new("accepted", "Acceptée");

    private JobOfferStatus(string name, string displayName) : base(name, displayName)
    {
    }
} 
