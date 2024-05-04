using System.ComponentModel.DataAnnotations;

namespace BardBot.Discord.Database.Models;

public class ChatExportHistory
{
    [Required]
    public DateTime Timestamp { get; set; }

    public ulong TriggeredByUser { get; set; }

    public DateTime? After { get; set; }

    public DateTime? Before { get; set; }

    [Required]
    public ulong GuildId { get; set; }

    [Required]
    public ulong ChannelId { get; set; }

    public string? FilePath { get; set; }

}
