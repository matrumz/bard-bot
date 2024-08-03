namespace BardBot.Discord.Exporting.TextChannel;

public interface IPostambleFormatter
{

    ValueTask<string> FormatPostambleAsync(ExportPostamble postamble, CancellationToken cancellationToken = default);

}
