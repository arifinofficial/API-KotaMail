using System.ComponentModel.DataAnnotations;

namespace API.KotaMail.Models.Connection
{
    public class UpdateModel
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }

        public bool IsPublic { get; set; } = false;
    }
}
