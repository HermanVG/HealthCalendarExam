using System;

namespace HealthCalendar.Models
{
    public class Assignment
    {
        // Primary Key
        public int AssignmentId { get; set; }
        public TimeOnly From { get; set; }
        public TimeOnly To { get; set; }
        public DateOnly Date { get; set; }
        public String? Message { get; set; }
        public String Location { get; set; } = String.Empty;

        // Foreign Key (User.Id)
        public int UserId { get; set; }
        // Navigation Property
        public virtual User Patient { get; set; } = default!;
    }
}