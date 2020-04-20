using System;
using Persistence;
using SimpleBus;

namespace WebApplication.Services
{
    public class MessageStore : IMessageStore
    {
        private readonly IMessageContextFactory _contextFactory;

        public MessageStore(IMessageContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void Add(IActivity activity)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entry = new Persistence.Entities.Activity
                    {
                        Type = activity.Type,
                        Payload = activity.Payload
                    };
                context.Add(entry);
                context.SaveChanges();

                activity.EventId = entry.Id.ToString();
            }
        }
    }
}
