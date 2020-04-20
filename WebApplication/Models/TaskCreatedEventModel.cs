using System;
using SimpleBus;

namespace WebApplication.Models
{
    public class TaskCreatedEventModel : IEventMessage
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
    }
}
