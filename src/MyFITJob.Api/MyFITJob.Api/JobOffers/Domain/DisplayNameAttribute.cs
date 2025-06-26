namespace MyFITJob.Api.JobOffers.Domain;

[AttributeUsage(AttributeTargets.Field)]
public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; }

    public DisplayNameAttribute(string displayName)
    {
        DisplayName = displayName;
    }
} 
