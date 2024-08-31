using API.Dto;
using API.RepositoryContract;
using API.ServiceContract;
using Framework.Service;

namespace API.Service
{
    public class ConnectionDetailService(IConnectionDetailRepository repository) : BaseUserService<ConnectionDetailDto, ulong, IConnectionDetailRepository>(repository), IConnectionDetailService
    {
    }
}
