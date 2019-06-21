using Autofac;
using AutoMapper;
using Repositories.Interfaces;

namespace Services.Abstractions
{
    public abstract class ServiceBaseSql : ServiceBase
    {
        protected IUnitOfWork UnitOfWork;

        protected ServiceBaseSql(IMapper mapper, IComponentContext scope) : base(mapper)
        {
            UnitOfWork = scope.ResolveNamed<IUnitOfWork>("sql");
        }
    }
}