using Discord.Interactions;

namespace BardBot.Discord.Setup;

public partial class InteractionGroup
{
    [SlashCommand("start", "Start the setup wizard")]
    public async Task StartSetupAsync()
    {
        await RespondAsync("Waking the Setup Wizard...", ephemeral: true);
    }
}
