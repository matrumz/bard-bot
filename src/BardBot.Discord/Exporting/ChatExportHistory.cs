namespace BardBot.Discord.Exporting;

public class ChatExportHistory
{
    // Required

    public required ulong ChannelId { get; set; }

    public required ulong GuildId { get; set; }

    public required DateTime Timestamp { get; set; }

    public required ulong TriggeredByUser { get; set; }

    // Optional

    public DateTime? After { get; set; }

    public DateTime? Before { get; set; }

    public string? FilePath { get; set; }

    // Internal

    public Guid _id { get; set; }

}
