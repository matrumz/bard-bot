using BardBot.Discord.Discord.Extensions;

using Discord;

namespace BardBot.Discord.Exporting.TextChannel;

internal sealed class ExportJob(
    ExportContext context,
    ExportWriterFactory writerFactory
)
{
    public Progress<int> Progress { get; } = new();

    public async Task ExportAsync(CancellationToken ct = default)
    {
        var messages = context.Channel.GetMessagesAsync(
            context.After,
            context.Before,
            batchSize: 100,
            options: new RequestOptions
            {
                CancelToken = ct,
                RetryMode = RetryMode.AlwaysRetry
            }
        );

        await using var writer = writerFactory.GetExportWriter(
            format: context.Format,
            localFilePath: context.Path
        );

        await writer.WritePreambleAsync(new ExportPreamble(
            Guild: context.Guild.Name,
            Category: context.Channel.GetCategory(),
            Channel: context.Channel,
            // ChannelTopic: context.Channel.Topic,
            // Thread:
        ), ct);

        await foreach (var message in messages.WithCancellation(ct))
        {
            await writer.WriteMessageAsync(message, ct);
        }

        await writer.WritePostambleAsync(new ExportPostamble(), ct);
    }

}
