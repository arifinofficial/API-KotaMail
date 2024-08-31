using API.Dto;
using API.RepositoryContract;
using API.ServiceContract;
using Framework.Service;

namespace API.Service
{
    public class ConnectionService(IConnectionRepository repository) : BaseUserService<ConnectionDto, ulong, IConnectionRepository>(repository), IConnectionService
    {
    }
}
