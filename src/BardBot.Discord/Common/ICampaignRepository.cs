namespace BardBot.Discord.Common;

public interface ICampaignRepository
{
    public IEnumerable<Campaign> Get();

    public Campaign? Get(Guid campaignId);

    public Campaign? Get(ulong guildId);
}
