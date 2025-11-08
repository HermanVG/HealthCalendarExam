using System;

namespace HealthCalendar.Models
{
    public class Assignment
    {
        // Primary Key
        int AssId { get; set; }
        TimeOnly From { get; set; }
        TimeOnly To { get; set; }
        DateOnly Date { get; set; }
        String? Message { get; set; }
        String Location { get; set; } = String.Empty;

        // Foreign Key (User.Id)
        int UserId { get; set; }
        // Navigation Property
        User Patient { get; set; } = default!;
    }
}