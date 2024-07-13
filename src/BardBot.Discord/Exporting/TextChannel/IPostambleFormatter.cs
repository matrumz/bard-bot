namespace BardBot.Discord.Exporting.TextChannel;

internal interface IPostambleFormatter
{

    ValueTask<string> FormatPostambleAsync(ExportPostamble postamble, CancellationToken cancellationToken = default);

}
