using BardBot.Discord.Exporting;

using Discord.Interactions;

using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Interactions;

[Group(GroupName, "Export resources from Discord")]
[RequireContext(ContextType.Guild)]
[RequireOwner]
public partial class ExportModule(
    ExportJobFactory exportJobFactory,
    ILogger<ExportModule> logger
) : InteractionModuleBase<SocketInteractionContext>
{
    private const string GroupName = "export";
}
