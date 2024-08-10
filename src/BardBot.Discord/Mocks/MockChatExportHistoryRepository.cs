using BardBot.Discord.Exporting;

namespace BardBot.Discord.Mocks
{
    using BardBot.Discord.Common;

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

namespace BardBot.Discord.Common
{

    internal partial record DiscordOptions
    {
        public List<ChatExportHistory> MockChatExportHistoryRepository { get; init; } = [];
    }

}
