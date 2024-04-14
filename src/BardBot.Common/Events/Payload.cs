using BardBot.Common.Events.Payloads;

namespace BardBot.Common.Events;

public abstract record Payload<T>
{
    public Instance Instance { get; init; } = new();
    public T Data { get; init; } = default!;
}
