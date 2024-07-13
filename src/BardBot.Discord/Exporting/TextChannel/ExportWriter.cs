using Discord;

namespace BardBot.Discord.Exporting.TextChannel;

internal sealed class ExportWriter(
    IMessageFormatter messageFormatter,
    IPostambleFormatter postambleFormatter,
    IPreambleFormatter preambleFormatter,
    TextWriter writer
) : IAsyncDisposable
{

    public async ValueTask WritePreambleAsync(ExportPreamble preamble, CancellationToken ct = default) =>
        await writer.WriteAsync(await preambleFormatter.FormatPreambleAsync(preamble, ct));

    public async ValueTask WritePostambleAsync(ExportPostamble postamble, CancellationToken ct = default) =>
        await writer.WriteAsync(await postambleFormatter.FormatPostambleAsync(postamble, ct));

    public async ValueTask WriteMessageAsync(IMessage message, CancellationToken ct = default) =>
        await writer.WriteAsync(await messageFormatter.FormatMessageAsync(message, ct));

    public async ValueTask DisposeAsync() =>
        await writer.DisposeAsync();

}
