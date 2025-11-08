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
        string Name { get; set; } = string.Empty;
        enum Role { Patient, Worker, Admin }
        
        // For Patient, Points to related Worker
        int? WorkerId { get; set; } 
        // Navigation property
        User? worker { get; set; }

        // Navigation property 
        //Assignment? assignment { get; set; } (Comment back when Assignment is done)
        // Navigation property
        //Availability? availability { get; set; } (Comment back when Availability is done)
    }
}