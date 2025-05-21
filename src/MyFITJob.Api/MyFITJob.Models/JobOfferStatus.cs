namespace MyFITJob.Models;

using System.ComponentModel;

public class JobOfferStatus : Enumeration
{
    public static readonly JobOfferStatus New = new("new", "Nouvelle");
    public static readonly JobOfferStatus Saved = new("saved", "Sauvegardée");
    public static readonly JobOfferStatus Applied = new("applied", "Candidature envoyée");
    public static readonly JobOfferStatus InterviewPlanned = new("interview_planned", "Entretien planifié");
    public static readonly JobOfferStatus Interviewed = new("interviewed", "Entretien réalisé");
    public static readonly JobOfferStatus OfferReceived = new("offer_received", "Offre reçue");
    public static readonly JobOfferStatus Accepted = new("accepted", "Acceptée");
    public static readonly JobOfferStatus Rejected = new("rejected", "Refusée");
    public static readonly JobOfferStatus Archived = new("archived", "Archivée");

    private JobOfferStatus(string name, string displayName) : base(name, displayName)
    {
    }
} 