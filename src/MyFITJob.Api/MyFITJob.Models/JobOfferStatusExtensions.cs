using System.Reflection;

namespace MyFITJob.Models;

public static class JobOfferStatusExtensions
{
    public static string GetDisplayName(this JobOfferStatus status)
    {
        var field = status.GetType().GetField(status.ToString());
        var attribute = field?.GetCustomAttribute<DisplayNameAttribute>();
        return attribute?.DisplayName ?? status.ToString();
    }

    public static JobOfferStatus FromDisplayName(string displayName)
    {
        foreach (JobOfferStatus status in Enum.GetValues(typeof(JobOfferStatus)))
        {
            if (status.GetDisplayName() == displayName)
            {
                return status;
            }
        }
        throw new ArgumentException($"No status found with display name: {displayName}");
    }
} 