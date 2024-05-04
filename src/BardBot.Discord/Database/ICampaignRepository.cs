using BardBot.Discord.Database.Models;

namespace BardBot.Discord.Database;

public interface ICampaignRepository
{
    public IEnumerable<Campaign> Get();

    public Campaign? Get(Guid campaignId);

    public Campaign? Get(ulong guildId);
}
