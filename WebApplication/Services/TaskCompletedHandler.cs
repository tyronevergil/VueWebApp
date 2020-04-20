using System;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using SimpleBus;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class TaskCompletedHandler : IMessageHandler<TaskCompletedEventModel>
    {
        readonly IHubContext<DataHub> _dataHubContext;

        public TaskCompletedHandler(IHubContext<DataHub> dataHubContext)
        {
            _dataHubContext = dataHubContext;
        }

        public void Handle(IReceivedMessage<TaskCompletedEventModel> message)
        {
            _dataHubContext.Clients.All.SendAsync("task.completed", JsonSerializer.Serialize(message.Message));
        }
    }
}
