using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Persistence;
using Persistence.Specifications;
using SimpleBus;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    public class EventController : Controller
    {
        private readonly IServiceBus _serviceBus;
        private readonly IMessageContextFactory _contextFactory;

        readonly IHubContext<DataHub> _dataHubContext;

        public EventController(IServiceBus serviceBus, IMessageContextFactory contextFactory, IHubContext<DataHub> dataHubContext)
        {
            _serviceBus = serviceBus;
            _contextFactory = contextFactory;

            _dataHubContext = dataHubContext;
        }

        [HttpGet]
        public IActionResult Replay()
        {
            TodoContextFactory.Reset();

            Thread.Sleep(1000);

            _dataHubContext.Clients.All.SendAsync("event.replay", "replay");

            Thread.Sleep(1000);

            using (var context = _contextFactory.CreateDataContext())
            {
                var activities = context
                    .Find(ActivitySpecs.GetActivities())
                    .ToList();

                foreach (var activity in activities)
                {
                    var type = Type.GetType(string.Format("{0}, WebApplication", activity.Type));
                    var command = JsonSerializer.Deserialize(activity.Payload, type);

                    _serviceBus.Process((IMessage)command, activity.Id.ToString());

                    Thread.Sleep(1000);
                }
            }

            return Ok("Done!");
        }
    }
}
