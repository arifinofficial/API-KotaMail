using API.Core;
using API.Dto;
using API.ServiceContract;
using Framework.Application.Controllers;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Mvc;

namespace API.KotaMail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailboxController(IConnectionService connectionService) : ApiBaseController
    {
        private readonly IConnectionService _connectionService = connectionService;

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

            var mailboxResponse = await _connectionService.GetMailboxAsync(new GenericRequest<Dto.MailboxDto>
            {
                Data = new MailboxDto
                {
                    Connection = response.DtoCollection.FirstOrDefault(),
                    FilterMailbox = [
                        new Dictionary<string, string> {
                            {
                                CoreConstant.FilterMailboxParameter.Subject,
                                "Netflix Update"
                            }
                        } 
                    ]
                }
            });

            if(mailboxResponse.IsError())
                return GetErrorJson(mailboxResponse);

            return GetSuccessJson(mailboxResponse, mailboxResponse.Data);
        }
    }
}
