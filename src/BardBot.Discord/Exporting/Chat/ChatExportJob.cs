using BardBot.Common.Extensions;
using BardBot.Discord.Database;
using BardBot.Discord.Database.Models;
using BardBot.Discord.Exporting.PathTokens;

namespace BardBot.Discord.Exporting.Chat;

public sealed class ChatExportJob(
    ICampaignRepository campaignRepository
)
{
    public required ChatExportContext Context { get; set; }

    private Campaign? Campaign { get => campaignRepository.Get(Context.Guild.Id); }

    public async Task<IEnumerable<FileInfo>> ToFiles()
    {
        var files = Campaign?
            .Channels
            .AsParallel()
            .Where(channel => channel.Labels.Any(label => label.Name == "game-chat"))
            .CrossJoin(Context.Ranges)
            .Select(((Channel channel, AfterBeforeDate range) tuple) =>
            {
                const string path = "/ttrpg/campaigns/are-we-excited/sessions/{before,:yyyy-MM-dd-HHmm}__{after,:yyyy-MM-dd-HHmm}/transcripts/{character,=common}.txt";

                var tokens = new List<Token>();
                tuple.channel.Character.IfNotNull(character => tokens.Add(Token.Character(character)));
                tuple.range.After.IfNotNull(after => tokens.Add(Token.After(after)));
                tuple.range.Before.IfNotNull(before => tokens.Add(Token.Before(before)));

                var file = new FileInfo(path.ApplyTokens(tokens));

                return Task.FromResult(file);
            })
            ?? [];

        return await Task.WhenAll(files);
    }

}
