using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Dtos;

namespace Services.Interfaces
{
    public interface IDishesService
    {
        Task<IEnumerable<DishDto>> GetAllDishesAsync();
    }
}