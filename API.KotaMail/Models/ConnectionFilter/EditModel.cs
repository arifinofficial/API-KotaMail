using System.ComponentModel.DataAnnotations;

namespace API.KotaMail.Models.ConnectionFilter
{
    public class EditModel
    {
        [Required]
        public ulong Id { get; set; }

        [Required]
        public ulong ConnectionId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Key { get; set; }

        [Required]
        [MaxLength(256)]
        public string Value { get; set; }
    }
}
