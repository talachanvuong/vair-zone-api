using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VAirZoneWebAPI.Models.User
{
	public class UserCodeModel
	{
		[Key]
		public int UserId { get; set; }

		[Column(TypeName = "nvarchar(4)")]
		public string Code { get; set; }

		public DateTime TimeExpired { get; set; }

		public virtual UserModel User { get; set; }
	}
}
