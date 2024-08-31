namespace API.Dto
{
    public class MailboxDto
    {
        public ConnectionDto Connection { get; set; }
        public List<Dictionary<string, string>> FilterMailbox { get; set; }
    }
}
