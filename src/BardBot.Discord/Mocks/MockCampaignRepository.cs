using BardBot.Discord.Database;
using BardBot.Discord.Database.Models;

namespace BardBot.Discord.Mocks
{

    internal class MockCampaignRepository(
        DiscordOptions discordOptions
    ) : ICampaignRepository
    {
        private readonly IEnumerable<Campaign> _repo = discordOptions.MockCampaignRepository;

        public IEnumerable<Campaign> Get()
        {
            return _repo;
        }

        public Campaign? Get(Guid campaignId)
        {
            return _repo.FirstOrDefault(c => c.Id == campaignId);
        }

        public Campaign? Get(ulong guildId)
        {
            return _repo.SingleOrDefault(c => c.GuildId == guildId);
        }

    }
}

namespace BardBot.Discord
{

    internal partial record DiscordOptions
    {
        public List<Campaign> MockCampaignRepository { get; init; } = [];
    }

}
