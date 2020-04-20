using System;
using System.Linq;
using Persistence;
using Persistence.Specifications;
using SimpleBus;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class TaskTypeCreateHandler : IMessageHandler<TaskCreateCommandModel>
    {
        private readonly ITodoContextFactory _contextFactory;

        public TaskTypeCreateHandler(ITodoContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void Handle(IReceivedMessage<TaskCreateCommandModel> message)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var found = context.Find(TaskTypeSpecs.GetTaskType(message.Message.Type)).Any();
                if (!found)
                {
                    var entry = new Persistence.Entities.TaskType
                    {
                        Label = message.Message.Type
                    };
                    context.Add(entry);
                    context.SaveChanges();

                    message.Context.Publish(new TaskTypeCreatedEventModel
                    {
                        Id = entry.Id,
                        TransactionId = message.Message.TransactionId
                    });
                }
            }
        }
    }
}
