using Discord.Interactions;

using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Setup;

[Group("setup-wizard", "Configure this bot for your server")]
[RequireContext(ContextType.Guild)]
[RequireOwner]
public partial class InteractionGroup : InteractionModuleBase<SocketInteractionContext>
{
    private ILogger<InteractionGroup> Logger { get; init; }

    public InteractionGroup(
        ILogger<InteractionGroup> logger
    )
    {
        Logger = logger;
    }

}
