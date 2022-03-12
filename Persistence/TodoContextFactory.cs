using System;
using System.Collections.Generic;
using System.Linq;
using CrudDatastore;

namespace Persistence
{
    public class TodoContextFactory : ITodoContextFactory
    {
        private static IList<Entities.Task> _tasks;
        private static IList<Entities.TaskType> _taskTypes;

        static TodoContextFactory()
        {
            Reset();
        }

        public DataContextBase CreateDataContext()
        {
            return new GenericDataContext(new TodoUnitOfWorkInMemory(_tasks, _taskTypes));
        }

        public static void Reset()
        {
            _tasks = new List<Entities.Task>();
            _tasks.Add(new Entities.Task { Id = 1, Description = "Buy Milk", Type = "Family", Completed = false });
            _tasks.Add(new Entities.Task { Id = 2, Description = "Book hotel at Subic", Type = "Travel", Completed = false });
            _tasks.Add(new Entities.Task { Id = 3, Description = "Finalize Presentation", Type = "Work", Completed = true });
            _tasks.Add(new Entities.Task { Id = 4, Description = "Finish design for the new website", Type = "Work", Completed = false });

            _taskTypes = new List<Entities.TaskType>();
            _taskTypes.Add(new Entities.TaskType { Id = 1, Label = "Family" });
            _taskTypes.Add(new Entities.TaskType { Id = 2, Label = "Travel" });
            _taskTypes.Add(new Entities.TaskType { Id = 3, Label = "Work" });
        }
    }

    internal class TodoUnitOfWorkInMemory : UnitOfWorkBase
    {
        private IDataStore<Entities.Task> Tasks(IList<Entities.Task> tasks)
        {
            return new DataStore<Entities.Task>(
                new DelegateCrudAdapter<Entities.Task>(
                    /* create */
                    (e) =>
                    {
                        e.Id = (tasks.Any() ? tasks.Max(i => i.Id) : 0) + 1;
                        tasks.Add(new Entities.Task
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
                        var task = tasks.FirstOrDefault(i => i.Id == e.Id);
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
                        return tasks.Where(predicate.Compile()).AsQueryable();
                    }
                ));
        }

        private IDataStore<Entities.TaskType> TaskTypes(IList<Entities.TaskType> taskTypes)
        {
            return new DataStore<Entities.TaskType>(
                new DelegateCrudAdapter<Entities.TaskType>(
                    /* create */
                    (e) =>
                    {
                        e.Id = (taskTypes.Any() ? taskTypes.Max(i => i.Id) : 0) + 1;
                        taskTypes.Add(new Entities.TaskType
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
                        return taskTypes.Where(predicate.Compile()).AsQueryable();
                    }
                )
            );
        }

        public TodoUnitOfWorkInMemory(IList<Entities.Task> tasks, IList<Entities.TaskType> taskTypes)
        {
            this.Register(Tasks(tasks));
            this.Register(TaskTypes(taskTypes));
        }
    }
}
