using BardBot.Common;
using BardBot.Discord.Database;
using BardBot.Discord.Exporting;

using Discord.Interactions;

using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Interactions;

[Group(GroupName, "Export resources from Discord")]
[RequireContext(ContextType.Guild)]
[RequireOwner]
public partial class ExportModule(
    DateTimeFactory dateTimeFactory,
    ExportJobFactory exportJobFactory,
    IChatExportHistoryRepository chatExportHistoryRepository,
    ILogger<ExportModule> logger
) : InteractionModuleBase<SocketInteractionContext>
{
    private const string GroupName = "export";
}
