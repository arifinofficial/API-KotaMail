using Framework.Dto;

namespace API.Dto
{
    public class ConnectionDto : BaseUserDto<ulong>
    {
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public string Path { get; set; }

        public ICollection<ConnectionDetailDto> ConnectionDetails { get; set; }
        public ICollection<ConnectionFilterDto> ConnectionFilters { get; set; }
    }
}
