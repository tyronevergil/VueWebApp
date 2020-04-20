using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleBus;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    public class CommandController : Controller
    {
        private readonly IServiceBus _serviceBus;

        public CommandController(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        [HttpPost("[action]")]
        public StatusCodeResult TaskCreate([FromBody] TaskCreateCommandModel command)
        {
            _serviceBus.Send(command);
            return StatusCode(201);
        }

        [HttpPost("[action]")]
        public StatusCodeResult TaskComplete([FromBody] TaskCompleteCommandModel command)
        {
            _serviceBus.Send(command);
            return StatusCode(201);
        }
    }
}
