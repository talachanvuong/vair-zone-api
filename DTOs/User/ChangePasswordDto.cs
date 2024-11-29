using System.ComponentModel.DataAnnotations;

namespace VAirZoneWebAPI.DTOs.User
{
    public class ChangePasswordDto
    {
        [Required]
        [MinLength(8)]
        [MaxLength(40)]
        public string OldPassword { get; set; }


        [Required]
        [MinLength(8)]
        [MaxLength(40)]
        public string NewPassword { get; set; }
    }
}