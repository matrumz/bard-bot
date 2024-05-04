using BardBot.Discord.Database.Models;

namespace BardBot.Discord.Database;

internal class MockChatExportHistoryRepository : IChatExportHistoryRepository
{
    private IEnumerable<ChatExportHistory> _chatExportHistories = [
    ];

    public IEnumerable<ChatExportHistory>? Get(ulong guildId)
    {
        return _chatExportHistories.Where(c => c.GuildId == guildId);
    }

}
