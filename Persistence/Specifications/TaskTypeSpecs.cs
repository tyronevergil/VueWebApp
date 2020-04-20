using System;
using System.Linq.Expressions;
using CrudDatastore;

namespace Persistence.Specifications
{
    public class TaskTypeSpecs : Specification<Entities.TaskType>
    {
        private TaskTypeSpecs(Expression<Func<Entities.TaskType, bool>> predicate)
            : base(predicate)
        { }

        private TaskTypeSpecs(string command, params object[] parameters)
            : base(command, parameters)
        { }

        public static TaskTypeSpecs GetTaskType(int id)
        {
            return new TaskTypeSpecs(t => t.Id == id);
        }

        public static TaskTypeSpecs GetTaskType(string label)
        {
            return new TaskTypeSpecs(t => t.Label == label);
        }

        public static TaskTypeSpecs GetTaskTypes()
        {
            return new TaskTypeSpecs(t => true);
        }
    }
}
