using API.ServiceContract.Request;
using API.ServiceContract.Response;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;

namespace API.ServiceContract
{
    public interface IMailboxService
    {
        Task<GenericResponse<List<EmailSummaryResponse>>> GetEmailSummaries(GenericRequest<EmailSummaryRequest> request);

        Task<GenericResponse<EmailDetailResponse>> GetEmailDetail(GenericRequest<EmailDetailRequest> request);
    }
}
