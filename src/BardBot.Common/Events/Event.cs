using Prism.Events;

namespace BardBot.Common.Events;

public abstract class Event<T> : PubSubEvent<Payload<T>>, IEvent<T>
{
}
