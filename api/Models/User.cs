using Microsoft.AspNetCore.Identity;

namespace HealthCalendar.Models
{
    public class User : IdentityUser
    {
        /* 
            Inherits from IdentityUser,
            Properties from IdentityUser that User also uses are:
            Id (Primary key),
            Email,
            PasswordHash
        */
        public string Name { get; set; } = string.Empty;
        public enum Role { Patient, Worker, Admin }
        
        // Foreign Key (User.Id) 
        // For Patient, Points to related Worker
        public int? WorkerrId { get; set; } 
        // Navigation property
        public virtual User? Worker { get; set; }
    }
}