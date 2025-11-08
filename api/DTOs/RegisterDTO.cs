namespace HealthCalendar.DTOs
{
    // DTO used when a user is registered
    public class RegisterDTO
    { 
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}