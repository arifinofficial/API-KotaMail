using Framework.Dto;

namespace API.Dto
{
    public class ConnectionDetailDto : BaseUserDto<ulong>
    {
        public ulong ConnectionId { get; set; }
        public ulong ConnectionListId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConnectionType { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }

        public ICollection<ConnectionDetailFilterDto> ConnectionDetailFilters { get; set; }
    }
}
