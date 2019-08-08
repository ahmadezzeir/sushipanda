using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
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
            var dishes = await _dishesRepository.GetAllAsync(x => x.Include(d => d.File));
            return Mapper.Map<IEnumerable<DishDto>>(dishes);
        }

        public async Task<Guid> CreateDish(DishCreationDto dishCreationDto)
        {
            var dish = Mapper.Map<Dish>(dishCreationDto);
            await _dishesRepository.AddAsync(dish);
            await UnitOfWork.CommitAsync();

            return dish.Id;
        }
    }
}