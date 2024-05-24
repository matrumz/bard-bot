using Discord;

namespace BardBot.Discord.Exporting.Chat;

internal interface IMessageFormatter
{

    ValueTask<string> FormatMessageAsync(
        IMessage message,
        CancellationToken cancellationToken = default
    );

}
