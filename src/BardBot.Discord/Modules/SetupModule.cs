using Discord.Interactions;

using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Modules;

public class SetupModule : InteractionModuleBase
{
    private ILogger<SetupModule> Logger { get; init; }

    public SetupModule(
        ILogger<SetupModule> logger
    )
    {
        Logger = logger;
    }

    [SlashCommand("setup", "Set up the bot")]
    public Task SetupAsync()
    {
        Logger.LogInformation("Setup command received in guid {Guid}", Context.Guild);
        return Task.CompletedTask;
    }
}
