namespace BardBot.Discord.Database.Models;

public class Campaign
{
    // Required

    public required Guid Id { get; set; }

    // Default-able

    public IEnumerable<Channel> Channels { get; set; } = [];

}
