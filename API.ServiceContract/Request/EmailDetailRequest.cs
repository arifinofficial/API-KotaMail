using API.Dto;

namespace API.ServiceContract.Request
{
    public class EmailDetailRequest
    {
        public ConnectionDto Connection { get; set; }
        public uint Uid { get; set; }
    }
}
