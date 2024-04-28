using Discord.Interactions;

using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Interactions;

public partial class SetupModule
{
    [SlashCommand("start", "Start the setup wizard")]
    public async Task StartSetupAsync()
    {
        await RespondAsync("Waking the Setup Wizard...", ephemeral: true);
    }
}
