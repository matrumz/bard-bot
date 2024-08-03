using System.ComponentModel.DataAnnotations;

namespace BardBot.Discord;

internal partial record DiscordOptions
{
    public BotConfiguration Bot { get; init; } = new();

    public record BotConfiguration
    {
        [Required(AllowEmptyStrings = false)]
        public string Token { get; init; } = string.Empty;
    }
}
