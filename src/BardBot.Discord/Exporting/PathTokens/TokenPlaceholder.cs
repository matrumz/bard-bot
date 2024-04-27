namespace BardBot.Discord.Exporting.PathTokens;

public sealed record TokenPlaceholder(
    /// <summary> The default value of the placeholder if no suitable token found. </summary>
    string DefaultValue,
    /// <summary> The format to apply to the substituted value. </summary>
    string? Format,
    /// <summary> The full match of the placeholder </summary>
    string Match,
    /// <summary> The id of the token to substitute. </summary>
    string TokenId,
    /// <summary> The flags to apply to the placeholder. </summary>
    TokenPlaceholderFlags Flags = TokenPlaceholderFlags.None
)
{
    public const char DefaultIndicator = '=';
    public const char FormatIndicator = ':';
}
