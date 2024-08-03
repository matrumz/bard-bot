using Microsoft.Extensions.DependencyInjection;

namespace BardBot.Discord.Exporting.TextChannel;

public sealed class ExportWriterFactory(
    IServiceProvider serviceProvider
)
{

    public ExportWriter GetExportWriter(
        ExportFormat format,
        FileInfo file
    )
        => GetExportWriter(
            format: format,
            writer: new StreamWriter(file.FullName, new FileStreamOptions()
            {
                Mode = FileMode.Create,
                Share = FileShare.Read,
            })
        );

    public ExportWriter GetExportWriter(
        ExportFormat format,
        TextWriter writer
    ) =>
        format switch
        {

            ExportFormat.PlainText => new ExportWriter(
                messageFormatter: ActivatorUtilities.CreateInstance<PlainText.IMessageFormatter>(serviceProvider),
                postambleFormatter: ActivatorUtilities.CreateInstance<PlainText.IPostambleFormatter>(serviceProvider),
                preambleFormatter: ActivatorUtilities.CreateInstance<PlainText.IPreambleFormatter>(serviceProvider),
                writer: writer
            ),

            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };

}
