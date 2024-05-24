namespace BardBot.Discord.Exporting.Chat;

internal record ExportPreamble(
    string Guild,
    string? Category,
    string Channel,
    string? ChannelTopic,
    string? Thread
);
