using BardBot.Discord.Database.Models;

namespace BardBot.Discord.Database;

public interface IChatExportHistoryRepository
{

    public IEnumerable<ChatExportHistory>? Get(ulong guildId);

}
