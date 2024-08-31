using Framework.Dto;

namespace API.Dto
{
    public class ConnectionListDto : BaseDto<ulong>
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
    }
}
