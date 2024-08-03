using BardBot.Discord.Discord.Extensions;

using Discord;

using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Exporting.TextChannel;

public sealed class ExportJob(
    ExportContext context,
    ExportWriterFactory writerFactory,
    ILogger<ExportJob> logger
)
{

    public async Task ExportAsync(CancellationToken ct = default)
    {
        logger.BeginScope(new Dictionary<string, object?>
        {
            ["File"] = context.File.FullName,
        });
        logger.LogDebug("Beginning {Format} export",
            context.Format.ToString()
        );

        // Start fetching messages early/asynchronously
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

        // Get a writer
        await using var writer = writerFactory.GetExportWriter(
            format: context.Format,
            file: context.File
        );

        ExportPreamble preamble = new(
            Guild: context.Guild.Name,
            Category: context.Channel.GetCategory()?.Name,
            Channel: context.Channel.GetSelfChannelOrParentChannel().Name,
            ChannelTopic: context.Channel.GetTopic(),
            Thread: context.Channel.GetThread()?.Name
        );

        logger.BeginScope(new Dictionary<string, object?>
        {
            ["Guild"] = preamble.Guild,
            ["Category"] = preamble.Category,
            ["Channel"] = preamble.Channel,
            ["Thread"] = preamble.Thread
        });

        logger.LogInformation("Exporting");

        // Begin writing with the preamble
        await writer.WritePreambleAsync(preamble, ct);

        // Write messages
        var messagesWritten = 0;
        await foreach (var message in messages.WithCancellation(ct))
        {
            await writer.WriteMessageAsync(message, ct);

            context.Progress?.Report(++messagesWritten);

            if (messagesWritten % 10 == 0)
                logger.LogDebug("Exported {Count} messages...", messagesWritten);
        }
        logger.LogInformation("Exported {Count} messages", messagesWritten);

        // Finish writing with the postamble
        await writer.WritePostambleAsync(new ExportPostamble(), ct);

        logger.LogInformation("Export complete");
    }

}
