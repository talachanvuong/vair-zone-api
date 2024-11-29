using System.ComponentModel.DataAnnotations;

namespace VAirZoneWebAPI.DTOs.User
{
	public class RegisterDto
	{
		[Required]
		[MinLength(3)]
		[MaxLength(40)]
		public string DisplayName { get; set; }

		[Required]
		public bool Gender { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MinLength(8)]
		[MaxLength(40)]
		public string Password { get; set; }
	}
}