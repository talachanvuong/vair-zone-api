using System.ComponentModel.DataAnnotations;

namespace VAirZoneWebAPI.DTOs.User
{
    public class RequestCodeDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}