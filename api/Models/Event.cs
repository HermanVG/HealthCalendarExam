using System;

namespace HealthCalendar.Models
{
    public class Event
    {
        // Primary Key
        public int EventId { get; set; }
        public TimeOnly From { get; set; }
        public TimeOnly To { get; set; }
        public DateOnly Date { get; set; }
        public String Title { get; set; } = String.Empty;
        public String Location { get; set; } = String.Empty;

        // Foreign Key (User.Id)
        public int UserId { get; set; }
        // Navigation Property
        public virtual User Patient { get; set; } = default!;
    }
}