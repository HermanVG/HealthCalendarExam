using System;

namespace HealthCalendar.Models
{
    public class Schedule
    {
        // Primary Key
        int SchId { get; set; }
        
        // Foreign Key (Availability.AvId)
        int AvId { get; set; }
        // Navigation property
        Availability availability { get; set; } = default!;

        // Foreign Key (Assignment.AssId)
        int AssId { get; set; }
        // Navigation property
        Assignment assignment { get; set; } = default!;
    }
}