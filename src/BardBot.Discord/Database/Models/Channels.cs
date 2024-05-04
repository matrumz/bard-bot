namespace BardBot.Discord.Database.Models;

public class Channel : ILabelled
{

    // Required

    public required ulong Id { get; set; }

    public required ulong GuildId { get; set; }

    // Optional

    public string? Character { get; set; }

    // Default-able

    public IEnumerable<Label> Labels { get; set; } = [];

}
