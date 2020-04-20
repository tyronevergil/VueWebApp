using System;
using System.Collections.Generic;
using System.Linq;
using CrudDatastore;

namespace Persistence
{
    public class TodoContextFactory : ITodoContextFactory
    {
        public DataContextBase CreateDataContext()
        {
            return new GenericDataContext(new TodoUnitOfWorkInMemory());
        }
    }

    internal class TodoUnitOfWorkInMemory : UnitOfWorkBase
    {
        private static readonly IList<Entities.Task> _tasks = new List<Entities.Task>();
        private static readonly IList<Entities.TaskType> _taskTypes = new List<Entities.TaskType>();

        private IDataStore<Entities.Task> Tasks()
        {
            return new DataStore<Entities.Task>(
                new DelegateCrudAdapter<Entities.Task>(
                    /* create */
                    (e) =>
                    {
                        e.Id = (_tasks.Any() ? _tasks.Max(i => i.Id) : 0) + 1;
                        _tasks.Add(new Entities.Task
                        {
                            Id = e.Id,
                            Description = e.Description,
                            Type = e.Type,
                            Completed = e.Completed
                        });
                    },

                    /* update */
                    (e) =>
                    {
                        var task = _tasks.FirstOrDefault(i => i.Id == e.Id);
                        if (task != null)
                        {
                            task.Description = e.Description;
                            task.Type = e.Type;
                            task.Completed = e.Completed;
                        }
                    },

                    /* delete */
                    (e) =>
                    {
                    },

                    /* read */
                    (predicate) =>
                    {
                        return _tasks.Where(predicate.Compile()).AsQueryable();
                    }
                ));
        }

        private IDataStore<Entities.TaskType> TaskTypes()
        {
            return new DataStore<Entities.TaskType>(
                new DelegateCrudAdapter<Entities.TaskType>(
                    /* create */
                    (e) =>
                    {
                        e.Id = (_taskTypes.Any() ? _taskTypes.Max(i => i.Id) : 0) + 1;
                        _taskTypes.Add(new Entities.TaskType
                        {
                            Id = e.Id,
                            Label = e.Label
                        });
                    },

                    /* update */
                    (e) => { },


                    /* delete */
                    (e) => { },

                    /* read */
                    (predicate) =>
                    {
                        return _taskTypes.Where(predicate.Compile()).AsQueryable();
                    }
                )
            );
        }

        static TodoUnitOfWorkInMemory()
        {
            _tasks.Add(new Entities.Task { Id = 1, Description = "Buy Milk", Type = "Family", Completed = false });
            _tasks.Add(new Entities.Task { Id = 2, Description = "Book hotel at Subic", Type = "Travel", Completed = false });
            _tasks.Add(new Entities.Task { Id = 3, Description = "Finalize Presentation", Type = "Work", Completed = true });
            _tasks.Add(new Entities.Task { Id = 4, Description = "Finish design for the new website", Type = "Work", Completed = false });

            _taskTypes.Add(new Entities.TaskType { Id = 1, Label = "Family" });
            _taskTypes.Add(new Entities.TaskType { Id = 2, Label = "Travel" });
            _taskTypes.Add(new Entities.TaskType { Id = 3, Label = "Work" });
        }

        public TodoUnitOfWorkInMemory()
        {
            this.Register(Tasks());
            this.Register(TaskTypes());
        }
    }
}
