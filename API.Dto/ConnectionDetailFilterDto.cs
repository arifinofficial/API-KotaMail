using Framework.Dto;

namespace API.Dto
{
    public class ConnectionDetailFilterDto : BaseUserDto<ulong>
    {
        public ulong ConnectionDetailId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
