using API.ServiceContract;
using API.ServiceContract.Request;
using Framework.Application.Controllers;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Mvc;

namespace API.KotaMail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailboxController(IConnectionService connectionService, IMailboxService mailboxService) : ApiBaseController
    {
        private readonly IConnectionService _connectionService = connectionService;
        private readonly IMailboxService _mailboxService = mailboxService;

        [HttpGet("{path}")]
        public async Task<IActionResult> Mailbox(string path)
        {
            var response = await _connectionService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"IsPublic=true AND Path=\"{path}\""
            });

            if(response.IsError())
                return GetErrorJson(response);

            var connectionDto = response.DtoCollection.FirstOrDefault();
            var connectionDetailFilterDto = connectionDto.ConnectionDetails.FirstOrDefault()?.ConnectionDetailFilters;

            var mailboxResponse = await _mailboxService.GetMailboxAsync(new GenericRequest<MailboxRequest>
            {
                Data = new MailboxRequest
                {
                    Connection = connectionDto,
                    ConnectionDetailFilters = connectionDetailFilterDto
                }
            });

            if(mailboxResponse.IsError())
                return GetErrorJson(mailboxResponse);

            return GetSuccessJson(mailboxResponse, mailboxResponse.Data);
        }
    }
}
