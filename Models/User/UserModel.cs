using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VAirZoneWebAPI.Models.User
{
	public class UserModel
	{
		[Key]
		public int UserId { get; set; }

		[Column(TypeName = "nvarchar(64)")]
		public string DisplayName { get; set; }

		public bool Gender { get; set; }

		[Column(TypeName = "nvarchar(256)")]
		public string Bio { get; set; } = string.Empty;

		[Column(TypeName = "nvarchar(2048)")]
		public string AdditionalUrl { get; set; } = string.Empty;

		[Column(TypeName = "nvarchar(256)")]
		public string Email { get; set; }

		[Column(TypeName = "nvarchar(256)")]
		public string Password { get; set; }

		public DateTime CreatedOn { get; set; }

		public virtual UserCodeModel UserCode { get; set; }
	}
}