namespace API.DataAccess.Application
{
    public class ConnectionList
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public ICollection<ConnectionDetail> ConnectionDetails { get; set; }
    }
}
