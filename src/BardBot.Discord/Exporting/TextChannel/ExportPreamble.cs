namespace BardBot.Discord.Exporting.TextChannel;

internal record ExportPreamble(
    /// <summary> The name of the guild. </summary>
    string Guild,
    /// <summary> The name of the category. </summary>
    string? Category,
    /// <summary> The name of the channel. </summary>
    string Channel,
    /// The topic of the channel. (description, usage, rules, etc.) </summary>
    string? ChannelTopic,
    /// <summary> The name of the thread. </summary>
    string? Thread
);
