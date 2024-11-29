using System.ComponentModel.DataAnnotations;

namespace VAirZoneWebAPI.DTOs.User
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(40)]
        public string Password { get; set; }
    }
}