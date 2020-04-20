using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleBus
{
    public class ServiceBus : IServiceBus
    {
        private readonly IMessageStore _store;
        private readonly IEnumerable<IMessageHandler> _handlers;

        public ServiceBus(IMessageStore store, IEnumerable<IMessageHandler> handlers)
        {
            _store = store;
            _handlers = handlers;
        }

        public void Send<T>(T command) where T : ICommandMessage
        {
            var activity = new Activity
            {
                Type = command.GetType().ToString(),
                Payload = JsonSerializer.Serialize(command)
            };

            _store.Add(activity);

            Process(command, activity.EventId);
        }

        private void Process(IMessage message, string eventId)
        {
            var messageType = message.GetType();
            var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);
            foreach (var handler in _handlers.Where(h => handlerType.IsAssignableFrom(h.GetType())))
            {
                Task.Factory.StartNew(() =>
                {
                    var context = new HandlerContext((m) => Process(m, eventId));
                    var method = handlerType.GetMethod("Handle");
                    method.Invoke(handler, new[] { Activator.CreateInstance(typeof(ReceivedMessage<>).MakeGenericType(messageType), eventId, message, context) });
                });
            }
        }
    }

    internal class Activity : IActivity
    {
        public string EventId { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
    }

    internal class HandlerContext : IHandlerContext
    {
        private readonly Action<IMessage> _publish;

        public HandlerContext(Action<IMessage> publish)
        {
            _publish = publish;
        }

        public void Publish<T>(T message) where T : IEventMessage
        {
            _publish(message);
        }
    }

    internal class ReceivedMessage<T> : IReceivedMessage<T> where T : IMessage
    {
        public ReceivedMessage(string eventId, T message, IHandlerContext context)
        {
            EventId = eventId;
            Message = message;
            Context = context;
        }

        public string EventId { get; private set;  }

        public T Message { get; private set; }

        public IHandlerContext Context { get; private set; }
    }
}
