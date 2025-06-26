using System.Reflection;

namespace MyFITJob.Api.Kernel.Domain;

public abstract class Enumeration : IComparable
{
    public string Name { get; private set; }
    public string DisplayName { get; private set; }

    protected Enumeration(string name, string displayName)
    {
        Name = name;
        DisplayName = displayName;
    }

    public override string ToString() => DisplayName;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    public override bool Equals(object obj)
    {
        if (obj is not Enumeration otherValue)
            return false;

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Name.Equals(otherValue.Name);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode() => Name.GetHashCode();

    public static T FromName<T>(string name) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(item => item.Name == name);
        if (matchingItem == null)
            throw new InvalidOperationException($"'{name}' is not a valid {typeof(T).Name}");

        return matchingItem;
    }

    public int CompareTo(object other) => Name.CompareTo(((Enumeration)other).Name);
} 
