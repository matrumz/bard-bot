namespace BardBot.Discord.Discord;

internal partial record Snowflake(
    ulong Value
)
{
    public static implicit operator ulong(Snowflake snowflake) => snowflake.Value;
    public static implicit operator Snowflake(ulong value) => new(value);
}
