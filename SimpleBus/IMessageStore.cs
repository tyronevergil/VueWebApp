using System;

namespace SimpleBus
{
    public interface IMessageStore
    {
        void Add(IActivity activity);
    }

    public interface IActivity
    {
        string EventId { get; set; }
        string Type { get; set; }
        string Payload { get; set; }
    }
}
