using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VAirZoneWebAPI.Models.Organizer
{
	public class ErrorCatcherModel
	{
        [Key]
        public int ErrorCatcherId { get; set; }

        public int OrganizerId { get; set; }

		[Column(TypeName = "nvarchar(2048)")]
		public string Problem { get; set; }

		public DateTime CreatedOn { get; set; }

		public virtual OrganizerModel Organizer { get; set; }
	}
}