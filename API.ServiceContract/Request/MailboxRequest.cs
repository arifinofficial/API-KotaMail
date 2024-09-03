using API.Dto;

namespace API.ServiceContract.Request
{
    public class MailboxRequest
    {
        public ConnectionDto Connection { get; set; }
        public ICollection<ConnectionDetailFilterDto> ConnectionDetailFilters { get; set; }
    }
}
