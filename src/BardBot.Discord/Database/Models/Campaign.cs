using BardBot.Common.Models;

namespace BardBot.Discord.Database.Models;

public class Campaign
{
    // Required

    public required Guid Id { get; set; }

    public required ulong GuildId { get; set; }

    // Optional

    public Dictionary<ulong, Channel> Channels { get; set; } = [];

    public ExportPathTemplates? ExportPathTemplates { get; set; }

    public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Utc;

}

public class Channel : ILabelled
{
    // Required

    public required ulong Id { get; set; }

    public required ulong GuildId { get; set; }

    // Optional

    // TODO
    // public bool? AutoExportThreads { get; set; }

    public string? Character { get; set; }

    public bool? ExportableGameChat { get; set; }

    public IEnumerable<Label> Labels { get; set; } = [];

}

public class ExportPathTemplates
{

    public string? Chats { get; set; }

    public string? ChatMedia { get; set; }

}
