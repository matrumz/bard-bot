using BardBot.Discord.Database.Models;

namespace BardBot.Discord.Database;

public partial interface IChatExportHistoryRepository
{

    public IEnumerable<ChatExportHistory>? Get(ulong guildId);

}

public partial interface IChatExportHistoryRepository
{

    public DateTime? LastChatExport(ulong guildId) => Get(guildId)?.OrderByDescending(history => history.Before)?.FirstOrDefault()?.Before;

}
