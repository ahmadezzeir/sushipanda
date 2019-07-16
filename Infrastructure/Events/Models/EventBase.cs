using System;

namespace Infrastructure.Events.Models
{
    public class EventBase
    {
        public DateTime CreatedAt => DateTime.Now;
    }
}