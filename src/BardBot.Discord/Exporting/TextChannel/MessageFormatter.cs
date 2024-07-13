
using Discord;

namespace BardBot.Discord.Exporting.TextChannel;

internal abstract class MessageFormatter : IMessageFormatter
{

    public abstract ValueTask<string> FormatMessageAsync(IMessage message, CancellationToken cancellationToken = default);

    public abstract string Spacer { get; }

}

