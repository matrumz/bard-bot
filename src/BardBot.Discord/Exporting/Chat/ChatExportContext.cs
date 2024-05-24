using BardBot.Discord.Discord;

using Discord;

namespace BardBot.Discord.Exporting.Chat;

public record ChatExportContext(
    IGuild Guild,
    IMessageChannel Channel,
    string OutputPath,
    ChatExportFormat Format,
    Snowflake After,
    Snowflake Before
);
