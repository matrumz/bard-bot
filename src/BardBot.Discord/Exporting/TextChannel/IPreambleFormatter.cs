namespace BardBot.Discord.Exporting.TextChannel;

public interface IPreambleFormatter
{

    ValueTask<string> FormatPreambleAsync(ExportPreamble preamble, CancellationToken cancellationToken = default);

}
