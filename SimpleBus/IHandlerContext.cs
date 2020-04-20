using System;

namespace SimpleBus
{
    public interface IHandlerContext
    {
        void Publish<T>(T message) where T : IEventMessage;
    }
}
