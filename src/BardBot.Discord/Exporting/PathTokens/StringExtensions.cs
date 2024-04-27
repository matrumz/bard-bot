using System.Globalization;
using System.Text.RegularExpressions;

namespace BardBot.Discord.Exporting.PathTokens;

public static partial class StringExtensions
{

    public static string ApplyTokens(this string template, IEnumerable<Token> tokens)
    {
        template.GetTokenPlaceholders().ToList().ForEach(placeholder =>
        {
            var token = tokens.FirstOrDefault(token => token.Id == placeholder.TokenId, Token.From(placeholder.TokenId, placeholder.DefaultValue));

            var value = token.Value is IFormattable formattable
                ? formattable.ToString(placeholder.Format, CultureInfo.InvariantCulture)
                : token.Value;

            template = template.Replace(placeholder.Match, value.ToString());
        });
        return template;
    }

    public static IEnumerable<TokenPlaceholder> GetTokenPlaceholders(this string template)
    {
        var matches = PlaceholderRegex().Matches(template);
        return matches.Select(match => match.Groups["match"].Value)
            // Split by ',' not escaped by '%'
            // i.e. "a,b%,c" => ["a", "b,c"]
            .Select(match => UnescapedDelimiterRegex().Split(match).Where(element => !string.IsNullOrEmpty(element)).Prepend($"{{{match}}}").ToArray())
            .Select(matchParts =>
            {
                var match = matchParts[0];
                var id = matchParts[1];
                var defaultValue = matchParts.FirstOrDefault(part => part.StartsWith(TokenPlaceholder.DefaultIndicator), string.Empty).TrimStart(TokenPlaceholder.DefaultIndicator);
                var format = matchParts.FirstOrDefault(part => part!.StartsWith(TokenPlaceholder.FormatIndicator), null)?.TrimStart(TokenPlaceholder.FormatIndicator);
                // Dynamically set the flags by case-insensitive matching the element with the enum name
                var flags = matchParts
                    // Flags are elements not 1st (the whole match) or 2nd (the id)
                    .Skip(2)
                    // Flags do not start with a special character (reserved for to indicate non-flag values)
                    .Where(part => char.IsLetter(part.First()) || char.IsDigit(part.First()))
                    .Select(flag => Enum.Parse<TokenPlaceholderFlags>(flag, true))
                    .Aggregate(TokenPlaceholderFlags.None, (flags, flag) => flags | flag);
                return new TokenPlaceholder(
                    DefaultValue: defaultValue,
                    Format: format,
                    Match: match,
                    TokenId: id
                );
            });
    }

    [GeneratedRegex(@"\{(?<match>[^}]+)\}")]
    private static partial Regex PlaceholderRegex();
    [GeneratedRegex(@"(?<!%),")]
    private static partial Regex UnescapedDelimiterRegex();
}
