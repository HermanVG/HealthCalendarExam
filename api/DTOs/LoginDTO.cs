namespace HealthCalendar.DTOs
{
    // DTO used when a user tries to log in
    public class LoginDTO
    { 
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}