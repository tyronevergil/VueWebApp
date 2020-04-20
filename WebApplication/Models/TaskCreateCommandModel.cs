using System;
using SimpleBus;

namespace WebApplication.Models
{
    public class TaskCreateCommandModel : ICommandMessage
    {
        public string Description { get; set; }
        public string Type { get; set; }
        public string TransactionId { get; set; }
    }
}
