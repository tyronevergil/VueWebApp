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
    public class TaskTypesController : Controller
    {
        private readonly ITodoContextFactory _contextFactory;

        public TaskTypesController(ITodoContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [HttpGet]
        public IEnumerable<TaskTypeDataModel> Get()
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

        [HttpGet("{id}")]
        public TaskTypeDataModel Get(int id)
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
