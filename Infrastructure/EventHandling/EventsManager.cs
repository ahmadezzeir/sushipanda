using System;
using Hangfire;
using Infrastructure.EventHandling.Interfaces;
using Infrastructure.Events.Models;

namespace Infrastructure.EventHandling
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
