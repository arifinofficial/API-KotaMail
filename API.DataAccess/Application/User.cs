using Microsoft.AspNetCore.Identity;

namespace API.DataAccess.Application
{
    public class User : IdentityUser
    {
        public ICollection<Connection> Connections { get; set; }
        public ICollection<ConnectionDetail> ConnectionDetails { get; set; }
        public ICollection<ConnectionFilter> ConnectionDetailFilters { get; set; }
    }
}
