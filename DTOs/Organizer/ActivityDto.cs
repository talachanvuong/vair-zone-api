namespace VAirZoneWebAPI.DTOs.Organizer
{
    public class ActivityDto
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public string PostUrl { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
