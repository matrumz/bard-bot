using System.Reflection;

using BardBot.Common.Attributes;

namespace BardBot.Discord.Exporting.Chat;

internal partial record ExportPreamble(
    string Guild,
    string? Category,
    string Channel,
    string? ChannelTopic,
    string? Thread
);

internal partial record ExportPreamble : IContainsOrderedProperties
{

    public IReadOnlyList<PropertyInfo> OrderedProperties() =>
        // Create an instance of the interface and call the default method
        ((IContainsOrderedProperties)this).OrderedProperties();

    [Order]
    public string Guild { get; init; } = Guild;

    [Order]
    public string? Category { get; init; } = Category;

    [Order]
    public string Channel { get; init; } = Channel;

    [Order]
    public string? ChannelTopic { get; init; } = ChannelTopic;

    [Order]
    public string? Thread { get; init; } = Thread;
}
