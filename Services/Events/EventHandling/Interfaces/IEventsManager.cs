using System;
using Services.Events.Models;

namespace Services.Events.EventHandling.Interfaces
{
    public interface IEventsManager
    {
        void EnqueueEvent<TEvent>(TEvent eventData) where TEvent : EventBase;

        void ScheduleEvent<TEvent>(TEvent eventData, TimeSpan timeSpan) where TEvent : EventBase;
    }
}