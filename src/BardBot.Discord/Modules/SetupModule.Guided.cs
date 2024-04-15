using Discord.Interactions;

using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Modules;

public partial class SetupModule
{
    [SlashCommand("start", "Start the setup wizard")]
    public async Task StartSetupAsync()
    {
        await RespondAsync("Waking the Setup Wizard...", ephemeral: true);
    }
}
