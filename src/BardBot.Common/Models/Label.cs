namespace BardBot.Common.Models;

public class Label
{
    public required string Name { get; set; }

    public object? Value { get; set; }

    public static implicit operator Label(string key) => new() { Name = key };
}

public class Label<T> : Label
{
    public new T? Value
    {
        get => (T?)base.Value;
        set => base.Value = value;
    }

    public static implicit operator Label<T>((string n, T v) nv) => new() { Name = nv.n, Value = nv.v };
}

public interface ILabelled
{
    public IEnumerable<Label> Labels { get; set; }
}
