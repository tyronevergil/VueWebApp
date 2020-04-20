using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.Specifications;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private readonly ITodoContextFactory _contextFactory;

        public TasksController(ITodoContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [HttpGet]
        public IEnumerable<TaskDataModel> Get()
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                return context
                    .Find(TaskSpecs.GetTasks())
                    .Select(t => new TaskDataModel
                        {
                            Id = t.Id,
                            Description = t.Description,
                            Type = t.Type,
                            Completed = t.Completed
                        })
                    .ToList();
            }
        }

        [HttpGet("{id}")]
        public TaskDataModel Get(int id)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                return context
                    .Find(TaskSpecs.GetTask(id))
                    .Select(t => new TaskDataModel
                        {
                            Id = t.Id,
                            Description = t.Description,
                            Type = t.Type,
                            Completed = t.Completed
                        })
                    .FirstOrDefault();
            }
        }
    }
}
