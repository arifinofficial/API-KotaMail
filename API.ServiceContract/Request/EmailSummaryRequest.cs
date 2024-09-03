using API.Dto;

namespace API.ServiceContract.Request
{
    public class EmailSummaryRequest
    {
        public ConnectionDto Connection { get; set; }
        public ICollection<ConnectionDetailFilterDto> ConnectionDetailFilters { get; set; }
    }
}
