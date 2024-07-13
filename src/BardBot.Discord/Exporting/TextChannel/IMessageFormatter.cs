using Discord;

namespace BardBot.Discord.Exporting.TextChannel;

internal interface IMessageFormatter
{

    ValueTask<string> FormatMessageAsync(IMessage message, CancellationToken cancellationToken = default);

    string Spacer { get; }

}
