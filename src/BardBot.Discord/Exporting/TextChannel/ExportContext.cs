using BardBot.Discord.Discord;

using Discord;

namespace BardBot.Discord.Exporting.TextChannel;

internal record ExportContext(
    IGuild Guild,
    IMessageChannel Channel,
    ExportFormat Format,
    Snowflake After,
    Snowflake Before
);
