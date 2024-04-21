using System.Globalization;
using System.Text.RegularExpressions;

namespace BardBot.Discord.Exporting.PathTokens;

internal static partial class StringExtensions
{

    public static string ApplyTokens(this string template, IEnumerable<Token<object>> tokens)
    {
        template.GetTokenPlaceholders().ToList().ForEach(placeholder =>
        {
            var token = tokens.FirstOrDefault(token => token.Id == placeholder.TokenId, Token.From(placeholder.TokenId, placeholder.DefaultValue));


            var value = token?.Value ?? placeholder.DefaultValue as ;
            if (value is IFormattable formattable)
            {
                value = formattable.ToString(placeholder.Format, CultureInfo.InvariantCulture);
            }

            // if (value is DateTime dateTime)
            // {
            //     value = dateTime.ToString(placeholder.Format);
            // }
            // else
            // if (token is not null)
            // {
            //     var value = token.Value;
            //     if (value is DateTime dateTime)
            //     {
            //         value = dateTime.ToString(placeholder.Format);
            //     }
            //     template = template.Replace(placeholder.Match, value.ToString());
            // }
        });
    }

    public static IEnumerable<TokenPlaceholder> GetTokenPlaceholders(this string template)
    {
        var matches = PlaceholderRegex().Matches(template);
        return matches.Select(match => match.Groups["match"].Value)
            // Split by ',' not escaped by '%'
            // i.e. "a,b%,c" => ["a", "b,c"]
            .Select(match => match.Split("(?<!%),", StringSplitOptions.RemoveEmptyEntries).Prepend($"{match}").ToArray())
            .Select(matchParts =>
            {
                var match = matchParts[0];
                var id = matchParts[1];
                // Default value is the first element that begins with '='
                var defaultValue = matchParts.FirstOrDefault(part => part.StartsWith('='), string.Empty);
                // Format is the first element that begins with ':'
                var format = matchParts.FirstOrDefault(part => part.StartsWith(':'), null);
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


}
