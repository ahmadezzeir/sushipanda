using System.Threading.Tasks;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Services.Events.EventHandling.Interfaces;
using Services.Events.Models;
using Services.Interfaces;

namespace Services.Events
{
    public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IOrdersService _ordersService;

        public OrderCreatedEventHandler(IHubContext<NotificationHub> hubContext, IOrdersService ordersService)
        {
            _hubContext = hubContext;
            _ordersService = ordersService;
        }

        public async Task HandleEvent(OrderCreatedEvent eventData)
        {
            var ordersCount = await _ordersService.GetOrdersCount();
            await _hubContext.Clients.All.SendAsync("getOrdersCount", ordersCount);
        }
    }
}