using MailKit.Net.Imap;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace API.Service
{
    public class ImapClientService(ILogger<ImapClientService> logger, IConfiguration configuration) : IDisposable
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ConcurrentDictionary<string, ImapClient> _imapClients = new ConcurrentDictionary<string, ImapClient>();
        private readonly ILogger<ImapClientService> _logger = logger;

        public void Dispose()
        {
            foreach (var client in _imapClients.Values)
            {
                if (client.IsConnected)
                {
                    client.Disconnect(true);
                }
                client.Dispose();
            }
            _imapClients.Clear();
        }

        public async Task<ImapClient> GetOrCreateClient(string server, int port, string username, string password, SecureSocketOptions options = SecureSocketOptions.SslOnConnect)
        {
            var key = $"{server}:{username}";

            if (_imapClients.TryGetValue(key, out var imapClient))
            {
                if (imapClient.IsConnected)
                {
                    return imapClient;
                }
                else
                {
                    _imapClients.TryRemove(key, out _);
                }
            }

            imapClient = new ImapClient();

            try
            {
                await imapClient.ConnectAsync(server, port, options);
                await imapClient.AuthenticateAsync(username, password);
                _imapClients[key] = imapClient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect and authenticate with IMAP server: {Server}", server);
                imapClient.Dispose();
                throw;
            }

            return imapClient;
        }
    }
}
