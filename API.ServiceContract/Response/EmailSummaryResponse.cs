namespace API.ServiceContract.Response
{
    public class EmailSummaryResponse
    {
        public uint Uid { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public DateTimeOffset? ReceivedDate { get; set; }
    }
}
