using API.Dto;
using Framework.ServiceContract;

namespace API.ServiceContract
{
    public interface IConnectionListService : IBaseService<ConnectionListDto, ulong>
    {
    }
}
