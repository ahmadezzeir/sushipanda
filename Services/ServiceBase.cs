using AutoMapper;
using Repositories.Interfaces;

namespace Services
{
    public abstract class ServiceBase
    {
        protected IMapper Mapper;
        protected IUnitOfWork UnitOfWork;

        protected ServiceBase(IMapper mapper, IUnitOfWork unitOfWork)
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
        }
    }
}