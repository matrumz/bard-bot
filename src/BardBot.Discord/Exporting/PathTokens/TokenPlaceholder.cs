namespace BardBot.Discord.Exporting.PathTokens;

internal sealed record TokenPlaceholder(
    string DefaultValue,
    string? Format,
    string Match,
    string TokenId
);
