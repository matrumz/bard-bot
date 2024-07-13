
using System.Text;

using BardBot.Common.Extensions;

namespace BardBot.Discord.Exporting.TextChannel.PlainText;

internal class PreambleFormatter : TextChannel.PreambleFormatter, IPreambleFormatter
{

    public override ValueTask<string> FormatPreambleAsync(ExportPreamble preamble, CancellationToken cancellationToken = default)
    {
        StringBuilder sb = new();
        sb.AppendLine(Boundary);
        sb.AppendLine($"Guild: {preamble.Guild}");
        preamble.Category.IfNotNull(category => sb.AppendLine($"Category: {category}"));
        sb.AppendLine($"Channel: {preamble.Channel}");
        preamble.ChannelTopic.IfNotNull(topic => sb.AppendLine($"Topic: {topic}"));
        preamble.Thread.IfNotNull(thread => sb.AppendLine($"Thread: {thread}"));
        sb.AppendLine(Boundary);
        return new(sb.ToString());
    }

    private static string Boundary { get; } = "=".Repeat(80);

}
