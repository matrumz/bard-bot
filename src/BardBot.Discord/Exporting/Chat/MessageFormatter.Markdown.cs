
using Discord;

namespace BardBot.Discord.Exporting.Chat;

internal class MarkdownMessageFormatter : IMessageFormatter
{

    ValueTask<string> IMessageFormatter.FormatMessageAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

}
