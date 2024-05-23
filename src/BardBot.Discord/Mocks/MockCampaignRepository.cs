using BardBot.Discord.Database.Models;

namespace BardBot.Discord.Database;

internal class MockCampaignRepository : ICampaignRepository
{
    private readonly List<Campaign> _campaigns = [
    ];

    public IEnumerable<Campaign> Get()
    {
        return _campaigns;
    }

    public Campaign? Get(Guid campaignId)
    {
        return _campaigns.FirstOrDefault(c => c.Id == campaignId);
    }

    public Campaign? Get(ulong guildId)
    {
        return _campaigns.SingleOrDefault(c => c.GuildId == guildId);
    }

}
