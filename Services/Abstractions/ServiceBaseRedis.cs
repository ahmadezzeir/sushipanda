using Autofac;
using AutoMapper;
using Repositories.Interfaces;

namespace Services.Abstractions
{
    public abstract class ServiceBaseRedis : ServiceBase
    {
        protected IUnitOfWork UnitOfWork;

        protected ServiceBaseRedis(IMapper mapper, IComponentContext scope) : base(mapper)
        {
            UnitOfWork = scope.ResolveNamed<IUnitOfWork>("redis");
        }
    }
}