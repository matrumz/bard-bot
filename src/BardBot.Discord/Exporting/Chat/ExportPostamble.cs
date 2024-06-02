using System.Reflection;

using BardBot.Common.Attributes;

namespace BardBot.Discord.Exporting.Chat;

internal partial record ExportPostamble();

internal partial record ExportPostamble : IContainsOrderedProperties
{

    public IReadOnlyList<PropertyInfo> OrderedProperties() =>
        // Create an instance of the interface and call the default method
        ((IContainsOrderedProperties)this).OrderedProperties();

}
