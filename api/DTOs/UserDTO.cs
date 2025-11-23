using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HealthCalendar.Models
{
    public class UserDTO
    {
        /* 
            Inherits from IdentityUser,
            Properties from IdentityUser that User also uses are:
            Id (Primary key),
            Username (Works as email, since Username is non-nullable),
            PasswordHash
        */

        // Primary Key
        public string Id {get; set;} = string.Empty;
        
        // Works as email, since Username is non-nullable for IdentityUser
        [Required]
        [EmailAddress]
        public string UserName {get; set;} = string.Empty;
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        // Role can be "Patient", "Worker" or "Admin"
        [Required]
        public string Role { get; set; } = string.Empty;
        
        // Foreign Key (User.Id)
        // For Patient, Points to related Worker
        public string? WorkerId { get; set; } 
    }
}