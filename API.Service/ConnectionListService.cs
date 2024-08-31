using API.Dto;
using API.RepositoryContract;
using API.ServiceContract;
using Framework.Service;

namespace API.Service
{
    public class ConnectionListService(IConnectionListRepository repository) : BaseService<ConnectionListDto, ulong, IConnectionListRepository>(repository), IConnectionListService
    {
    }
}
