using System.Text.RegularExpressions;

namespace BardBot.Discord.Exporting.PathTokens;

internal sealed partial record TokenPlaceholder(
    /// <summary> The id of the token to substitute. </summary>
    string TokenId,
    /// <summary> The format to apply to the substituted value. </summary>
    string? Format,
    /// <summary> The default value of the placeholder if no suitable token found. </summary>
    string DefaultValue,
    /// <summary> The full match of the placeholder </summary>
    string Match,
    /// <summary> The flags to apply to the placeholder. </summary>
    TokenPlaceholderFlags Flags = TokenPlaceholderFlags.None
)
{
    public const char DefaultIndicator = '=';
    public const char Delimiter = ',';
    public const char Escape = '%';
    public const char FormatIndicator = ':';

    [GeneratedRegex(@"(?<match>\{(?<contents>[^}]+)\})")]
    public static partial Regex Regex();
    [GeneratedRegex(@"(?<!%),")]
    public static partial Regex UnescapedDelimiterRegex();
}

internal static partial class StringExtensions
{
    internal static IEnumerable<TokenPlaceholder> GetTokenPlaceholders(this string template) =>
        TokenPlaceholder.Regex()
            .Matches(template)
            .Select(match => (match: match.Groups["match"].Value, contents: match.Groups["contents"].Value))
            .Select(((string match, string contents) tuple) => (
                tuple.match,
                TokenPlaceholder.UnescapedDelimiterRegex()
                    .Split(tuple.contents)
                    // Remove empty elements
                    .Where(part => !string.IsNullOrEmpty(part))
                    // Unescape the delimiter
                    .Select(part => part.Replace($"{TokenPlaceholder.Escape}{TokenPlaceholder.Delimiter}", TokenPlaceholder.Delimiter.ToString()))
                )
            )
            .Select(((string match, IEnumerable<string> parts) tuple) =>
                new TokenPlaceholder(
                    TokenId: tuple.parts.First(),
                    Format: tuple.parts
                        .FirstOrDefault(part => part!.StartsWith(TokenPlaceholder.FormatIndicator), null)
                        ?.TrimStart(TokenPlaceholder.FormatIndicator),
                    DefaultValue: tuple.parts
                        .FirstOrDefault(part => part.StartsWith(TokenPlaceholder.DefaultIndicator), string.Empty)
                        .TrimStart(TokenPlaceholder.DefaultIndicator),
                    Flags: tuple.parts
                        // Flags are elements not 1st (id)
                        .Skip(1)
                        // Flags do not start with a special character (reserved for to indicate non-flag values)
                        .Where(part => char.IsLetter(part.First()) || char.IsDigit(part.First()))
                        // Dynamically set the flags by case-insensitive matching the element with the enum name
                        .Select(flag => Enum.Parse<TokenPlaceholderFlags>(flag, true))
                        .Aggregate(TokenPlaceholderFlags.None, (flags, flag) => flags | flag),
                    Match: tuple.match
                )
            );
}
