using API.Dto;

namespace API.ServiceContract.Request
{
    public class MailboxRequest
    {
        public ConnectionDto Connection { get; set; }
        public List<MailboxFilterRequest> FilterMailbox { get; set; }
    }
}
