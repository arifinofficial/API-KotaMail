using API.Dto;
using Framework.ServiceContract;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;

namespace API.ServiceContract
{
    public interface IConnectionService : IBaseUserService<ConnectionDto, ulong>
    {
        Task<GenericResponse<List<MailboxMessageDto>>> GetMailboxAsync(GenericRequest<MailboxDto> request);
    }
}
