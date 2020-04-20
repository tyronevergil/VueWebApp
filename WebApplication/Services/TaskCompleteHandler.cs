using System;
using System.Linq;
using Persistence;
using Persistence.Specifications;
using SimpleBus;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class TaskCompleteHandler : IMessageHandler<TaskCompleteCommandModel>
    {
        private readonly ITodoContextFactory _contextFactory;

        public TaskCompleteHandler(ITodoContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void Handle(IReceivedMessage<TaskCompleteCommandModel> message)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entry = context.Find(TaskSpecs.GetTask(message.Message.Id)).FirstOrDefault();
                if (entry != null)
                {
                    entry.Completed = true;
                    context.Update(entry);
                    context.SaveChanges();

                    message.Context.Publish(new TaskCompletedEventModel
                    {
                        Id = entry.Id,
                        TransactionId = message.Message.TransactionId
                    });
                }
            }
        }
    }
}
