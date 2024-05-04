using BardBot.Discord.Exporting.Chat;

using Discord;

using Microsoft.Extensions.DependencyInjection;

namespace BardBot.Discord.Exporting;

public sealed class ExportJobFactory(
    IServiceProvider services
)
{
    public ChatExportJob CreateChatExportJob(IGuild guild, params AfterBeforeDate[] ranges) =>
        CreateChatExportJob(guild, ranges.AsEnumerable());

    public ChatExportJob CreateChatExportJob(IGuild guild, IEnumerable<AfterBeforeDate> ranges)
    {
        var job = services.GetRequiredService<ChatExportJob>();
        job.Context = new ChatExportContext(Guild: guild, Ranges: ranges);
        return job;
    }
}
