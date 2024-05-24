using Discord;

namespace BardBot.Discord.Exporting.Chat;

internal abstract class MessageWriter(
    IMessageFormatter formatter,
    Stream stream
) : IAsyncDisposable
{

    public virtual ValueTask WritePreambleAsync(CancellationToken cancellationToken = default) =>
        default;

    public virtual ValueTask WriteEpilogueAsync(CancellationToken cancellationToken = default) =>
        default;

    public virtual ValueTask WriteMessageAsync(
        IMessage message,
        CancellationToken cancellationToken = default
    ) => default;

    public virtual async ValueTask DisposeAsync() =>
        await Stream.DisposeAsync();

}
