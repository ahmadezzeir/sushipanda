using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Domain.Models;
using Repositories.Interfaces;
using Services.Abstractions;
using Services.Dtos;
using Services.Events.EventHandling.Interfaces;
using Services.Events.Models;
using Services.Interfaces;

namespace Services
{
    public class OrdersService : ServiceBaseSql, IOrdersService
    {
        private readonly IRepository<Order> _ordersRepository;
        private readonly IRepository<Dish> _dishesRepository;
        private readonly IEventsManager _eventsManager;

        public OrdersService(IMapper mapper, IComponentContext scope, IEventsManager eventsManager) : base(mapper, scope)
        {
            _ordersRepository = UnitOfWork.Repository<Order>();
            _dishesRepository = UnitOfWork.Repository<Dish>();
            _eventsManager = eventsManager;
        }

        public async Task CreateOrderAsync(OrderCreationDto orderCreationDto)
        {
            var order = Mapper.Map<Order>(orderCreationDto);
            foreach (var dishId in orderCreationDto.Dishes)
            {
                var dish = await _dishesRepository.GetByIdAsync(dishId);
                order.Dishes.Add(new OrderDish { Dish = dish});
            }
            await _ordersRepository.AddAsync(order);

            await UnitOfWork.CommitAsync();

            _eventsManager.EnqueueEvent(new OrderCreatedEvent());
        }

        public async Task<int> GetOrdersCount()
        {
            return await _ordersRepository.CountAsync();
        }
    }
}