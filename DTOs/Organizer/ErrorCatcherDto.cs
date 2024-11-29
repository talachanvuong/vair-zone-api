namespace VAirZoneWebAPI.DTOs.Organizer
{
    public class ErrorCatcherDto
    {
        public int ErrorCatcherId { get; set; }
        public string OrganizerName { get; set; }
        public string Problem { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
