using Discord.Interactions;
using Discord.Interactions.Builders;

namespace BardBot.Discord.InteractionModules;

public partial class ExportModule
{
    [SlashCommand("chat", "Configured channels & threads.")]
    public async Task ExportChatAsync()
    {
    }
}
