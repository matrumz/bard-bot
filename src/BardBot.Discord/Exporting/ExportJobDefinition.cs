using BardBot.Discord.Discord;

using Discord;

namespace BardBot.Discord.Exporting;

internal partial record ExportJobDefinition(
    IGuild Guild,
    IChannel Channel,
    Snowflake? Before,
    Snowflake? After
);
