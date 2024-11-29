using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VAirZoneWebAPI.Models.Organizer
{
	public class ActivityModel
    {
        [Key]
        public int ActivityId { get; set; }

		[Column(TypeName = "nvarchar(256)")]
		public string ActivityName { get; set; }

		[Column(TypeName = "nvarchar(2048)")]
		public string Description { get; set; }

		[Column(TypeName = "nvarchar(2048)")]
		public string PostUrl { get; set; }

		[Column(TypeName = "nvarchar(2048)")]
        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public int OrganizerId { get; set; }

        public virtual OrganizerModel Organizer { get; set; }
    }
}