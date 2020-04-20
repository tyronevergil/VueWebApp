using System;
using System.Linq.Expressions;
using CrudDatastore;

namespace Persistence.Specifications
{
    public class TaskSpecs : Specification<Entities.Task>
    {
        private TaskSpecs(Expression<Func<Entities.Task, bool>> predicate)
            : base(predicate)
        { }

        private TaskSpecs(string command, params object[] parameters)
            : base(command, parameters)
        { }

        public static TaskSpecs GetTask(int id)
        {
            return new TaskSpecs(t => t.Id == id);
        }

        public static TaskSpecs GetTasks()
        {
            return new TaskSpecs(t => true);
        }
    }
}
