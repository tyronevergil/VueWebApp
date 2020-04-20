using System;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using SimpleBus;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class TaskTypeCreatedHandler : IMessageHandler<TaskTypeCreatedEventModel>
    {
        readonly IHubContext<DataHub> _dataHubContext;

        public TaskTypeCreatedHandler(IHubContext<DataHub> dataHubContext)
        {
            _dataHubContext = dataHubContext;
        }

        public void Handle(IReceivedMessage<TaskTypeCreatedEventModel> message)
        {
            _dataHubContext.Clients.All.SendAsync("tasktype.created", JsonSerializer.Serialize(message.Message));
        }
    }
}
