using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Domain.Models;
using Repositories.Interfaces;
using Services.Abstractions;
using Services.Dtos;
using Services.Interfaces;

namespace Services
{
    public class DishesService : ServiceBaseSql, IDishesService
    {
        private readonly IRepository<Dish> _dishesRepository;

        public DishesService(IMapper mapper, IComponentContext scope) : base(mapper, scope)
        {
            _dishesRepository = UnitOfWork.Repository<Dish>();
        }

        public async Task<IEnumerable<DishDto>> GetAllDishesAsync()
        {
            var dishes = await _dishesRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<DishDto>>(dishes);
        }
    }
}