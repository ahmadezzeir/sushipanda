using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderCreationDto orderCreationDto)
        {
            await _ordersService.CreateOrderAsync(orderCreationDto);
            return Ok();
        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetOrdersCount()
        {
            var count = await _ordersService.GetOrdersCount();
            return Ok(count);
        }
    }
}