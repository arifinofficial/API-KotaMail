using API.Core;
using API.ServiceContract;
using API.ServiceContract.Request;
using API.ServiceContract.Response;
using Framework.Core;
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
    public class MailboxService(IConfiguration configuration) : IMailboxService
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<GenericResponse<List<MailboxResponse>>> GetMailboxAsync(GenericRequest<MailboxRequest> request)
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
            var response = new GenericResponse<List<MailboxResponse>>();

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
                var messages = new List<MailboxResponse>();

                foreach (var message in messagesList)
                {
                    messages.Add(new MailboxResponse
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

        private static async Task<List<MimeMessage>> ReadEmailsFolder(IMailFolder folder, List<MailboxFilterRequest> filterMailbox)
        {
            var messages = new List<MimeMessage>();
            SearchQuery query = SearchQuery.All;

            if(filterMailbox  != null)
            {
                var from = filterMailbox.FirstOrDefault(x => x.Key == CoreConstant.FilterMailboxParameter.From);
                if(from != null)
                {
                    if (!string.IsNullOrEmpty(from.Value))
                    {
                        query = query.And(SearchQuery.FromContains(from.Value));
                    }
                }

                var subject = filterMailbox.FirstOrDefault(x => x.Key == CoreConstant.FilterMailboxParameter.Subject);
                if(subject != null)
                {
                    if (!string.IsNullOrEmpty(subject.Value))
                    {
                        query = query.And(SearchQuery.SubjectContains(CoreConstant.FilterMailboxParameter.Subject));
                    }
                }
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
