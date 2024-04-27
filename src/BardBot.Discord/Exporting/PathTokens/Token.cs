namespace BardBot.Discord.Exporting.PathTokens;

public partial record Token(
    string Id,
    object Value
)
{
    public static Token After(DateTime value) => new("after", value);
    public static Token Before(DateTime value) => new("before", value);
    public static Token Character(string value) => new("character", value);

    /// <summary>
    /// Create a new custom token with the given id and value.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Token From(string id, object value) => new(id, value);
}

public partial record Token<TValue>
{
    public const string DefaultDateTimeFormat = "yyyy-MM-dd-HHmm";
    public const string DefaultDelimitedStringFormat = "s|^|.|";
}
