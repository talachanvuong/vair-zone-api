using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VAirZoneWebAPI.Models.Organizer
{
	public class OrganizerModel
	{
		[Key]
		public int OrganizerId { get; set; }

		[Column(TypeName = "nvarchar(256)")]
		public string OrganizerName { get; set; }

		[Column(TypeName = "nvarchar(2048)")]
		public string PageUrl { get; set; }

        public virtual ICollection<ActivityModel> Activities { get; set; }

        public virtual ICollection<ErrorCatcherModel> ErrorCatchers { get; set; }
    }
}
