namespace MyFITJob.Models;

[AttributeUsage(AttributeTargets.Field)]
public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; }

    public DisplayNameAttribute(string displayName)
    {
        DisplayName = displayName;
    }
} 