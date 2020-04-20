using System;
using CrudDatastore;

namespace Persistence.Entities
{
    public class Activity : EntityBase
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
    }
}
