
using System.Text;

using BardBot.Common.Attributes;
using BardBot.Common.Extensions;

using Discord;

namespace BardBot.Discord.Exporting.Chat;

internal class PlainTextMessageFormatter : IMessageFormatter
{

    ValueTask<string> IMessageFormatter.FormatMessageAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        StringBuilder sb = new();
        sb.AppendLine(MessageHeader(message));
        sb.AppendLine("---");
        sb.AppendLine(message.Content);
        Attachments(message).IfNotNull(attachments => sb.Append(attachments));
        return new(sb.ToString());
    }

    ValueTask<string> IMessageFormatter.FormatPreambleAsync(ExportPreamble preamble, CancellationToken cancellationToken = default) =>
        new(Amble(preamble));


    ValueTask<string> IMessageFormatter.FormatPostambleAsync(ExportPostamble postamble, CancellationToken cancellationToken = default) =>
        new(Amble(postamble));

    ValueTask<string> IMessageFormatter.SpacerAsync(CancellationToken cancellationToken) =>
        new("\n\n");

    private static string AmbleBoundary { get; } = "=".Repeat(80);

    private static string Amble(IContainsOrderedProperties amble)
    {
        var props = amble.OrderedProperties();
        return props.Count > 0
            ? new StringBuilder()
                .AppendLine(AmbleBoundary)
                .AppendJoin('\n', props
                    .Select(property => $"{property.Name}: {property.GetValue(amble)?.ToString() ?? ""}")
                )
                .Append(AmbleBoundary)
                .ToString()
            : string.Empty
            ;
    }

    private static string DateTime__String(DateTimeOffset dto) =>
        DateTime__String(dto.DateTime);

    private static string DateTime__String(DateTime dateTime) =>
        dateTime.ToString("yyyy-MM-dd HH:mm:ss");

    private static string MessageHeader(IMessage message) =>
        $"[{DateTime__String(message.Timestamp)}] {message.Author.Username}";

    private static string? Attachments(IMessage message) =>
        message.Attachments.Count > 0
        ? new StringBuilder()
            .AppendLine()
            .AppendJoin('\n', message.Attachments
                .Select(attachment => $"[Attachment: {attachment.Title} -- {attachment.Url} -- {attachment.Description}]")
            )
            .ToString()
        : null
        ;

}
