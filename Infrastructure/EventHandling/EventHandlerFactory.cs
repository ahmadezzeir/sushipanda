using System;
using Infrastructure.EventHandling.Interfaces;
using Infrastructure.Events.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.EventHandling
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