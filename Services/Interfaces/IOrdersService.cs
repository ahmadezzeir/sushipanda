using System.Threading;
using System.Threading.Tasks;
using Services.Dtos;

namespace Services.Interfaces
{
    public interface IOrdersService
    {
        Task CreateOrderAsync(OrderCreationDto orderCreationDto);

        Task<int> GetOrdersCount();
    }
}