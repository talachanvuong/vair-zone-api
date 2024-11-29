namespace VAirZoneWebAPI.DTOs.User
{
    public class UserDto
    {
        public string DisplayName { get; set; }
        public bool Gender { get; set; }
        public string? Bio { get; set; }
        public string? AdditionalUrl { get; set; }
        public string Email { get; set; }
    }
}