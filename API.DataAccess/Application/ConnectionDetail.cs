namespace API.DataAccess.Application
{
    public class ConnectionDetail
    {
        public ulong Id { get; set; }
        public string UserId { get; set; }
        public ulong ConnectionId { get; set; }
        public ulong ConnectionListId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConnectionType { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public User User { get; set; }
        public Connection Connection { get; set; }
        public ConnectionList ConnectionList { get; set; }
    }
}
