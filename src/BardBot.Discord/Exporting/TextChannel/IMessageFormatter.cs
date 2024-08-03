using Discord;

namespace BardBot.Discord.Exporting.TextChannel;

public interface IMessageFormatter
{

    ValueTask<string> FormatMessageAsync(IMessage message, CancellationToken cancellationToken = default);

    string Spacer { get; }

}
