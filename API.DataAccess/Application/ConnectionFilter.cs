namespace API.DataAccess.Application
{
    public class ConnectionFilter
    {
        public ulong Id { get; set; }
        public string UserId { get; set; }
        public ulong ConnectionId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public User User { get; set; }
    }
}
