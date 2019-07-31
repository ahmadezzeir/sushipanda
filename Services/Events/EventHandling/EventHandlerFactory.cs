using System;
using Microsoft.Extensions.DependencyInjection;
using Services.Events.EventHandling.Interfaces;
using Services.Events.Models;

namespace Services.Events.EventHandling
{
    public class EventHandlerFactory : IEventHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public EventHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEventHandler<TEvent> GetEventHandler<TEvent>() where TEvent : EventBase
        {
            return _serviceProvider.GetService<IEventHandler<TEvent>>(); 
        }
    }
}