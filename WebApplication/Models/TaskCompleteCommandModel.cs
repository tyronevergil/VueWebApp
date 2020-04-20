using System;
using SimpleBus;

namespace WebApplication.Models
{
    public class TaskCompleteCommandModel : ICommandMessage
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
    }
}
