using BardBot.Discord.Discord;

using Discord;

namespace BardBot.Discord.Exporting.TextChannel;

public partial record ExportContext(
    IGuild Guild,
    IMessageChannel Channel,
    ExportFormat Format,
    Snowflake After,
    Snowflake Before,
    FileInfo File
)
{

    public IProgress<int>? Progress { get; init; }

};
