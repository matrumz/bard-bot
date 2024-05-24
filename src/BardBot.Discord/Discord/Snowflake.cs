using Discord;

namespace BardBot.Discord.Discord;

internal partial record Snowflake(ulong Value);

// As ulong
internal partial record Snowflake
{
    public static implicit operator ulong(Snowflake snowflake) => snowflake.Value;
    public static implicit operator Snowflake(ulong value) => new(value);
}

// As DateTime
internal partial record Snowflake
{
    public static implicit operator DateTime(Snowflake snowflake) => SnowflakeUtils.FromSnowflake(snowflake.Value).DateTime;
    public static implicit operator Snowflake(DateTime value) => new(SnowflakeUtils.ToSnowflake(value));
}

// As DateTimeOffset
internal partial record Snowflake
{
    public static implicit operator DateTimeOffset(Snowflake snowflake) => SnowflakeUtils.FromSnowflake(snowflake.Value);
    public static implicit operator Snowflake(DateTimeOffset value) => new(SnowflakeUtils.ToSnowflake(value));
}

// Comparison
internal partial record Snowflake : IComparable<Snowflake>, IComparable
{
    public int CompareTo(Snowflake? other) => Value.CompareTo(other?.Value);
    public int CompareTo(object? obj) =>
        obj is Snowflake other
            ? Value.CompareTo(other.Value)
            : throw new ArgumentException($"Object must be of type {nameof(Snowflake)}");
    public static bool operator >(Snowflake left, Snowflake right) => left.CompareTo(right) > 0;
    public static bool operator <(Snowflake left, Snowflake right) => left.CompareTo(right) < 0;
}
