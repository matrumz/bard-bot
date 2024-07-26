using BardBot.Common;
using BardBot.Discord.Database;
using BardBot.Discord.Exporting;

using Discord.Interactions;

using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Exporting;

[Group(GroupName, "Export resources from Discord")]
[RequireContext(ContextType.Guild)]
[RequireOwner]
public partial class InteractionGroup(
    DateTimeFactory dateTimeFactory,
    ICampaignRepository campaignRepository,
    IChatExportHistoryRepository chatExportHistoryRepository,
    ILogger<InteractionGroup> logger
) : InteractionModuleBase<SocketInteractionContext>
{
    private const string GroupName = "export";
}
