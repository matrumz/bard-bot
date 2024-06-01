
using System.Text;

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
    }

    ValueTask<string> IMessageFormatter.FormatPreambleAsync(ExportPreamble preamble, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    ValueTask<string> IMessageFormatter.FormatPostambleAsync(ExportPostamble postamble, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    ValueTask<string> IMessageFormatter.SpacerAsync(CancellationToken cancellationToken) =>
        new("\n\n");

    private static string DateTime__String(DateTimeOffset dto) =>
        DateTime__String(dto.DateTime);

    private static string DateTime__String(DateTime dateTime) =>
        dateTime.ToString("yyyy-MM-dd HH:mm:ss");

    private static string MessageHeader(IMessage message) =>
        $"[{DateTime__String(message.Timestamp)}] {message.Author.Username}";

    private static string

}
