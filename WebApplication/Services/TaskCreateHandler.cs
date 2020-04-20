using System;
using Persistence;
using SimpleBus;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class TaskCreateHandler : IMessageHandler<TaskCreateCommandModel>
    {
        private readonly ITodoContextFactory _contextFactory;

        public TaskCreateHandler(ITodoContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void Handle(IReceivedMessage<TaskCreateCommandModel> message)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entry = new Persistence.Entities.Task
                    {
                        Description = message.Message.Description,
                        Type = message.Message.Type
                    };
                context.Add(entry);
                context.SaveChanges();

                message.Context.Publish(new TaskCreatedEventModel
                    {
                        Id = entry.Id,
                        TransactionId = message.Message.TransactionId
                    });
            }
        }
    }
}
