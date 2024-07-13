namespace BardBot.Discord.Exporting.TextChannel;

internal abstract class PreambleFormatter : IPreambleFormatter
{

    public abstract ValueTask<string> FormatPreambleAsync(ExportPreamble preamble, CancellationToken cancellationToken = default);

}
