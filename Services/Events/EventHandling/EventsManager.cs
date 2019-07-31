using System;
using Hangfire;
using Services.Events.EventHandling.Interfaces;
using Services.Events.Models;

namespace Services.Events.EventHandling
{
    public class EventsManager : IEventsManager
    {
        public void EnqueueEvent<TEvent>(TEvent eventData) where TEvent : EventBase
        {
            BackgroundJob.Enqueue<IEventHandlerManager>(x => x.HandleEvent(eventData));
        }

        public void ScheduleEvent<TEvent>(TEvent eventData, TimeSpan timeSpan) where TEvent : EventBase
        {
            BackgroundJob.Schedule<IEventHandlerManager>(x => x.HandleEvent(eventData), timeSpan);
        }
    }
}
