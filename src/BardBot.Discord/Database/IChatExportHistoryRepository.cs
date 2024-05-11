using BardBot.Discord.Database.Models;

namespace BardBot.Discord.Database;

/// <summary>
/// Standard interface for a repository of chat export history.
/// </summary>
public partial interface IChatExportHistoryRepository
{

    public IEnumerable<ChatExportHistory>? Get(ulong guildId);

}

// Helper methods for the chat export history repository.
public partial interface IChatExportHistoryRepository
{

    public DateTime? LastChatExport(ulong guildId) => Get(guildId)?.OrderByDescending(history => history.Before)?.FirstOrDefault()?.Before;

}
