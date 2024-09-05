using Framework.Dto;

namespace API.Dto
{
    public class ConnectionFilterDto : BaseUserDto<ulong>
    {
        public ulong ConnectionId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
