using API.Core;
using API.Dto;
using API.ServiceContract;
using API.ServiceContract.Request;
using API.ServiceContract.Response;
using Framework.Core;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using MailKit;
using MailKit.Search;
using Microsoft.Extensions.Configuration;

namespace API.Service
{
    public class MailboxService(IConfiguration configuration, ImapClientService imapClientService) : IMailboxService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ImapClientService _imapClientService = imapClientService;

        public async Task<GenericResponse<List<EmailSummaryResponse>>> GetEmailSummaries(GenericRequest<EmailSummaryRequest> request)
        {
            if (request.Data.Connection.ConnectionDetails.Count < 1)
                return null;

            var connectionDetail = request.Data.Connection.ConnectionDetails.First();

            var base64key = _configuration["Application:Key"];
            var base64Iv = _configuration["Application:IV"];
            var server = connectionDetail.Server;
            var port = connectionDetail.Port;
            var email = connectionDetail.Email;
            var password = Cryptography.Decrypt(connectionDetail.Password, base64key, base64Iv);
            var response = new GenericResponse<List<EmailSummaryResponse>>();

            try
            {
                var client = await _imapClientService.GetOrCreateClient(server, port, email, password);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                var messageSummaries = await GetSummaries(inbox, request.Data.ConnectionDetailFilters);

                // Disable temporary for spam message
                //var spamFolder = client.GetFolder(SpecialFolder.Junk);
                //if (spamFolder != null)
                //{
                //    await spamFolder.OpenAsync(FolderAccess.ReadOnly);
                //    var spamSummaries = await GetSummaries(spamFolder, request.Data.ConnectionDetailFilters);

                //    messageSummaries.AddRange(spamSummaries);
                //}

                messageSummaries.Reverse();
                response.Data = messageSummaries;
            }
            catch (Exception ex)
            {
                response.AddErrorMessage(ex.Message);
                return response;
            }

            return response;
        }
        private static async Task<List<EmailSummaryResponse>> GetSummaries(IMailFolder folder, ICollection<ConnectionDetailFilterDto> filterMail)
        {
            var uniqueIds = new List<UniqueId>();
            SearchQuery query = SearchQuery.All;

            if (filterMail != null)
            {
                var from = filterMail.FirstOrDefault(x => x.Key == CoreConstant.FilterMailboxParameter.From);
                if (from != null)
                {
                    if (!string.IsNullOrEmpty(from.Value))
                    {
                        query = query.And(SearchQuery.FromContains(from.Value));
                    }
                }

                var subject = filterMail.FirstOrDefault(x => x.Key == CoreConstant.FilterMailboxParameter.Subject);
                if (subject != null)
                {
                    if (!string.IsNullOrEmpty(subject.Value))
                    {
                        query = query.And(SearchQuery.SubjectContains(CoreConstant.FilterMailboxParameter.Subject));
                    }
                }
            }

            var uids = await folder.SearchAsync(query);
            var summaries = await folder.FetchAsync(uids, MessageSummaryItems.Envelope);
            var results = new List<EmailSummaryResponse>();

            foreach (var summary in summaries)
            {
                results.Add(new EmailSummaryResponse
                {
                    Uid = summary.UniqueId.Id,
                    Subject = summary.Envelope.Subject,
                    From = summary.Envelope.From.ToString(),
                    ReceivedDate = summary.Envelope.Date
                });
            }

            return results;
        }

        public async Task<GenericResponse<EmailDetailResponse>> GetEmailDetail(GenericRequest<EmailDetailRequest> request)
        {
            var connectionDetail = request.Data.Connection.ConnectionDetails.First();

            var base64key = _configuration["Application:Key"];
            var base64Iv = _configuration["Application:IV"];
            var server = connectionDetail.Server;
            var port = connectionDetail.Port;
            var email = connectionDetail.Email;
            var password = Cryptography.Decrypt(connectionDetail.Password, base64key, base64Iv);
            var response = new GenericResponse<EmailDetailResponse>();

            try
            {
                var client = await _imapClientService.GetOrCreateClient(server, port, email, password);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                var uid = new UniqueId(request.Data.Uid);
                var message = await inbox.GetMessageAsync(uid);

                response.Data = new EmailDetailResponse
                {
                    HtmlBody = message.HtmlBody,
                    TextBody = message.TextBody
                };
            }
            catch (Exception ex)
            {
                response.AddErrorMessage(ex.Message);
                return response;
            }

            return response;
        }
    }
}
