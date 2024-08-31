using API.Core;
using API.Dto;
using API.RepositoryContract;
using API.ServiceContract;
using Framework.Core;
using Framework.Service;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace API.Service
{
    public class ConnectionService(IConnectionRepository repository, IConfiguration configuration) : BaseUserService<ConnectionDto, ulong, IConnectionRepository>(repository), IConnectionService
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<GenericResponse<List<MailboxMessageDto>>> GetMailboxAsync(GenericRequest<MailboxDto> request)
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
            var response = new GenericResponse<List<MailboxMessageDto>>();

            using var client = new ImapClient();
            try
            {
                await client.ConnectAsync(server, port, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(email, password);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                var inboxMessages = await ReadEmailsFolder(inbox, request.Data.FilterMailbox);

                var spamMessages = new List<MimeMessage>();
                var spamFolder = client.GetFolder(SpecialFolder.Junk);
                if (spamFolder != null)
                {
                    await spamFolder.OpenAsync(FolderAccess.ReadOnly);
                    await ReadEmailsFolder(spamFolder, request.Data.FilterMailbox);
                }

                var messagesList = inboxMessages.Union(spamMessages).ToList();
                var messages = new List<MailboxMessageDto>();

                foreach (var message in messagesList)
                {
                    messages.Add(new MailboxMessageDto
                    {
                        HtmlBody = message.HtmlBody,
                        TextBody = message.TextBody,
                    });
                }

                response.Data = messages;

                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                response.AddErrorMessage(ex.Message);
                return response;
            }

            return response;
        }

        private static async Task<List<MimeMessage>> ReadEmailsFolder(IMailFolder folder, List<Dictionary<string, string>> filterMailbox)
        {
            var messages = new List<MimeMessage>();
            SearchQuery query = SearchQuery.All;

            var from = filterMailbox.Find(x => x.ContainsKey(CoreConstant.FilterMailboxParameter.From));
            if (from != null)
            {
                query = query.And(SearchQuery.FromContains(from[CoreConstant.FilterMailboxParameter.From]));
            }

            var subject = filterMailbox.Find(x => x.ContainsKey(CoreConstant.FilterMailboxParameter.Subject));
            if (subject != null)
            {
                query = query.And(SearchQuery.SubjectContains(subject[CoreConstant.FilterMailboxParameter.Subject]));
            }

            var uids = await folder.SearchAsync(query);

            if (uids.Count > 0)
            {
                foreach (var uid in uids)
                {
                    var message = await folder.GetMessageAsync(uid);
                    messages.Add(message);
                }
            }

            messages.Reverse();

            return messages;
        }
    }
}
