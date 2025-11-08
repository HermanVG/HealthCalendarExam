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
        public string Title { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        // Foreign Key (User.Id)
        public string UserId { get; set; } = string.Empty;
        // Navigation Property
        public virtual User Patient { get; set; } = default!;
    }
}