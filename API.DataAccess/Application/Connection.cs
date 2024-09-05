namespace API.DataAccess.Application
{
    public class Connection
    {
        public ulong Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public string Path { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public User User { get; set; }
        public ICollection<ConnectionDetail> ConnectionDetails { get; set; }
        public ICollection<ConnectionFilter> ConnectionFilters { get; set; }
    }
}
