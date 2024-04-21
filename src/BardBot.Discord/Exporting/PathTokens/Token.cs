namespace BardBot.Discord.Exporting.PathTokens;

internal partial record Token<TValue>(
    string Id,
    TValue Value
)
{
    public static Token<DateTime> Before(DateTime value) => new("before", value);
    public static Token<DateTime> After(DateTime value) => new("after", value);
    public static Token<string> Character(string value) => new("character", value);

    /// <summary>
    /// Create a new custom token with the given id and value.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Token<TValue> From(string id, TValue value) => new(id, value);
}

internal partial record Token<TValue>
{
    public const string DefaultDateTimeFormat = "yyyy-MM-dd-HHmm";
    public const string DefaultDelimitedStringFormat = "s|^|.|";
}
