using Discord.Interactions;
using Discord.Interactions.Builders;

namespace BardBot.Discord.Interactions;

public partial class ExportModule
{
    [SlashCommand("chat", "Configured channels & threads.")]
    public async Task ExportChatAsync()
    {
    }
}
