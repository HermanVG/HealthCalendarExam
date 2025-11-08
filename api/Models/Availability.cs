using System;

namespace HealthCalendar.Models
{
    public class Availability
    {
        // Primary Key
        public int AvailabilityId { get; set; }
        public TimeOnly From { get; set; }
        public TimeOnly To { get; set; }

        // Specifies Day of Week 
        public enum DayOfWeek { Mon, Tue, Wed, Thu, Fri, Sun, Sat }
        // Specifies sepcific date, used to override DayOfWeek
        public DateOnly? Date { get; set; }

        // Foreign Key (User.Id)
        public int UserId { get; set; }
        // Navigation Property
        public virtual User Patient { get; set; } = default!;
    }
}