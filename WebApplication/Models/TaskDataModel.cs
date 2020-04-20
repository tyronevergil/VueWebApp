using System;

namespace WebApplication.Models
{
    public class TaskDataModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool Completed { get; set; }
    }
}
