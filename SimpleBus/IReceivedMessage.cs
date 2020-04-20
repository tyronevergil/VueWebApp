using System;

namespace SimpleBus
{
    public interface IReceivedMessage<T> where T : IMessage
    {
        string EventId { get; }
        T Message { get; }
        IHandlerContext Context { get; }
    }
}
