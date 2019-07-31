using System;

namespace Services.Events.Models
{
    public class EventBase
    {
        public DateTime CreatedAt => DateTime.Now;
    }
}