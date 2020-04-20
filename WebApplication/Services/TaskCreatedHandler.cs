using System;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using SimpleBus;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class TaskCreatedHandler : IMessageHandler<TaskCreatedEventModel>
    {
        readonly IHubContext<DataHub> _dataHubContext;

        public TaskCreatedHandler(IHubContext<DataHub> dataHubContext)
        {
            _dataHubContext = dataHubContext;
        }

        public void Handle(IReceivedMessage<TaskCreatedEventModel> message)
        {
            _dataHubContext.Clients.All.SendAsync("task.created", JsonSerializer.Serialize(message.Message));
        }
    }
}
