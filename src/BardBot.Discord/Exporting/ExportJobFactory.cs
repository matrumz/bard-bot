using BardBot.Discord.Exporting.Chat;

using Microsoft.Extensions.DependencyInjection;

namespace BardBot.Discord.Exporting;

public sealed class ExportJobFactory(
    IServiceProvider services
)
{
    public ChannelExporter CreateChatExportJob() =>
        services.GetRequiredService<ChannelExporter>();
}
