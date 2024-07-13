using System.Text;

using Discord;

using BardBot.Common.Extensions;

namespace BardBot.Discord.Exporting.TextChannel.PlainText;

internal class MessageFormatter : TextChannel.MessageFormatter, IMessageFormatter
{

    public override ValueTask<string> FormatMessageAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        StringBuilder sb = new();
        sb.AppendLine(message.Header());
        sb.AppendLine("---");
        sb.AppendLine(message.Content);
        message.Attachments().IfNotNull(attachments => sb.Append(attachments));
        return new(sb.ToString());
    }

    public override string Spacer { get; } = Environment.NewLine;

}

internal static class Extensions
{

    public static string Header(this IMessage message) =>
        $"[{message.Timestamp}] {message.Author.Username}";

    public static string? Attachments(this IMessage message) =>
        message.Attachments.Count > 0
            ? new StringBuilder()
                .AppendLine()
                .AppendJoin('\n', message.Attachments
                    .Select(attachment => $"[Attachment: {attachment.Title} -- {attachment.Url} -- {attachment.Description}]")
                )
                .ToString()
            : null
        ;

    public static string ToString(this DateTimeOffset dto) =>
        dto.DateTime.ToString();

    public static string ToString(this DateTime dt) =>
        dt.ToString("yyyy-MM-dd HH:mm:ss");

}
