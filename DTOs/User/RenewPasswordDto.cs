using System.ComponentModel.DataAnnotations;

namespace VAirZoneWebAPI.DTOs.User
{
    public class RenewPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        public string Code { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(40)]
        public string NewPassword { get; set; }
    }
}