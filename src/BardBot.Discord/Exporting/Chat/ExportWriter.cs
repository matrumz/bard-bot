using Discord;

namespace BardBot.Discord.Exporting.Chat;

internal sealed class ExportWriter(
    IMessageFormatter formatter,
    TextWriter writer
) : IAsyncDisposable
{

    public async ValueTask WritePreambleAsync(ExportPreamble preamble, CancellationToken ct = default) =>
        await writer.WriteAsync(await formatter.FormatPreambleAsync(preamble, ct));

    public async ValueTask WritePostambleAsync(ExportPostamble postamble, CancellationToken ct = default) =>
        await writer.WriteAsync(await formatter.FormatPostambleAsync(postamble, ct));

    public async ValueTask WriteMessageAsync(IMessage message, CancellationToken ct = default) =>
        await writer.WriteAsync(await formatter.FormatMessageAsync(message, ct));

    public async ValueTask DisposeAsync() =>
        await writer.DisposeAsync();

}
