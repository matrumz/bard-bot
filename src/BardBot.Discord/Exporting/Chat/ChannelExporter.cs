namespace BardBot.Discord.Exporting.Chat;

internal sealed class ChannelExporter
{

    private

    public async Task<FileInfo> ExportChannelAsync(
        ChatExportContext context,
        IProgress<int>? progress = default,
        CancellationToken cancellationToken = default
    )
    {
        private ExportWriter? _writer;
    private ExportWriter Writer
    {
        get =>
            _writer ??=
        }
}

}
