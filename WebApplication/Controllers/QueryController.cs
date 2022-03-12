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
    public class QueryController : Controller
    {
        private readonly ITodoContextFactory _contextFactory;

        public QueryController(ITodoContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [HttpGet]
        [Route("Tasks")]
        public IEnumerable<TaskDataModel> GetTasks()
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

        [HttpGet]
        [Route("Tasks/{id}")]
        public TaskDataModel GetTasks(int id)
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

        [HttpGet]
        [Route("TaskTypes")]
        public IEnumerable<TaskTypeDataModel> GetTaskTypes()
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                return context
                    .Find(TaskTypeSpecs.GetTaskTypes())
                    .Select(t => new TaskTypeDataModel
                    {
                        Label = t.Label
                    })
                    .ToList();
            }
        }

        [HttpGet]
        [Route("TaskTypes/{id}")]
        public TaskTypeDataModel GetTaskTypes(int id)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                return context
                    .Find(TaskTypeSpecs.GetTaskType(id))
                        .Select(t => new TaskTypeDataModel
                        {
                            Label = t.Label
                        })
                    .FirstOrDefault();
            }
        }
    }
}
