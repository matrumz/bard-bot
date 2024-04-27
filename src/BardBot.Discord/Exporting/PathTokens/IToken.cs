namespace BardBot.Discord.Exporting.PathTokens;

public interface IToken<out TValue>
{
    public TValue Value { get; }
}
