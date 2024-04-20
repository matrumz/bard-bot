namespace BardBot.Discord.Exporting.PathTokens;

internal record Token<TValue>(
    string ShortId,
    string LongId,
    string Format
)
{
    private const string DefaultDateTimeFormat = "yyyy-MM-dd-HHmm";
    private const string DefaultDelimitedStringFormat = "s|^|.|";

    public static Token<DateTime> Before(string? format) { get; } = new("b", "before", format ?? DefaultDateTimeFormat);
    public static Token<DateTime> After(string? format) { get; } = new("a", "after", format ?? DefaultDateTimeFormat);
    public static Token<string> Character { get; } = new("c", "character", format ?? DefaultDelimitedStringFormat);
}
