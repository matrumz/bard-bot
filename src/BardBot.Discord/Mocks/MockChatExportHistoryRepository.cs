using BardBot.Discord.Database;
using BardBot.Discord.Database.Models;

namespace BardBot.Discord.Mocks
{

    internal class MockChatExportHistoryRepository(
        DiscordOptions discordOptions
    ) : IChatExportHistoryRepository
    {
        private readonly IEnumerable<ChatExportHistory> _repo = discordOptions.MockChatExportHistoryRepository;

        public IEnumerable<ChatExportHistory>? Get(ulong guildId)
        {
            return _repo.Where(c => c.GuildId == guildId);
        }

    }

}

namespace BardBot.Discord
{

    internal partial record DiscordOptions
    {
        public List<ChatExportHistory> MockChatExportHistoryRepository { get; init; } = [];
    }

}
