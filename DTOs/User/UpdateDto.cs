using System.ComponentModel.DataAnnotations;

namespace VAirZoneWebAPI.DTOs.User
{
    public class UpdateDto
    {
        [MinLength(3)]
        [MaxLength(40)]
        public string? DisplayName { get; set; }

        [MaxLength(256)]
        public string? Bio { get; set; }

        [MaxLength(2048)]
        public string? AdditionalUrl { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}