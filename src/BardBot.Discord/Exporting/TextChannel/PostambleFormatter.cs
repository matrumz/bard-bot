namespace BardBot.Discord.Exporting.TextChannel;

internal abstract class PostambleFormatter : IPostambleFormatter
{

    public abstract ValueTask<string> FormatPostambleAsync(ExportPostamble postamble, CancellationToken cancellationToken = default);

}
