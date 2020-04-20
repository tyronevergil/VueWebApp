using System;
using CrudDatastore;

namespace Persistence.Entities
{
    public class TaskType : EntityBase
    {
        public int Id { get; set; }
        public string Label { get; set; }
    }
}
