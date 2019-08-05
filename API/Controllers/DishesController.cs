using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IDishesService _dishesService;

        public DishesController(IDishesService dishesService)
        {
            _dishesService = dishesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var results = await _dishesService.GetAllDishesAsync();
            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> Post(DishCreationDto dishCreationDto)
        {
            var result = await _dishesService.CreateDish(dishCreationDto);
            return Ok(result);
        }
    }
}