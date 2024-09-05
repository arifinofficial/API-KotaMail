using API.Dto;
using API.RepositoryContract;
using API.ServiceContract;
using Framework.Service;

namespace API.Service
{
    public class ConnectionFilterService(IConnectionFilterRepository repository) : BaseUserService<ConnectionFilterDto, ulong, IConnectionFilterRepository>(repository), IConnectionFilterService
    {
    }
}
