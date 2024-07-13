namespace BardBot.Discord.Exporting.TextChannel;

internal interface IPreambleFormatter
{

    ValueTask<string> FormatPreambleAsync(ExportPreamble preamble, CancellationToken cancellationToken = default);

}
