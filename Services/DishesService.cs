using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Domain.Models;
using Infrastructure.FileSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Interfaces;
using Services.Abstractions;
using Services.Dtos;
using Services.Interfaces;

namespace Services
{
    public class DishesService : ServiceBaseSql, IDishesService
    {
        private readonly IRepository<Dish> _dishesRepository;
        private readonly IRepository<File> _filesRepository;

        private readonly IFileSystemService _fileSystemService;

        public DishesService(IMapper mapper, IComponentContext scope, IFileSystemService fileSystemService) : base(mapper, scope)
        {
            _dishesRepository = UnitOfWork.Repository<Dish>();
            _filesRepository = UnitOfWork.Repository<File>();

            _fileSystemService = fileSystemService;
        }

        public async Task<IEnumerable<DishDto>> GetAllDishesAsync()
        {
            Func<IQueryable<Dish>, IOrderedQueryable<Dish>> orderBy = queryable =>
                queryable.OrderByDescending(d => d.Orders.Count).ThenByDescending(d => d.Name);
            Func<IQueryable<Dish>, IIncludableQueryable<Dish, object>> include = queryable =>
                queryable.Include(d => d.File).Include(d => d.Orders);
            var dishes = await _dishesRepository.GetPagedAsync(1, 8,
                null, orderBy, include);
            return Mapper.Map<IEnumerable<DishDto>>(dishes.Results);
        }

        public async Task<Guid> CreateDish(DishCreationDto dishCreationDto)
        {
            var dish = Mapper.Map<Dish>(dishCreationDto);
            await _dishesRepository.AddAsync(dish);
            await UnitOfWork.CommitAsync();

            var file = await _filesRepository.GetByIdAsync(dish.FileId);
            await _fileSystemService.MoveFileAsync(file.Name, "dishes");

            return dish.Id;
        }
    }
}