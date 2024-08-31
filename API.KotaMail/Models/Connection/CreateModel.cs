using System.ComponentModel.DataAnnotations;

namespace API.KotaMail.Models.Connection
{
    public class CreateModel
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string ConnectionType { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }

        public bool IsPublic { get; set; } = false;

        [Required]
        public ulong ConnectionListId { get; set; }
    }
}
