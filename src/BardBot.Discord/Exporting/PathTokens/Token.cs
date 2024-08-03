using System.Globalization;

namespace BardBot.Discord.Exporting.PathTokens;

internal partial record Token(
    string Id,
    object Value
)
{
    public static Token After(DateTime value) => new("after", value);
    public static Token Before(DateTime value) => new("before", value);
    public static Token Character(string value) => new("character", value);
    public static Token Extension(string value) => new("extension", value);

    /// <summary>
    /// Create a new custom token with the given id and value.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Token From(string id, object value) => new(id, value);
}

internal static partial class StringExtensions
{
    internal static string ApplyTokens(this string template, IEnumerable<Token> tokens)
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
}
