
namespace BardBot.Discord.Exporting.TextChannel.PlainText;

internal class PostambleFormatter : TextChannel.PostambleFormatter, IPostambleFormatter
{

    public override ValueTask<string> FormatPostambleAsync(ExportPostamble postamble, CancellationToken cancellationToken = default)
        => new(string.Empty);

}
