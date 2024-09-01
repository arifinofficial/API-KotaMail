using API.Dto;
using Framework.ServiceContract;

namespace API.ServiceContract
{
    public interface IConnectionService : IBaseUserService<ConnectionDto, ulong>
    {
    }
}
