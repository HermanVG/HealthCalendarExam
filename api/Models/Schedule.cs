using System;

namespace HealthCalendar.Models
{
    public class Schedule
    {
        // Primary Key
        public int ScheduleId { get; set; }
        
        // Foreign Key (Availability.AvailabilityId)
        public int AvailabilityId { get; set; }
        // Navigation property
        public virtual Availability availability { get; set; } = default!;

        // Foreign Key (Assignment.AssignmentId)
        public int AssignmentId { get; set; }
        // Navigation property
        public virtual Assignment assignment { get; set; } = default!;
    }
}