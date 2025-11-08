using System.ComponentModel.DataAnnotations;

namespace HealthCalendar.DTOs
{
    // DTO used when a user tries to log in
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}