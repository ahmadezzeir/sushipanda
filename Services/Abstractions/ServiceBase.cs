using AutoMapper;

namespace Services.Abstractions
{
    public abstract class ServiceBase
    {
        protected IMapper Mapper;

        protected ServiceBase(IMapper mapper)
        {
            Mapper = mapper;
            
        }
    }
}