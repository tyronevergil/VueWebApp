using System;
using CrudDatastore;

namespace Persistence.Entities
{
    public class Task : EntityBase
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool Completed { get; set; }
    }
}
