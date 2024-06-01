using Discord;

namespace BardBot.Discord.Exporting.Chat;

internal interface IMessageFormatter
{

    ValueTask<string> FormatMessageAsync(
        IMessage message,
        CancellationToken cancellationToken = default
    );

    ValueTask<string> FormatPreambleAsync(
        ExportPreamble preamble,
        CancellationToken cancellationToken = default
    );

    ValueTask<string> FormatPostambleAsync(
        ExportPostamble postamble,
        CancellationToken cancellationToken = default
    );

    ValueTask<string> SpacerAsync(
        CancellationToken cancellationToken = default
    );

}
