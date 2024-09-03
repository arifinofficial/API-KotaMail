using API.Dto;
using API.RepositoryContract;
using API.ServiceContract;
using Framework.Service;

namespace API.Service
{
    public class ConnectionDetailFilterService(IConnectionDetailFilterRepository repository) : BaseUserService<ConnectionDetailFilterDto, ulong, IConnectionDetailFilterRepository>(repository), IConnectionDetailFilterService
    {
    }
}
