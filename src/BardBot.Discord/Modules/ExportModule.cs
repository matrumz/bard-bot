using BardBot.Discord.Hosting;

using Discord.Interactions;

using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Modules;

[Group("export", "Export resources from Discord")]
[RequireContext(ContextType.Guild)]
[RequireOwner]
public partial class ExportModule(
    IChannelExporter channelExporter,
    ILogger<ExportModule> logger
) : InteractionModuleBase<SocketInteractionContext>
{
}
