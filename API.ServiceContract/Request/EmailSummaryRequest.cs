using API.Dto;

namespace API.ServiceContract.Request
{
    public class EmailSummaryRequest
    {
        public ConnectionDto Connection { get; set; }
        public ICollection<ConnectionFilterDto> ConnectionDetailFilters { get; set; }
    }
}
