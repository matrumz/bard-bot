using Discord.Interactions;

using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Modules;

[Group("setup-wizard", "Configure this bot for your server")]
[RequireContext(ContextType.Guild)]
[RequireOwner]
public partial class SetupModule : InteractionModuleBase<SocketInteractionContext>
{
    private ILogger<SetupModule> Logger { get; init; }

    public SetupModule(
        ILogger<SetupModule> logger
    )
    {
        Logger = logger;
    }

}
