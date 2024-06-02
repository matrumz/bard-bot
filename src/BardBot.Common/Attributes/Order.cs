using System.Reflection;
using System.Runtime.CompilerServices;

namespace BardBot.Common.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class OrderAttribute(
    [CallerFilePath] string file = "",
    [CallerLineNumber] int line = 0
) : Attribute
{
    public string File { get => file; }
    public int Order { get => line; }
}

public interface IContainsOrderedProperties
{
    public IReadOnlyList<PropertyInfo> OrderedProperties() =>
        GetType().GetProperties()
            .Where(property => Attribute.IsDefined(property, typeof(OrderAttribute)))
            .Select(property => (property, attribute: (OrderAttribute)Attribute.GetCustomAttribute(property, typeof(OrderAttribute))!))
            .OrderBy(pa => pa.attribute.File)
            .ThenBy(pa => pa.attribute.Order)
            .Select(pa => pa.property)
            .ToArray()
        ;
}
